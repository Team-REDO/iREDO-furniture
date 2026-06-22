namespace SynchronizerService.Models
{
    public class ColorDb
    {
        public string Id { get; set; } // Mongo _id
        public string ColorGuid { get; set; }
        public string Name { get; set; }
        public string Href { get; set; }
    }
}