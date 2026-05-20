# iREDO User Service
# User Microservice

The User Microservice handles authentication, authorization, user profile management, addresses, roles, and saved lists for the REDO Furniture platform.

It is built with:

- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- Docker
- Google OAuth 2.0
- JWT Authentication



---

# Features

## Authentication

- Google OAuth 2.0 login
- JWT token generation
- Protected API endpoints using `[Authorize]`
- Cookie authentication support for Google OAuth

## User Management

- Person entity
- PersonDetails entity
- User roles
- Soft delete support through the Tombstone Pattern using `PersonRemoved`

## Address Management

- Add address endpoint

## Saved Lists

- Create saved lists
- Store saved posts

---

# Project Structure

```text
services/user
│
├── Controllers/
├── Data/
├── DomainModels/
├── DTOs/
├── Migrations/
├── Services/
├── .env
├── Program.cs
├── Dockerfile
├── docker-compose.yml
└── appsettings.json
```

---

# Database Structure

## Tables

### Persons
Stores the root user entity.

| Column | Type |
|---|---|
| Id | int |
| Guid | uniqueidentifier |
| RoleId | int |

---

### Roles
Stores application roles.

| Column | Type |
|---|---|
| Id | int |
| Name | nvarchar |

Seeded roles:

- user
- admin

---

### PersonDetails
Stores user profile information.

| Column | Type |
|---|---|
| Id | int |
| PersonId | int |
| Email | nvarchar |
| Firstname | nvarchar |
| Middlename | nvarchar |
| Lastname | nvarchar |
| PhoneNumber | nvarchar |
| ModifiedDate | datetime2 |

---

### Addresses
Stores addresses connected to a person.

| Column | Type |
|---|---|
| Id | int |
| PersonId | int |
| Street | nvarchar |
| StreetNumber | nvarchar |
| FloorDoor | nvarchar |
| ZipCode | nvarchar |
| City | nvarchar |
| Country | nvarchar |
| ModifiedDate | datetime2 |

---

### SavedLists
Stores user saved lists.

| Column | Type |
|---|---|
| Id | int |
| PersonId | int |
| Name | nvarchar |
| ModifiedDate | datetime2 |

---

### SavedListPosts
Stores posts inside saved lists.

| Column | Type |
|---|---|
| Id | int |
| SavedListId | int |
| SalesPostId | uniqueidentifier |
| ModifiedDate | datetime2 |

---

### PersonRemoved
Stores deleted user history.

| Column | Type |
|---|---|
| Id | int |
| PersonId | int |
| RemovedDate | datetime2 |

---

# Entity Relationships

```text
Role (1) ──────── (N) Person
Person (1) ────── (1) PersonDetails
Person (1) ────── (N) Addresses
Person (1) ────── (N) SavedLists
SavedList (1) ─── (N) SavedListPosts
Person (1) ────── (N) PersonRemoved
```

---
# Address Endpoints

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

# Environment Variables

Create a `.env` file inside `services/user`.

```env
# DATABASE
USER_DB_HOST=mssql
USER_DB_PORT=1433
USER_DB_NAME=UserDb
USER_DB_USER=sa
USER_DB_PASSWORD=YourPassword

# API
USER_API_PORT=5003
USER_API_Jwt_key=YourJwtSecret

# GOOGLE OAUTH
USER_GOOGLE_CLIENT_ID=YourGoogleClientId
USER_GOOGLE_CLIENT_SECRET=YourGoogleClientSecret

# DOCKER
CONTAINER_NAME=mssql_REDO_user
```

---

# Running the Service

## Run with Docker

```bash
docker-compose down -v
docker-compose up --build
```

---
## Google OAuth Setup

<details>
<summary>Google OAuth Setup</summary>

## 1. Open Google Cloud Console

https://console.cloud.google.com/

---

## 2. Create a Project

Create a new Google Cloud project for the user service.

Example:

REDO User Service

---

## 3. Enable Google APIs

