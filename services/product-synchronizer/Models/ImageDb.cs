namespace SynchronizerService.Models
{
    public class ImageDb
    {
        public string Id { get; set; } // Mongo _id
        public string ImageGuid { get; set; }
        public string ImageUrl { get; set; }
    }
}