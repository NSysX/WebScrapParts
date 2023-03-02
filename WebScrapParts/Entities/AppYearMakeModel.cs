namespace WebScrapParts.Entities
{
    public class AppYearMakeModel
    {
        public int Id { get; set; }
        public Int16 YearId { get; set; }
        public string MakeName { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public bool Found { get; set; }
        public bool Scratch { get; set; }
    }
}
