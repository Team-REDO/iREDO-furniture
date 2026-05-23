namespace Purchase.Data
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;

        public string DatabaseName { get; set; } = string.Empty;

        public string PurchaseCollection { get; set; } = string.Empty;

        public string OrderCollection { get; set; } = string.Empty;

        public string OrderItemCollection { get; set; } = string.Empty;
    }
}
