using Newtonsoft.Json;

namespace WebScrapParts.Entities
{
    public class AppYearMakeModelDetails
    {

        private string criteria = "";

        public AppYearMakeModelDetails()
        {

        }

        /// <summary>
        /// Recibe un string Separado Por Comas y crea el Objeto
        /// </summary>
        /// <param name="infoRefaccion"></param>
        public AppYearMakeModelDetails(int idAppYearMakeModel, string infoRefaccion) 
        {
            var diccionarioCriteria = new Dictionary<string, List<string>>();
            criteria = "";
            string ultimaLlave = "";

            // check for interactiveDiagram is in it
            bool isInteractiveDiag = infoRefaccion.Contains("Interactive Diagrams");
            infoRefaccion = infoRefaccion.Replace("WHERE TO BUY,", "");
            infoRefaccion = infoRefaccion.Replace("BUY NOW,", "");
            infoRefaccion = infoRefaccion.Replace("Interactive Diagrams,", "");

            // es un solo objeto tiene solo una linea
            var listaDatosRefaccion = new List<string>(infoRefaccion.Split(",").ToList());
            
            this.IdAppYearMakeModel = idAppYearMakeModel;
            this.PartNumber = listaDatosRefaccion[0].Replace("Part No:", "").Trim();
            this.BrandName = listaDatosRefaccion[1].Trim();
            this.Description = listaDatosRefaccion[2].Trim();
            this.IsInteractiveDiagrams = isInteractiveDiag;

            for (int i = 3; i < listaDatosRefaccion.Count(); i++)
            {
                criteria += string.IsNullOrEmpty(criteria) ?  "" : "," ;
                criteria += listaDatosRefaccion[i].Trim();

                if (listaDatosRefaccion[i].Trim().Replace(":", "") == "QUALIFIERS" ||
                    listaDatosRefaccion[i].Trim().Replace(":", "") == "APPLICATION CRITERIA")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    ultimaLlave = listaDatosRefaccion[i].Trim().Replace(":", "");
                    // APPLICATION CRITERIA 
                    // QUALIFIERS: hay que quitar los dos puntos
                    // abro un registro
                    Console.WriteLine($"Ultima Llave  = { ultimaLlave }");

                    if (string.IsNullOrEmpty(ultimaLlave.Trim()))
                    {
                        Console.WriteLine($" #### NO HAY LLAVE PARA EL DIC => { infoRefaccion } ");
                    }

                    diccionarioCriteria.Add(ultimaLlave, new());
                    Console.ForegroundColor = ConsoleColor.Gray;
                    continue;
                }

                diccionarioCriteria[ultimaLlave].Add(listaDatosRefaccion[i].Trim());
            }

            this.AppCriteria = criteria;
            this.AppCriteriaJson = JsonConvert.SerializeObject(diccionarioCriteria);
        }

        public AppYearMakeModelDetails(int idAppYearMakeModel,
                                       string partNumber,
                                       string brandName,
                                       string Description,
                                       string appCriteria,
                                       string appCriteriaJson,
                                       bool isInteractiveDiagrams)
        {
            this.IdAppYearMakeModel = idAppYearMakeModel;
            this.PartNumber = partNumber;
            this.BrandName = brandName;
            this.Description = Description;
            this.AppCriteria = appCriteria;
            this.AppCriteriaJson = appCriteriaJson;
            this.IsInteractiveDiagrams = isInteractiveDiagrams;
        }

        public long Id { get; set; }
        public int IdAppYearMakeModel { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AppCriteria { get; set; } = string.Empty;
        public string AppCriteriaJson { get; set; } = string.Empty;
        public bool IsInteractiveDiagrams { get; set; }
    }
}
