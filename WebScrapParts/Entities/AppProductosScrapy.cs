namespace WebScrapParts.Entities
{
    public class AppProductosScrapy
    {
        public int Id { get; set; }
        public int IdProdMaestro { get; set; }
        public string CodigoProdMaestro { get; set; } = string.Empty;
        public bool Encontrado { get; set; }
        public string EquivalenciaLlave { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Link1 { get; set; } = string.Empty;
        public string Link2 { get; set; } = string.Empty;
        public bool Completado { get; set; }
        public string SitioWebNombre { get; set; } = string.Empty;
        public string DirFotos { get; set; } = string.Empty;
    }
}