using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebScrapParts.Entities;
using WebScrapParts.Helpers;
using WebScrapParts.Infraestructure.Persistence.Contexts;
using Microsoft.Win32;

namespace WebScrapParts
{
    public class DC_ScrapySelenium
    {
        //private readonly List<AppProductosScrapy> appProductosScrapies;

        //public DC_ScrapySelenium(List<AppProductosScrapy> appProductosScrapies)
        //{
        //    this.appProductosScrapies = appProductosScrapies;
        //}

        IWebDriver driver = new ChromeDriver();

        public bool NavegoAlLink_DC(string link, out string linkReal)
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

            if (noSeEncontro.Count > 0)
            {
                return false;
            }

            return true;
        }

        public async Task ScrapDcParts(List<AppProductosScrapy> appProductosScrapies)
        {
            TimeSpan timer = TimeSpan.FromSeconds(1);
            var db = new WebScrapPartsDbContext();
            List<AplicacionBanco> aplicacionBancos = new();
            var specs = new Specs();
            var manejoCarpeta = new ManejoCarpeta();
            var bajar = new WebClient();
            string linkReal;

            foreach (var codigo in appProductosScrapies)
            {
                aplicacionBancos.Clear();

                // verifico si es vacio el codigo llave brinco al siguiente registro
                if (string.IsNullOrEmpty(codigo.EquivalenciaLlave.Trim()))
                {
                    continue;
                }

                codigo.CodigoProdMaestro = codigo.CodigoProdMaestro.Replace('/', '_');

                // abre el navegador y navega hasta el link1
                bool encontroApplicaciones = NavegoAlLink_DC(codigo.Link1, out linkReal);

                driver.FindElement(By.XPath("//*[@id=\"txtBusquedaRapida\"]")).SendKeys(codigo.EquivalenciaLlave.Trim());

                await Task.Delay(1000);

                IList<IWebElement> registrosCoincidencias = driver.FindElements(By.XPath("//*[@id=\"ui-id-1\"]//div"));

                registrosCoincidencias[0].Click();

                //string texto;

                //foreach (var registro in registrosCoincidencias)
                //{
                //    texto = registro.GetAttribute("innerHTML");
                //    Console.WriteLine(registro.GetAttribute("innerHTML"));
                //}
                break;
            }

        }
    }
}
