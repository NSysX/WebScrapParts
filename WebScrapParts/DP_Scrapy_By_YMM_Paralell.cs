using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using WebScrapParts.Entities;
using WebScrapParts.Infraestructure.Persistence.Contexts;

namespace WebScrapParts
{
    public class DP_Scrapy_By_YMM_Paralell
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(55);
        IWebDriver driver = new ChromeDriver();

        public List<string> ObtenerLinksPaginaLlave(AppYearMakeModel appYearMakeModel)
        {
            List<string> linksPaginados = new();
            WebDriverWait espera = new WebDriverWait(driver, timeSpan);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl($@"https://www.drivparts.com/");
            bool encontrado = false;

            // 1 .- ANIO
            // Llena el combobox 
            //   Thread.Sleep(1000);
            espera.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"years\"]/div/div[1]/input")));
            driver.FindElement(By.XPath("//*[@id=\"years\"]/div/div[1]/input")).SendKeys(appYearMakeModel.YearId.ToString().Trim());
            driver.FindElement(By.XPath("//*[@id=\"years\"]/div/div[1]/input")).SendKeys(Keys.Enter);
            //  FIN ANIO

            // 2.- ARMADORA
            // Llena el ComboBox de Armadoras
           // Thread.Sleep(2000);
            espera.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"makes\"]/div/div[1]/input")));
            driver.FindElement(By.XPath("//*[@id=\"makes\"]/div/div[1]/input")).Click();
            //Thread.Sleep(1000);

            //*[@id="makes"]
            //*[@id="makes"]/ul//li
            //*[@id="makes"]/ul
            //*[@id="makes"]/ul/li[1]/a
            //*[@id="makes"]/ul/li[1]/a
            //*[@id="makes"]//a
           // listaDatosXpath = "//*[@id=\"makes\"]//a";
            Thread.Sleep(500);
           // espera.Until(ExpectedConditions.ElementIsVisible(By.XPath(listaDatosXpath)));
            var armadoras = driver.FindElements(By.XPath("//*[@id=\"makes\"]/ul//li"));
            encontrado = false;

            foreach (var armadora in armadoras)
            {
                if (armadora.GetAttribute("innerText").Trim().ToUpper() == appYearMakeModel.MakeName.Trim().ToUpper())
                {
                    encontrado = true;
                    break;
                }
            }

            // BUSCO QUE EXISTA LA ARMADORA 
            if (!encontrado)
            {
                Console.WriteLine("============ PARALLEL xxxxx {0}", appYearMakeModel.MakeName);
                return new();
            }

            driver.FindElement(By.XPath("//*[@id=\"makes\"]/div/div[1]/input")).SendKeys(appYearMakeModel.MakeName.Trim());
            driver.FindElement(By.XPath("//*[@id=\"makes\"]/div/div[1]/input")).SendKeys(Keys.Enter);
            //  FIN ARMADORA

            // 3.- MODELO
            // Llena el ComboBox de Modelos
            //    Thread.Sleep(2000);
            espera.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"models\"]/div/div[1]/input")));
            driver.FindElement(By.XPath("//*[@id=\"models\"]/div/div[1]/input")).Click();
            // listaDatosXpath = "//*[@id=\"models\"]/ul//li";
            Thread.Sleep(3000);
            var modelos = driver.FindElements(By.XPath("//*[@id=\"models\"]/ul/li//a"));
            encontrado = false;

            foreach (var modelo in modelos)
            {
                Console.WriteLine($"{modelo.GetAttribute("innerText").Trim().ToUpper() }");
                if (modelo.GetAttribute("innerText").Trim().ToUpper() == appYearMakeModel.ModelName.Trim().ToUpper())
                {
                    encontrado = true;
                    break;
                }
            }

            if (encontrado)
            {
                Console.WriteLine("Encontrado => {0},{1},{2}", appYearMakeModel.YearId, appYearMakeModel.MakeName, appYearMakeModel.ModelName);
                //--- actualizo el campo encontrado a verdadero
                //db.AppYearMakeModel.Where(x => x.Id == registro.Id)
                //    .ExecuteUpdate(actualiza => actualiza.SetProperty(r => r.Found, true));
            }
            else
            {
                Console.WriteLine("============ MODELO NO ENCONTRADO => {0},{1},{2}", appYearMakeModel.YearId, appYearMakeModel.MakeName, appYearMakeModel.ModelName);
                return new(); // lista vacia
            }

            driver.FindElement(By.XPath("//*[@id=\"models\"]/div/div[1]/input")).SendKeys(appYearMakeModel.ModelName.Trim());
            driver.FindElement(By.XPath("//*[@id=\"models\"]/div/div[1]/input")).SendKeys(Keys.Enter);

            // FIN MODELO -----------

            // 4.- CLICK BOTON SEARCH 
            driver.FindElement(By.XPath(
                                  "//*[@id=\"page-content\"]/div/div[1]/div/div/div/div[2]/div/div/div/div[2]/div/div[2]/button")).Click();
            // FIN BOTON 

            // ------- AQUI HAY QUE HACER EL CICLO DE NUEVO PARA CADA PAGINA DE RESULTADOS
            // OBTENENOS EL URL Y LA CANTIDAD DE PAGINAS DIV PAGINACION
            string divPaginacion = "//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]";
            espera.Until(ExpectedConditions.ElementIsVisible(By.XPath(divPaginacion)));
            var paginacion = driver.FindElements(By.XPath(divPaginacion));

            //--- YA DEBO TENER UNA URL
            string urlActual = driver.Url;

            // si no tiene paginacion es una sola pagina
            if (paginacion.Count() == 0)
            {
                // LINK VACIO POR QUE NO TIENE EL DIV DE PAGINACION
                Console.WriteLine("NO ENCONTRO PAGINACION");
                return linksPaginados;
            }

            espera.Until(ExpectedConditions.ElementToBeClickable(
                         By.XPath("//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]/input")));

            var numeroActualPagina = driver.FindElement(
                         By.XPath("//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]/input"));

            int totalPaginas = Int32.Parse(paginacion[0].Text.Replace("PAGE OF ", "").Trim());
            int paginaActual = Int32.Parse(numeroActualPagina.GetAttribute("value").Trim());

            Console.WriteLine();
            Console.WriteLine($"Total Paginas Actualizado = {totalPaginas}");
            Console.WriteLine($"Termina PaginaActual = {paginaActual}");
            Console.WriteLine();

            for (int i = 1; i <= totalPaginas; i++)
            {
                linksPaginados.Add(urlActual + "&page=" + i.ToString().Trim());
                Console.WriteLine($"Pagina { linksPaginados.Count() }");
            }

            return linksPaginados;
        }

        public async Task DPScrapByYmm_Parallel(List<AppYearMakeModel> yearMakeModels)
        {
            var db = new WebScrapPartsDbContext();
            string linkReal = $@"https://www.drivparts.com/";

            //IWebDriver driver = new ChromeDriver();
            //driver.Manage().Window.Maximize();
            WebDriverWait espera2 = new WebDriverWait(driver, timeSpan);
            List<AppYearMakeModelDetails> aYMMD = new();
            AppYearMakeModelDetails appYearMakeModelDetails = new();
            string refaccionesXpath = "";
            string detallesRefaccion = "";

            await Task.Delay(100);

            // Neceito Obtener el linkLlave
            foreach (var registro in yearMakeModels)
            {
                var linksPaginado = ObtenerLinksPaginaLlave(registro);

                // Si no se encontro nada
                if (linksPaginado.Count() == 0)
                {
                    continue;
                }

                // si trae la segunda pagina
                Parallel.ForEach(linksPaginado, (link, stateLoop) =>
                {
                    IWebDriver driver = new ChromeDriver();
                    driver.Navigate().GoToUrl(link);
                });


                while (true)
                {
                    Console.WriteLine("PARALLEL Entro al While wwwwwwwww");
                    Thread.Sleep(3000);

                    // Verifico que hay Resultados todas las listas
                     espera2.Until(ExpectedConditions.ElementIsVisible(
                                             By.XPath("//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[2]/ul")));

                    refaccionesXpath = "//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[2]/ul//li";
                    var refacciones = driver.FindElements(By.XPath(refaccionesXpath));

                    // si no encuentra resultados de Refacciones
                    if (refacciones.Count == 0)
                    {
                        Console.WriteLine("NO HAY RESULTADOS DE REFACCIONES xxxxxxxxxxxxx");
                        continue;
                    }

                    Console.WriteLine("Refacciones Count => {0}", refacciones.Count);

                    Parallel.ForEach(refacciones, (refaccion, stateLoop) =>
                    {
                        // si refaccion esta vacia 
                        if (string.IsNullOrEmpty(refaccion.Text.Trim()))
                        {
                            Console.WriteLine("|========== Esta vacio la refaccion ==========|");
                            return;
                        }

                        // reemplazo \r\n en ,
                        detallesRefaccion = refaccion.Text.Replace("\r\n", ",");

                        // mando crear el objeto 
                        appYearMakeModelDetails = new AppYearMakeModelDetails(registro.Id, detallesRefaccion);

                        Console.WriteLine($"aplicacion => {refaccion.Text}");

                        // Agrego los objetos de cada refaccion a la lista
                        aYMMD.Add(appYearMakeModelDetails);

                        Console.WriteLine("-----------------------------------------------------------");
                    });

                    break;
                }

                //if (paginacion.Count == 0)
                //{
                //    Console.WriteLine();
                //    Console.WriteLine("NO EXISTE EL BOTON DE PAGINACION @@@@@@@@@@@");
                //    Console.WriteLine();
                //    break;
                //}

                //if (paginaActual < totalPaginas)
                //{
                //    paginaActual++;
                //    numeroActualPagina.SendKeys(Keys.Backspace);
                //    numeroActualPagina.SendKeys(Keys.Backspace);
                //    numeroActualPagina.SendKeys(paginaActual.ToString().Trim());
                //    numeroActualPagina.SendKeys(Keys.Enter);
                //    continue;
                //}

                //stateLoop.Break();

                aYMMD.Clear();

                Console.WriteLine();
                Console.WriteLine("Fin del Registro ************************");


            }


            Console.WriteLine();
            Console.WriteLine("Fin de la Aplicacion ************************");





            // grabo todos los detalles de la refacciones encontradas
            // Thread.Sleep(3000);

            // si existe el div de paginacion
            // var existeDivPaginacion = espera.(ExpectedConditions.ElementExists(By.ClassName(divPaginacion)));
            //*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]/button[2]

            // espera.Until(ExpectedConditions.ElementIsVisible(By.XPath(divPaginacion)));



            //-- Verifico que hay Resultados todas las listas
            //espera.Until(ExpectedConditions.ElementExists(
            //             By.XPath("//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]//button")));
            //espera.Until(ExpectedConditions.ElementIsVisible(
            //             By.XPath("//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]//button")));
            //espera.Until(ExpectedConditions.ElementToBeClickable(
            //             By.XPath("//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]//button")));

            //var paginacionBotones = driver.FindElements(
            //             By.XPath("//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]//button"));



            //Console.WriteLine($"Texto del Boton = {paginacionBotones[1].GetAttribute("innerHTML")}");

            //-- segundo boton
            //if (paginacionBotones[1].Enabled)
            //{
            //    paginacionBotones[1].Click();
            //    continue;
            //}


            //---- GRABO LOS TODAS LAS REFACCIONES AppYearMakeModel_Details DEL AUTOMOVIL
            //--- actualizo el campo encontrado a verdadero
            //db.AppYearMakeModel.Where(x => x.Id == registro.Id)
            //  .ExecuteUpdate(actualiza => actualiza.SetProperty(r => r.Scratch, true));

            //db.AppYearMakeModelDetails.AddRange(aYMMD);

            //db.SaveChanges();


            // // Cargo los Resultados y busco Products Category
            // espera.Until(ExpectedConditions.ElementIsVisible(
            //     By.XPath("//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[2]/div[1]/div/div/div[2]//button")));

            // var categoriasProd = driver.FindElements(By.XPath(
            //     "//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[2]/div[1]/div/div/div[2]//button"));

            // Console.WriteLine("Categorias Del Vehiculo => {0}", categoriasProd.Count());

            // Int16 indice = 0;

            // foreach (var categoria in categoriasProd)
            // {
            //     indice++;

            //     if (categoria.Text.Trim() == "Product Category")
            //     {
            //         break;
            //     }
            // }

            // string categoriaEngine = $"//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[2]/div[1]/div/div/div[2]/div[{indice}]/button";

            //// Thread.Sleep(1000);

            // Console.WriteLine(categoriaEngine);
            // driver.FindElement(By.XPath(categoriaEngine)).Click();

            //// Thread.Sleep(1000);
            // var subCategoriasEngine = driver.FindElements(By.XPath($"//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[2]/div[1]/div/div/div[2]/div[{indice}]/div/div//label"));

            // // Hacemos el recorrido en busca del Engine
            // Int16 indiceSubCategoria = 1;

            // foreach (var subcategoria in subCategoriasEngine)
            // {
            //     indiceSubCategoria++;

            //     if (subcategoria.Text.Trim() == "Engine" || subcategoria.Text.Trim() == "Motor")
            //     {
            //         break;
            //     }
            // }

            // string subcategoriaXpath = $"//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[2]/div[1]/div/div/div[2]/div[{indice}]/div/div/div[{indiceSubCategoria}]/div";

            // Console.WriteLine("SubCategory => {0}", subcategoriaXpath);

            // driver.FindElement(By.XPath(subcategoriaXpath)).Click();

            // Thread.Sleep(4000);
        }
    }
}
