# iREDO User Service

The `user-service` handles authentication, user profile management, addresses, saved lists, and user-related persistence for the iREDO platform.

Built with:
- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- JWT Authentication
- Google OAuth
- Docker

Because apparently every modern application needs authentication, OAuth, JWTs, containerization, migrations, and emotional resilience just to store a furniture wishlist.

---

---

# Features

## Authentication
- Google OAuth login
- JWT token generation
- Protected API endpoints using Bearer authentication

## User Management
- Person and profile management
- Address handling
- Saved lists for posts/products
- Removed user tracking

## Database
- Entity Framework Core with SQL Server
- Automatic migrations on startup
- Relational domain modeling

## Infrastructure
- Dockerized setup
- Environment variable configuration
- Swagger/OpenAPI support
- CORS enabled

---

# Project Structure

```txt
services/user
│
├── Controllers/
├── Data/
├── DomainModels/
├── DTOs/
├── Migrations/
├── Services/
│
├── Dockerfile
├── docker-compose.yml
├── Program.cs
├── appsettings.json
└── .env
```

---

# Domain Models

## Person
Root user entity.

Contains:
- Internal database identity
- Global GUID
- Relationships to saved lists and removal history

## PersonDetails
Stores personal user information:
- Email
- First name
- Last name
- Phone number

Connected 1:1 with `Person`.

## Address
Stores user address information:
- Street
- Zip code
- City
- Country

Connected 1:1 with `PersonDetails`.

## SavedList
Represents a user-created saved collection.

Examples:
- Favorites
- Wishlist
- Saved furniture posts

## SavedListPost
Represents individual posts/items inside a saved list.

## PersonRemoved
Stores removed/deleted user records for audit tracking and lifecycle management.

---

# Authentication Flow

## Google Login

```http
GET /api/auth/google-login
```

Redirects the user to Google OAuth.

---

## Google Response

```http
GET /api/auth/google-response
```

After successful authentication:
1. User information is retrieved from Google
2. Existing user is checked by email
3. User is created if not found
4. JWT token is generated and returned

---

## Current User

```http
GET /api/auth/me
```

Protected endpoint requiring JWT authentication.

Returns:
```json
{
  "email": "user@example.com"
}
```

---

# Address API

## Add Address

```http
POST /api/auth/add-address
```

Requires JWT Bearer token.

### Request

```json
{
  "street": "Main Street",
  "streetNumber": "10",
  "floorDoor": "2A",
  "zipCode": "2100",
  "city": "Copenhagen",
  "country": "Denmark"
}
```

### Response

```json
{
  "street": "Main Street",
  "streetNumber": "10",
  "floorDoor": "2A",
  "zipCode": "2100",
  "city": "Copenhagen",
  "country": "Denmark"
}
```

---

# Environment Variables

Create a `.env` file:

```env
# DATABASE
USER_DB_HOST=mssql
USER_DB_PORT=1433
USER_DB_NAME=UserDb
USER_DB_USER=sa
USER_DB_PASSWORD=YourPassword

# USER SERVICE
USER_API_PORT=5003
USER_API_Jwt_key=YourJwtKey

# GOOGLE AUTH
USER_GOOGLE_CLIENT_ID=YourGoogleClientId
USER_GOOGLE_CLIENT_SECRET=YourGoogleClientSecret

# DOCKER
CONTAINER_NAME=mssql_REDO_user
```

---

# Running with Docker

## Start Containers

```bash
docker-compose up --build
```

Application runs on:

```txt
http://localhost:5003
```

Swagger UI:

```txt
http://localhost:5003/swagger
```

---

# Database Migrations

## Create Migration

```bash
dotnet ef migrations add MigrationName
```

## Update Database

```bash
dotnet ef database update
```

The application also attempts automatic migration execution on startup with retry logic.

Because containers starting before databases are ready is one of civilization’s longest-running distributed systems traditions.

---

# Technologies

- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- Google OAuth
- Swagger/OpenAPI
- Docker
- DotNetEnv

---

# Security Notes

- JWT keys should never be committed to source control
- `.env` files should remain private
- Google OAuth secrets must be stored securely
- Production environments should use HTTPS

---

# Future Improvements

- Refresh tokens
- Role-based authorization
- Email verification
- Password-based authentication
- Event-driven messaging
- Rate limiting
- Better logging and monitoring
- Soft delete implementation

Because no backend is ever “finished.” It merely reaches new evolutionary stages of complexity.

_________________________________________________________________________________________________________

``` bash
dotnet ef database drop

dotnet build
dotnet ef migrations add InitialCreate
dotnet ef migrations add AddPersonStuff
dotnet ef database update
dotnet list package
```

// =========================
// Person ↔ PersonRemoved (1:N)
// =========================

````

dotnet ef database drop
dotnet ef migrations remove
dotnet ef migrations add CleanStart
dotnet ef database update
````
// =========================
// Rebuild docker
// =========================
````
docker-compose down
>> docker-compose up --build
````