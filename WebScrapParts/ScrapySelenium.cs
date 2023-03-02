using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Net;
using WebScrapParts.Entities;
using WebScrapParts.Helpers;
using WebScrapParts.Infraestructure.Persistence.Contexts;

namespace WebScrapParts
{
    public class ScrapySelenium
    {
        IWebDriver driver = new ChromeDriver();

        public bool NavegoAlLink(string link, out string linkReal)
        {
            // link = ProductoLinkDrivCat.RegresaLink(codigo.CategoriaProducto, codigo.CodigoOriginal);
            // link = codigo.Link1;
            linkReal = link;

            driver.Navigate().GoToUrl(link.Trim());
            WebDriverWait temporizadorAplicacion = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Thread.Sleep(3000);

            // esperamos a que carge la tabla de aplicaciones
            WebDriverWait temporizadorTablaApp = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // si te muestra la etiqueta de que no se encontro nada
            var noSeEncontro = driver.FindElements(By.ClassName("driv-part-results-no-results"));

            if ( noSeEncontro.Count > 0 )
            {
                return false;
            }

            return true;
        }

        public async void ScrapDrivParts(List<AppProductosScrapy> codigosCategoria)
        {
            TimeSpan timeSpan= TimeSpan.FromSeconds(1);
            var db = new WebScrapPartsDbContext();
            var aplicaciones = new List<AplicacionBanco>();
            var specs = new Specs();
            var manejoCarpeta = new ManejoCarpeta();
            var bajar = new WebClient();
            string linkReal;

            foreach (var codigo in codigosCategoria)
            {
                aplicaciones.Clear();

                //if ( codigo.Encontrado )
                //{
                //    continue;
                //}

                // verifico si es vacio el codigo llave brinco al siguiente registro
                if( string.IsNullOrEmpty( codigo.EquivalenciaLlave.Trim() ) )
                {
                    continue;
                }

                codigo.CodigoProdMaestro = codigo.CodigoProdMaestro.Replace('/','_');

                manejoCarpeta.Creacion( 
                    categoria: codigo.Categoria, 
                    catalogoOnline: codigo.SitioWebNombre,
                    codigoBase: codigo.CodigoProdMaestro,
                    unidad: codigo.DirFotos.Trim()
                );

                // abre el navegador y navega hasta el link1
                bool encontroApplicaciones = NavegoAlLink(codigo.Link1, out linkReal);

                // si no navego al Link y no tiene un link2 va a entrar
                if ( !encontroApplicaciones && String.IsNullOrEmpty(codigo.Link2.Trim()))
                {
                    continue;
                }

                // si no encontro el link1 y el link2 tiene valor va a entrar
                if ( !encontroApplicaciones && !String.IsNullOrEmpty(codigo.Link2.Trim()))
                {
                    // navego al link2
                    encontroApplicaciones = NavegoAlLink(codigo.Link2, out linkReal);

                    // si no encontro informacion pero la pagina si existe
                    if ( !encontroApplicaciones )
                    {
                        continue;
                    }

                }
                             
                var appProductosScrapy = db.AppProductosScrapy.AsNoTracking().FirstOrDefault(x => x.Id == codigo.Id);

                if(appProductosScrapy != null)
                {
                    appProductosScrapy.Encontrado = true;
                    db.Update(appProductosScrapy);
                    await db.SaveChangesAsync(); 
                }

                WebDriverWait temporizadorTablaApp2 = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                // Aqui esta el control de las etiquetas de Specs application media
                var menuSpecsApp = driver.FindElements(By.XPath("//*[@id=\"page-content\"]/div/div[2]/div/div[1]/div/div/div[3]/div/div[1]/ul//li"));

                bool tienePestanaApplications = false;
                int indiceOpcApp = 0;

                foreach (var item in menuSpecsApp)
                {
                    indiceOpcApp++;

                    if ( item.Text == "APPLICATIONS" || item.Text == "APLICACIONES")
                    {
                        tienePestanaApplications = true;
                        break;
                    }
                }

                // Si no tiene pestanas 
                if ( !tienePestanaApplications ) 
                {
                    continue;
                }

                //                         //*[@id="page-content"]/div/div/div/div/div[2]/div/div/div/div[2]/div[1]/div/div/div[2]
                string tabAplicaciones = $"//*[@id=\"page-content\"]/div/div[2]/div/div[1]/div/div/div[3]/div/div[1]/ul/li[{indiceOpcApp}]";

                var opcApp = driver.FindElement(By.XPath(tabAplicaciones));

                opcApp.Click();

                // Si solo esta la opcion de APPLICATIONS
                string rutaXpath;

                if (indiceOpcApp == 1) // si solo esta APPLICATIONS
                {
                    rutaXpath = "//*[@id=\"page-content\"]/div/div[2]/div/div[1]/div/div/div[3]/div/div[2]/div[1]/div/div[1]/div[2]/div[1]/div/table/tbody//td";
                }
                else // SI HAY SPECS APPLICATIONS MEDIA
                {
                    rutaXpath = "//*[@id=\"page-content\"]/div/div[2]/div/div[1]/div/div/div[3]/div/div[3]/div[1]/div/div[1]/div[2]/div[1]/div/table/tbody//td";
                }

                var columnasCantidad = driver.FindElements(By.XPath(rutaXpath));

                for (int i = 10; i <= columnasCantidad.Count; i += 10)
                {
                    var lstItems = columnasCantidad.Skip(i - 10).Take(10).ToList();

                    for (int a = 0; a < lstItems.Count; a++)
                    {
                        AplicacionBanco app = new(0,
                                                  codigo.IdProdMaestro,
                                                  codigo.CodigoProdMaestro.Trim(),
                                                  codigo.EquivalenciaLlave.Trim(),
                                                  codigo.Categoria.Trim(),
                                                  manejoCarpeta.PathCompleto.Trim(),
                                                  linkReal.Trim(),
                                                  lstItems[a++].Text.Trim(),
                                                  lstItems[a++].Text.Trim(),
                                                  lstItems[a].Text.Trim(),
                                                  lstItems[a].Text.Trim(),
                                                  lstItems[a++].Text.Trim(),
                                                  lstItems[a++].Text.Trim(),
                                                  lstItems[a++].Text.Trim(),
                                                  lstItems[a++].Text.Trim(),
                                                  lstItems[a++].Text.Trim(),
                                                  lstItems[a++].Text.Trim(),
                                                  lstItems[a++].Text.Trim(),
                                                  lstItems[a++].Text.Trim(),
                                                  "N/A");

                        aplicaciones.Add(app);

                        Console.WriteLine($" { app.Id }, { app.IdProdMaestroNumber}, { app.ProdMaestroNumber }, { app.OemNumber }, { app.Category } ");
                    }
                }

                // se le agrega //img para que busque solo las imagenes
                var imagesProduct = driver.FindElements(By.XPath(
                      "//*[@id=\"page-content\"]/div/div[2]/div/div[1]/div/div/div[2]/div[1]/div[1]//img"
                ));

                for (int i = 0; i < imagesProduct.Count(); i++)
                {
                    bajar.DownloadFile(imagesProduct[i].GetAttribute("src"), $"{ manejoCarpeta.PathCompleto.Trim() }{ codigo.CodigoProdMaestro.Trim() }_{i}.png");
                }

                // Guardamos en la tabla de banco de aplicaciones
                db.AplicacionBanco.AddRange( aplicaciones );
                db.SaveChanges();
            }
        }
    }
}