Go to:

APIs & Services → Library

Enable:

- Google People API

---

## 4. Configure OAuth Consent Screen

Go to:

APIs & Services → OAuth consent screen

Select:

- External

Fill in:
- App name
- Support email
- Developer email

---

## 5. Add Test Users

Under:

Audience → Test users

Add the Google accounts allowed to log in during development.

---

## 6. Create OAuth Credentials

Go to:

APIs & Services → Credentials

Create:

Create Credentials → OAuth Client ID

Application type:

- Web application

---

## 7. Add Authorized Redirect URI

```text
http://localhost:5003/signin-google
```

---

## 8. Copy Credentials

Add to `.env`:

```env
USER_GOOGLE_CLIENT_ID=your_client_id
USER_GOOGLE_CLIENT_SECRET=your_client_secret
```

---

## 9. Restart Docker

```bash
docker-compose down -v
docker-compose up --build
```

---

## 10. Test Login

```text
http://localhost:5003/api/auth/google-login
```

</details>

___
# Entity Framework Commands

## Create Migration

```bash
dotnet ef migrations add InitialCreate
```

If migrations fail or tables are not created correctly, delete:

```text
Migrations/
bin/
obj/

## Apply Migration

```bash
dotnet ef database update
```

## Remove Migration

```bash
dotnet ef migrations remove
```

## Reset Database

```bash
docker-compose down -v
```

This removes SQL Server volumes and resets the database completely. Useful when EF Core starts behaving like it learned database theory through interpretive dance.

---

# Authentication Flow

## Google Login

```http
GET /api/auth/google-login
```

User is redirected to Google OAuth.

---

## Google Response

```http
GET /api/auth/google-response
```

Flow:

1. Google authenticates user
2. Backend retrieves email/profile
3. Person is created if user does not exist
4. Default role is assigned
5. JWT token is generated
6. Token returned to client

---

# Protected Endpoints

Protected endpoints require:

```http
Authorization: Bearer YOUR_TOKEN
```

Example:

```http
POST /api/auth/add-address
```

Headers:

```http
Authorization: Bearer eyJhbGciOi...
```
Example using curl:

```bash
curl -X 'POST' \
  'http://localhost:5003/api/auth/add-address' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhbC5saWxsZXNrb3ZAZ21haWwuY29tIiwiZXhwIjoxNzc5MzA0MDM3fQ.nBtBZ_gWbCtykqbIvuICPXaoSrr7M7i9MN8xWbvh8Ec' \
  -H 'Content-Type: application/json' \
  -d '{
  "street": "Roarsvej",
  "streetNumber": "7a",
  "floorDoor": "1 th",
  "zipCode": "4700",
  "city": "Næstved",
  "country": "Denmark"
}'
```
---

# Swagger

Swagger is available in Development mode:

```text
http://localhost:5003/swagger
```

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

# Common Issues

## Only `__EFMigrationsHistory` Table Gets Created

Cause:

- Broken migrations
- Missing snapshot
- Docker using stale build
- Empty migration

Fix:

```bash
docker-compose down -v
```

```bash
dotnet clean
```

Delete:

```text
Migrations/
bin/
obj/
```

Then recreate migration:
```bash
dotnet build
```
```bash
dotnet ef migrations add InitialCreate
```

```bash
docker-compose up --build
```
---

## Swagger Returns 404

Ensure application runs in Development mode:

```env
ASPNETCORE_ENVIRONMENT=Development
```

---

## Unauthorized API Requests

Protected endpoints require JWT Bearer tokens.

Add header:

```http
Authorization: Bearer YOUR_TOKEN
```

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

// =========================
// Rebuild database
// =========================
``` bash
dotnet ef database drop
dotnet ef migrations remove
dotnet build
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet list package
```

// =========================
// Rebuild docker
// =========================
````

docker-compose down
dotnet clean
>> dotnet build
dotnet ef migrations add InitialCreate
docker-compose up --build
````