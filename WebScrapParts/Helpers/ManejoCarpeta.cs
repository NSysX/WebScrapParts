namespace WebScrapParts.Helpers
{
    public class ManejoCarpeta
    {
        public string PathCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Crea las Carpetas para almacenar las Aplicaciones y Fotos
        /// </summary>
        /// <param name="categoria">Es utilizado para el nombre de la carpeta</param>
        /// <param name="catalogoOnline">Es utilizado para crear dentro de la categoria el nombre del Catalogo online</param>
        /// <param name="codigoBase">Es utilizado para crear la carpeta con el codigo del producto</param>
        /// <returns></returns>
        public bool Creacion( string categoria ,  string catalogoOnline, string codigoBase, string unidad = "C:" ) 
        {
            string carpetaBase = @$"{unidad.Trim()}\AplicacionesRefacciones";
            string pathCompleto = @$"{carpetaBase.Trim()}\{catalogoOnline.Trim()}\{categoria.Trim()}\{codigoBase.Trim()}\";

            try
            {
                var di = new DirectoryInfo(pathCompleto);

                if (di.Exists == false)
                {
                    di.Create();
                    Console.WriteLine($"Se ha creado el directorio {pathCompleto}");
                }
            }
            catch (Exception)
            {
                return false;
            }

            this.PathCompleto = pathCompleto;

            return true;
        }

    }
}
