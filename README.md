# CRN Product API

A scalable and maintainable RESTful Product Management API developed using **ASP.NET Core Web API with .NET 8 and C#**.

This project was developed as part of the **CRN Technosoft Technical Assessment** and follows industry-standard practices including layered architecture, Repository Pattern, Service Layer, JWT authentication, refresh tokens, role-based authorization, validation, centralized exception handling, API versioning, pagination, Swagger/OpenAPI documentation, automated testing, and Docker support.

## 📌 Project Overview

The objective of this project is to design and implement a RESTful API around Products to perform complete CRUD operations.

### Key Features

- Product CRUD operations
- RESTful API design
- .NET 8 Web API
- Entity Framework Core
- SQL Server
- Repository Pattern
- Service Layer
- DTOs
- FluentValidation
- JWT Authentication
- Refresh Token
- BCrypt Password Hashing
- Role-Based Authorization
- Centralized Exception Handling Middleware
- API Versioning
- Pagination
- CORS
- Response Compression
- Security Headers
- Swagger / OpenAPI
- Unit Testing with xUnit and Moq
- Integration Testing with WebApplicationFactory
- Docker and Docker Compose support

---

## 🛠️ Technology Stack

| Technology | Purpose |
|---|---|
| C# | Programming Language |
| .NET 8 | Application Framework |
| ASP.NET Core Web API | RESTful API |
| Entity Framework Core | ORM / Data Access |
| SQL Server | Database |
| JWT | Authentication |
| BCrypt | Password Hashing |
| FluentValidation | Request Validation |
| Swagger / OpenAPI | API Documentation |
| xUnit | Unit Testing |
| Moq | Mocking |
| WebApplicationFactory | Integration Testing |
| Docker | Containerization |
| Docker Compose | Multi-container setup |
| Visual Studio 2022 | Development IDE |

---

## 🏗️ High-Level Architecture

```text
                         CLIENT
                           |
                           v
                 +-------------------+
                 |   API Controller  |
                 +-------------------+
                           |
                           v
                 +-------------------+
                 |   Service Layer   |
                 +-------------------+
                           |
                           v
                 +-------------------+
                 | Repository Layer  |
                 +-------------------+
                           |
                           v
                 +-------------------+
                 | Entity Framework  |
                 |       Core        |
                 +-------------------+
                           |
                           v
                 +-------------------+
                 |    SQL Server     |
                 +-------------------+
```

---

## 📁 Project Structure

```text
CRN.Product.API
│
├── Controllers
│   ├── AuthController.cs
│   └── ProductsController.cs
│
├── Data
│   ├── ApplicationDbContext.cs
│   └── DbInitializer.cs
│
├── DTOs
│   ├── LoginRequestDto.cs
│   ├── LoginResponseDto.cs
│   ├── ProductDto.cs
│   ├── RefreshTokenRequestDto.cs
│   └── PagedResult.cs
│
├── Entities
│   ├── Product.cs
│   └── User.cs
│
├── Middleware
│   └── ExceptionHandlingMiddleware.cs
│
├── Repositories
│   ├── IProductRepository.cs
│   └── ProductRepository.cs
│
├── Security
│   └── SecurityHeadersMiddleware.cs
│
├── Services
│   ├── IProductService.cs
│   └── ProductService.cs
│
├── Validators
│   └── ProductValidator.cs
│
├── Migrations
│
├── Program.cs
├── appsettings.json
├── Dockerfile
├── docker-compose.yml
├── .dockerignore
├── .gitignore
└── README.md

CRN.Product.API.Tests
│
├── ProductsApiTests.cs
├── ProductServiceTests.cs
└── CRN.Product.API.Tests.csproj
```

---

## 🧱 Architecture Layers

### Controller Layer

Handles:

- HTTP requests and responses
- Routing
- API versioning
- Authentication
- Authorization

### Service Layer

Contains business logic and coordinates application operations between controllers and repositories.

### Repository Layer

Handles database access and CRUD operations using Entity Framework Core.

Read-only queries use `AsNoTracking()` where appropriate.

### Data Layer

Contains:

- `ApplicationDbContext`
- Entity Framework Core configuration
- Migrations
- Database initialization

---

## 🗄️ Database Design

The application uses **Microsoft SQL Server** with Entity Framework Core.

