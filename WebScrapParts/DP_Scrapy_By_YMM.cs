using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using WebScrapParts.Entities;
using WebScrapParts.Infraestructure.Persistence.Contexts;

namespace WebScrapParts
{
    public class DP_Scrapy_By_YMM
    {
        public async Task DPScrapByYmm(List<AppYearMakeModel> yearMakeModels)
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(15);
            var db = new WebScrapPartsDbContext();
            string linkReal = $@"https://www.drivparts.com/";

            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            WebDriverWait espera = new WebDriverWait(driver, timeSpan);
            string listaDatosXpath = "";
            bool encontrado = false;
            List<AppYearMakeModelDetails> aYMMD = new();
            AppYearMakeModelDetails appYearMakeModelDetails = new();
            string refaccionesXpath = "";
            string detallesRefaccion = "";

            // Recorro la lista de los automotores
            foreach (var registro in yearMakeModels)
            {
                driver.Navigate().GoToUrl(linkReal);

                // Llena el combobox de anios
                Thread.Sleep(1000);
                espera.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"years\"]/div/div[1]/input")));
                driver.FindElement(By.XPath("//*[@id=\"years\"]/div/div[1]/input")).SendKeys(registro.YearId.ToString().Trim());
                driver.FindElement(By.XPath("//*[@id=\"years\"]/div/div[1]/input")).SendKeys(Keys.Enter);
                // Llena el ComboBox de Armadoras
                //    Thread.Sleep(1000);
                espera.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"makes\"]/div/div[1]/input")));
                driver.FindElement(By.XPath("//*[@id=\"makes\"]/div/div[1]/input")).Click();

                // Busca que Exista la Armadora 
                listaDatosXpath = "//*[@id=\"makes\"]/ul/li//a";
                var armadoras = driver.FindElements(By.XPath(listaDatosXpath));
                encontrado = false;

                foreach (var armadora in armadoras)
                {
                    if (armadora.GetAttribute("innerText").Trim().ToUpper() == registro.MakeName.Trim().ToUpper())
                    {
                        encontrado = true;
                        break;
                    }
                }

                if (!encontrado)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("============> ARAMDORA NO ENCONTRADA {0}", registro.MakeName);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    continue;
                }

                driver.FindElement(By.XPath("//*[@id=\"makes\"]/div/div[1]/input")).SendKeys(registro.MakeName.Trim());
                driver.FindElement(By.XPath("//*[@id=\"makes\"]/div/div[1]/input")).SendKeys(Keys.Enter);

                // Llena el ComboBox de Modelos
                //    Thread.Sleep(2000);
                espera.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"models\"]/div/div[1]/input")));
                driver.FindElement(By.XPath("//*[@id=\"models\"]/div/div[1]/input")).Click();
                listaDatosXpath = "//*[@id=\"models\"]/ul/li//a";
                var modelos = driver.FindElements(By.XPath(listaDatosXpath));
                encontrado = false;

                foreach (var modelo in modelos)
                {
                    if (modelo.GetAttribute("innerText").Trim().ToUpper() == registro.ModelName.Trim().ToUpper())
                    {
                        encontrado = true;
                        break;
                    }
                }

                if (encontrado)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Encontrado => {0},{1},{2}", registro.YearId, registro.MakeName, registro.ModelName);
                    //--- actualizo el campo encontrado a verdadero
                    await db.AppYearMakeModel.Where(x => x.Id == registro.Id)
                         .ExecuteUpdateAsync(actualiza => actualiza.SetProperty(r => r.Found, true));
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("============ MODELO NO ENCONTRADO => {0},{1},{2}", registro.YearId, registro.MakeName, registro.ModelName);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    continue;
                }

                driver.FindElement(By.XPath("//*[@id=\"models\"]/div/div[1]/input")).SendKeys(registro.ModelName.Trim());
                driver.FindElement(By.XPath("//*[@id=\"models\"]/div/div[1]/input")).SendKeys(Keys.Enter);
                driver.FindElement(By.XPath("//*[@id=\"models\"]/div/div[1]/input")).SendKeys(Keys.Tab);


                Thread.Sleep(3000);
                // Da click en el boton buscar
                //*[@id=\"page-content\"]/div/div[1]/div/div/div/div[2]/div/div/div/div[2]/div/div[2]/button
                // driver.FindElement(By.XPath("//*[@id=\"page-content\"]/div/div[1]/div/div/div/div[2]/div/div/div/div[2]/div/div[2]/button")).Click();
                driver.FindElement(By.XPath("//*[@id=\"page-content\"]/div/div[1]/div/div/div/div[2]/div/div/div/div[2]/div/div[2]/button")).SendKeys(Keys.Enter);


                Thread.Sleep(3000);

                // ------- AQUI HAY QUE HACER EL CICLO DE NUEVO PARA CADA PAGINA DE RESULTADOS

                Console.WriteLine();

                while (true)
                {
                    Console.WriteLine("Entro al While wwwwwwwww");
                    // Thread.Sleep(3000);
                    //await Task.Delay(3000);

                    //var sinResultado = driver.FindElements(By.ClassName("no-results"));

                    //if( sinResultado.Count() > 0)
                    //{

                    //}

                    // Verifico que hay Resultados todas las listas
                    espera.Until(ExpectedConditions.ElementIsVisible(
                                             By.XPath("//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[2]/ul")));

                    refaccionesXpath = "//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[2]/ul//li";
                    var refacciones = driver.FindElements(By.XPath(refaccionesXpath));

                    // si no encuentra resultados de Refacciones
                    if (refacciones.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("NO HAY RESULTADOS DE REFACCIONES xxxxxxxxxxxxx");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        continue;
                    }

                    Console.WriteLine("Refacciones Count => {0}", refacciones.Count);

                    foreach (var refaccion in refacciones)
                    {
                        // si refaccion esta vacia 
                        if(string.IsNullOrEmpty(refaccion.Text.Trim()))
                        {
                            Console.WriteLine("|========== Esta vacio la refaccion ==========|");
                            continue;
                        }

                        // reemplazo \r\n en ,
                        detallesRefaccion = refaccion.Text.Replace("\r\n", ",");

                        // mando crear el objeto 
                        appYearMakeModelDetails = new AppYearMakeModelDetails(registro.Id, detallesRefaccion);

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"aplicacion => {refaccion.Text}");
                        Console.ForegroundColor = ConsoleColor.Gray;

                        // Agrego los objetos de cada refaccion a la lista
                        aYMMD.Add(appYearMakeModelDetails);

                        Console.WriteLine("-----------------------------------------------------------");
                    }

                    // grabo todos los detalles de la refacciones encontradas
                    // Thread.Sleep(3000);
                    string divPaginacion = "//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]";
                    // si existe el div de paginacion
                    // var existeDivPaginacion = espera.(ExpectedConditions.ElementExists(By.ClassName(divPaginacion)));
                    //*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]/button[2]

                    // espera.Until(ExpectedConditions.ElementIsVisible(By.XPath(divPaginacion)));

                    var paginacion = driver.FindElements(By.XPath(divPaginacion));

                    //-- Verifico que hay Resultados todas las listas
                    //espera.Until(ExpectedConditions.ElementExists(
                    //             By.XPath("//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]//button")));
                    //espera.Until(ExpectedConditions.ElementIsVisible(
                    //             By.XPath("//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]//button")));
                    //espera.Until(ExpectedConditions.ElementToBeClickable(
                    //             By.XPath("//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]//button")));

                    //var paginacionBotones = driver.FindElements(
                    //             By.XPath("//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]//button"));

                    if (paginacion.Count == 0)
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("NO EXISTE EL BOTON DE PAGINACION @@@@@@@@@@@");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine();
                        break;
                    }

                    //Console.WriteLine($"Texto del Boton = {paginacionBotones[1].GetAttribute("innerHTML")}");

                    //-- segundo boton
                    //if (paginacionBotones[1].Enabled)
                    //{
                    //    paginacionBotones[1].Click();
                    //    continue;
                    //}

                    espera.Until(ExpectedConditions.ElementToBeClickable(
                                 By.XPath("//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]/input")));
                    var numeroActualPagina = driver.FindElement(
                                 By.XPath("//*[@id=\"page-content\"]/div/div/div[1]/div/div[2]/div/div/div/div[3]/div/div[1]/div[3]/input"));

                    int totalPaginas = Int32.Parse(paginacion[0].Text.Replace("PAGE OF ", "").Trim());
                    int paginaActual = Int32.Parse(numeroActualPagina.GetAttribute("value").Trim());

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Total Paginas Actualizado = {totalPaginas}");
                    Console.WriteLine($"Termina PaginaActual = {paginaActual}");
                    Console.WriteLine();


                    if (paginaActual < totalPaginas)
                    {
                        paginaActual++;
                        numeroActualPagina.SendKeys(Keys.Backspace);
                        numeroActualPagina.SendKeys(Keys.Backspace);
                        numeroActualPagina.SendKeys(paginaActual.ToString().Trim());
                        numeroActualPagina.SendKeys(Keys.Enter);
                        continue;
                    }

                    break;
                }

                //---- GRABO LOS TODAS LAS REFACCIONES AppYearMakeModel_Details DEL AUTOMOVIL
                //---- Actualizo el Campo Scratch a true
                await db.AppYearMakeModel.Where(x => x.Id == registro.Id)
                  .ExecuteUpdateAsync(actualiza => actualiza.SetProperty(r => r.Scratch, true));

                await db.AppYearMakeModelDetails.AddRangeAsync(aYMMD);
                await db.SaveChangesAsync();

                aYMMD.Clear();

                Console.WriteLine();
                Console.WriteLine("Fin del Registro ************************");

            }

            Console.WriteLine();
            Console.WriteLine("Fin de la Aplicacion ************************");
        }
    }
}
