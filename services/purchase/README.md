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
```

---

# Database Structure

## Tables