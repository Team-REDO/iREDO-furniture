# iREDO User Service
# User Microservice

# Project Structure

```text
services/Purchase
        ├── Controllers/
        │    └── PurchaseController.cs
        │
        ├── Data/
        │    ├── MongoDbSettings.cs
        │    └── Repositories/
        │
        ├── DomainModels/
        │    ├── Purchase.cs
        │    ├── Order.cs
        │    ├── OrderItem.cs
        │    ├── Category.cs
        │    ├── Image.cs
        │    └── Color.cs
        │
        ├── DTOs/
        │    ├── CreatePurchaseRequest.cs
        │    └── PurchaseResponse.cs
        │
        ├── Enums/
        │    ├── OrderStatus.cs
        │    └── ConditionType.cs
        │
        ├── Services/
        │    └── StripeService.cs
        │
        ├── .env
        ├── appsettings.json
        ├── Program.cs
        ├── Dockerfile
        ├── docker-compose.yml
        └── Purchase.csproj

services/Purchasepurchase/
        ├── Controllers/
        │   ├── PurchaseController.cs
        │   ├── TestController.cs
        │   └── WebhookController.cs
        │
        ├── Data/
        │   └── MongoDbSettings.cs
        │
        ├── DomainModels/
        │   ├── Order.cs
        │   ├── OrderItem.cs
        │   └── Purchase.cs
        │
        ├── Enums/
        │   ├── ConditionType.cs
        │   └── OrderStatus.cs
        │
        ├── Properties/
        │   └── launchSettings.json
        │
        ├── Services/
        │   └── StripeService.cs
        │
        ├── src/
        │   └── DELETEME.md
        │
        ├── .env
        ├── .gitignore
        ├── appsettings.Development.json
        ├── appsettings.json
        ├── docker-compose.yml
        ├── Dockerfile
        ├── Program.cs
        ├── Purchase.csproj
        ├── Purchase.csproj.user
        ├── Purchase.http
        ├── Purchase.slnx
        └── README.md
```
```
stripe listen --forward-to localhost:5258/webhook
stripe trigger checkout.session.completed
```

---

# Database Structure

## Tables