### Product Table

```sql
CREATE TABLE [dbo].[Product]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [ProductName] NVARCHAR(255) NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreatedOn] DATETIME NOT NULL,
    [ModifiedBy] NVARCHAR(100) NULL,
    [ModifiedOn] DATETIME NULL
);
```

### Item Table

```sql
CREATE TABLE [dbo].[Item]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [ProductId] INT NOT NULL,
    [Quantity] INT NOT NULL,
    FOREIGN KEY ([ProductId]) REFERENCES [Product]([Id])
);
```

---

## 📦 Product API

The API uses versioned RESTful endpoints.

### Base Route

```text
/api/v1/Products
```

### Get All Products

```http
GET /api/v1/Products
```

Pagination example:

```http
GET /api/v1/Products?pageNumber=1&pageSize=10
```

### Get Product By ID

```http
GET /api/v1/Products/{id}
```

Example:

```http
GET /api/v1/Products/1
```

### Create Product

```http
POST /api/v1/Products
```

Example request:

```json
{
  "productName": "Laptop",
  "createdBy": "Admin"
}
```

### Update Product

```http
PUT /api/v1/Products/{id}
```

### Delete Product

```http
DELETE /api/v1/Products/{id}
```

---

## 🔐 Authentication

The API uses **JWT Bearer Authentication**.

Authentication flow:

```text
Login Request
     |
     v
Validate User
     |
     v
Verify BCrypt Password
     |
     v
Generate Access Token
     |
     v
Generate Refresh Token
```

Access tokens are short-lived and refresh tokens are used to obtain new access tokens.

---

## 🔄 Refresh Token Flow

```text
Access Token Expired
        |
        v
Send Refresh Token
        |
        v
Validate Refresh Token
        |
        v
Generate New Access Token
        |
        v
Rotate Refresh Token
```

---

## 👥 Role-Based Authorization

Example roles:

```text
Admin
User
```

Protected endpoints can use:

```csharp
[Authorize]
```

Role-specific endpoints can use:

```csharp
[Authorize(Roles = "Admin")]
```

---

## 🔒 Password Security

Passwords are not stored as plain text.

**BCrypt** is used for password hashing and verification.

---

## ⚠️ Exception Handling

A centralized `ExceptionHandlingMiddleware` provides consistent API error responses and prevents internal implementation details from being exposed.

---

## ✅ Data Validation

**FluentValidation** is used to validate incoming product requests before business logic is executed.

---

## 📄 DTOs

DTOs are used to separate API contracts from database entities.

Examples:

```text
ProductDto
LoginRequestDto
LoginResponseDto
RefreshTokenRequestDto
PagedResult
```

---

## 📑 Pagination

Collection endpoints support pagination.

Example:

```http
GET /api/v1/Products?pageNumber=1&pageSize=10
```

Pagination helps reduce response size, database load, and network usage.

---

## 🔢 API Versioning

The API currently uses version `v1`.

Example:

```text
/api/v1/Products
```

API versioning allows future versions to be introduced without breaking existing clients.

---

## 🌐 CORS

CORS is configured to support cross-origin requests during development.

For production, allowed origins should be restricted to trusted frontend applications.

---

## 🛡️ Security Headers

Security response headers include:

```text
X-Content-Type-Options
X-Frame-Options
Referrer-Policy
```

HTTPS redirection is also enabled.

---

## ⚡ Performance Considerations

The application includes:

- Async/await for database operations
- `AsNoTracking()` for read-only queries
- Pagination for collection endpoints
- Response compression
- Repository abstraction
- Database indexing considerations

---

## 📖 Swagger / OpenAPI

Swagger provides interactive API documentation and testing.

After starting the application:

```text
https://localhost:<port>/swagger
```

Swagger provides:

- Endpoint documentation
- Request/response models
- Authentication support
- Interactive API testing

---

## 🧪 Testing Strategy

The solution contains unit and integration tests.

### Unit Testing

Technologies:

- xUnit
- Moq

Unit tests cover:

- Create Product
- Get Product
- Product Not Found
- Update Product
- Update Product Not Found
- Delete Product
- Delete Product Not Found

### Integration Testing

Integration testing uses:

- `Microsoft.AspNetCore.Mvc.Testing`
- `WebApplicationFactory`

