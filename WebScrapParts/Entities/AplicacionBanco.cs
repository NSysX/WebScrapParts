using Microsoft.EntityFrameworkCore;

namespace WebScrapParts.Entities
{
    public class AplicacionBanco
    {
        public AplicacionBanco(int id, int idProdMaestroNumber,string prodMaestroNumber ,string oemNumber, string category, string imagesDirectory, string link ,string makeName, string modelName, string yearRange, string yearInitial, string yearEnd, string description, string position, string driveWheel, string vehicleQty, string engineBase, string engineVIN, string engineDesign, string notas)
        {
            Id = id;
            IdProdMaestroNumber= idProdMaestroNumber;
            ProdMaestroNumber = prodMaestroNumber;
            OemNumber = oemNumber;
            Category = category;
            ImagesDirectory = imagesDirectory;
            Link = link;
            MakeName = makeName;
            ModelName = modelName;
            YearRange = yearRange;
            YearInitial = yearInitial.Contains("-") ? yearInitial.Substring(0, yearInitial.IndexOf('-')) : yearInitial;
            YearEnd = yearEnd.Contains("-") ? yearEnd.Substring(yearEnd.IndexOf('-') + 1) : yearEnd;
            Description = description;
            Position = position;
            DriveWheel = driveWheel;
            VehicleQty = vehicleQty;
            EngineBase = engineBase;
            EngineVIN = engineVIN;
            EngineDesign = engineDesign;
            Notas = notas;
        }

        public int Id { get; set; }
        public int IdProdMaestroNumber { get; set; }
        public string ProdMaestroNumber { get; set; } = string.Empty;
        public string OemNumber { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ImagesDirectory { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string MakeName { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string YearRange { get; set; } = string.Empty;
        public string YearInitial { get; set; } = string.Empty;
        public string YearEnd { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string DriveWheel { get; set; } = string.Empty;
        public string VehicleQty { get; set; } = string.Empty;
        public string EngineBase { get; set; } = string.Empty;
        public string EngineVIN { get; set; } = string.Empty;
        public string EngineDesign { get; set; } = string.Empty;
        public string Notas { get; set; } = string.Empty;
    }

}
