public static class FakeInventory
{
    private static readonly Dictionary<string, int> Stock = new()
    {
        { "550e8400-e29b-41d4-a716-446655440000", 10 },
        { "product-2", 0 }
    };

    public static (bool available, string reason) Check(string productId, int quantity)
    {
        if (!Stock.ContainsKey(productId))
            return (false, "NOT_FOUND");

        if (Stock[productId] < quantity)
            return (false, "OUT_OF_STOCK");

        return (true, "OK");
    }
}

//Det her skal erstattes af rigtige implementeringer, som tjekker en database eller et eksternt system for
//at se om produktet er på lager. Det er kun for at illustrere hvordan det kunne se ud.