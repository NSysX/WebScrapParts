namespace WebScrapParts.Helpers
{
    public static class ProductoLinkDrivCat
    {
        public static string RegresaLink(string categoria, string numeroOriginal)
        {
            numeroOriginal = numeroOriginal.Replace(" ", "%20");
            string resultado = string.Empty;

            switch (categoria)
            {
                // https://www.drivparts.com/es-mx/part-details.html?part_number=                                                               
                case "SellosValvulas":
                    resultado = @$"https://www.drivparts.com/part-details.html?part_number={numeroOriginal}&brand_code=BCWV";
                    break;
                case "Crucetas":
                    resultado = @$"https://www.drivparts.com/part-details.html?part_number={numeroOriginal}&brand_code=FLLK";
                    break;
                case "Empaque Cabeza":
                    resultado = $@"https://www.drivparts.com/es-mx/part-details.html?part_number={numeroOriginal}&brand_code=T207";
                    break;
                case "Metales Biela":
                    resultado = $@"https://www.drivparts.com/part-details.html?part_number={numeroOriginal}&brand_code=BDBR";
                    break;
                case "Metales Centro":
                    resultado = $@"https://www.drivparts.com/part-details.html?part_number={numeroOriginal}&brand_code=BDBR";
                    break;
                default:
                    resultado = "No se encontro Opcion";
                    break;
            }

            return resultado;
        }
    }
}