The integration test verifies that the application and Swagger endpoint start successfully.

### Test Result

```text
8 Tests
8 Passed
0 Failed
0 Skipped
```

---

## 🐳 Docker

Docker support is included using:

```text
Dockerfile
docker-compose.yml
.dockerignore
```

Docker Compose is designed to run:

```text
ASP.NET Core API
       |
       v
SQL Server
```

### Build and Run

```bash
docker compose up --build
```

The exact API/Swagger port depends on the `docker-compose.yml` configuration.

---

## ⚙️ Local Development Setup

### Prerequisites

- Visual Studio 2022
- .NET 8 SDK
- SQL Server / SQL Server LocalDB
- SQL Server Management Studio or SQL Server Object Explorer
- Git

### Clone Repository

```bash
git clone https://github.com/Anirudha15/CRN.Product.API.git
cd CRN.Product.API
```

### Configure Database

For local development, configure the connection string in:

```text
appsettings.json
```

Example LocalDB connection:

```text
Server=(localdb)\MSSQLLocalDB;Database=CRNProductDB;Trusted_Connection=True;TrustServerCertificate=True
```

### Apply Database Migration

Using Visual Studio Package Manager Console:

```powershell
Update-Database
```

### Run Application

From Visual Studio:

```text
F5
```

or:

```bash
dotnet run
```

Then open:

```text
https://localhost:<port>/swagger
```

---

## 🧪 Run Tests

In Visual Studio:

```text
Test
  ↓
Test Explorer
  ↓
Run All Tests
```

Expected result:

```text
8 Passed
0 Failed
0 Skipped
```

---

## 🔑 Configuration and Secrets

Production secrets should not be committed to source control.

Sensitive values such as:

- JWT signing keys
- Database passwords
- API secrets
- Production connection strings

should be supplied through environment variables, User Secrets, or a secure secret-management solution.

---

## 🚀 Deployment

High-level deployment flow:

```text
Developer
    |
    v
GitHub Repository
    |
    v
Build Application
    |
    v
Create Docker Image
    |
    v
Container Registry
    |
    v
Deploy API Container
    |
    v
SQL Server
```

---

## 📊 HTTP Status Codes

| Status Code | Meaning |
|---|---|
| 200 | OK |
| 201 | Created |
| 204 | No Content |
| 400 | Bad Request |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |
| 500 | Internal Server Error |

---

## 🔐 Security Measures

The project implements:

- JWT authentication
- Short-lived access tokens
- Refresh tokens
- BCrypt password hashing
- Role-based authorization
- Input validation
- CORS
- HTTPS
- Security response headers
- Centralized exception handling
- DTO-based API contracts
- Protection of sensitive configuration

---

## 📈 Scalability and Maintainability

The layered architecture provides:

- Separation of concerns
- Easier unit testing
- Reduced coupling
- Better maintainability
- Easier future feature development
- Independent evolution of application layers

Architecture:

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
Entity Framework Core
    ↓
SQL Server
```

---

## 📋 Assessment Requirements Covered

```text
.NET 8 with C#                    ✅
ASP.NET Core Web API              ✅
SQL Server                        ✅
Entity Framework Core             ✅
Product CRUD                      ✅
RESTful API Design                ✅
Repository Pattern                ✅
Service Layer                     ✅
DTOs                              ✅
FluentValidation                  ✅
JWT Authentication                ✅
Refresh Token Strategy             ✅
Role-Based Authorization          ✅
Centralized Error Handling        ✅
API Versioning                    ✅
Pagination                        ✅
AsNoTracking                      ✅
Async/Await                       ✅
CORS                              ✅
HTTPS                             ✅
Security Headers                  ✅
Response Compression              ✅
Swagger/OpenAPI                   ✅
xUnit                             ✅
Moq                               ✅
WebApplicationFactory             ✅
Dockerfile                        ✅
Docker Compose                    ✅
Testing Strategy                  ✅
Environment Configuration         ✅
High-Level Deployment             ✅
```

---

## 📂 GitHub Repository

**Repository:**  
https://github.com/Anirudha15/CRN.Product.API

---

## 👨‍💻 Author

Developed by **Anirudha Shinde** as part of the **CRN Technosoft Technical Assessment**.

---
