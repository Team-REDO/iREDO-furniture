using DTO;
using Stripe;
using Stripe.Checkout;

public class StripeService
{
    public StripeService()
    {
        DotNetEnv.Env.Load();

        var key = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");

        if (string.IsNullOrEmpty(key))
            throw new Exception("STRIPE_SECRET_KEY not set");

        StripeConfiguration.ApiKey = key;
    }

    public string CreateCheckoutSession(CreateCheckoutRequest order)
    {
        var lineItems = order.OrderItems.Select(item =>
            new SessionLineItemOptions
            {
                Quantity = item.Quantity,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "dkk",
                    UnitAmount = (long)(item.Price * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Title
                    }
                }
            }).ToList();

        var options = new SessionCreateOptions
        {
            Mode = "payment",
            // SuccessUrl = "http://localhost:5173/payment-success",
            // CancelUrl = "http://localhost:5173/payment-cancel",
            SuccessUrl = "http://localhost:3000/payment-success",
            CancelUrl = "http://localhost:3000/payment-cancel",
            CustomerEmail = order.Email,
            LineItems = lineItems,
            Metadata = new Dictionary<string, string>
            {
                { "email", order.Email },
                { "orderGuid", order.OrderGuid },
                { "status", order.Status },
                { "itemCount", order.OrderItems.Count.ToString() },
                { "totalQuantity", order.OrderItems.Sum(x => x.Quantity).ToString() }
            }
        };

        var service = new SessionService();
        var session = service.Create(options);

        return session.Url;
    }
}