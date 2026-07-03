# Product API - .NET 8 Technical Assessment

A RESTful Product Management API built using **ASP.NET Core 8**, following **Clean Architecture** principles with JWT Authentication, Entity Framework Core, SQL Server, Swagger, FluentValidation, xUnit, and Docker support.

---

# Technology Stack

- ASP.NET Core 8 Web API
- C#
- SQL Server
- Entity Framework Core
- JWT Authentication
- Refresh Token Authentication
- AutoMapper
- FluentValidation
- Serilog
- Swagger / OpenAPI
- xUnit
- Moq
- Docker

---

# Project Architecture

```
ProductApi

├── ProductApi.API
│   ├── Controllers
│   ├── Middleware
│   ├── Program.cs
│   └── appsettings.json
│
├── ProductApi.Application
│   ├── DTOs
│   ├── Interfaces
│   ├── Mapping
│   ├── Services
│   └── Validators
│
├── ProductApi.Domain
│   ├── Entities
│   └── Exceptions
│
├── ProductApi.Infrastructure
│   ├── Data
│   ├── Identity
│   ├── Repositories
│   └── UnitOfWork
│
├── ProductApi.API.Tests
│
└── ProductApi.Application.Tests
```

---

# Features

- CRUD operations for Products
- Repository Pattern
- Unit of Work Pattern
- JWT Authentication
- Refresh Token Support
- Role Based Authorization
- Global Exception Handling Middleware
- FluentValidation
- Pagination
- AutoMapper
- Swagger Documentation
- Response Compression
- API Versioning
- Serilog Logging
- xUnit Unit Testing
- Docker Support

---

# Database

### Product

| Column | Type |
|---------|------|
| Id | int |
| ProductName | nvarchar(255) |
| CreatedBy | nvarchar(100) |
| CreatedOn | datetime |
| ModifiedBy | nvarchar(100) |
| ModifiedOn | datetime |

---

### Item

| Column | Type |
|---------|------|
| Id | int |
| ProductId | int |
| Quantity | int |

---

# Authentication

The API uses JWT Bearer Authentication.

Workflow

```
Login
   ↓
Access Token
   ↓
API Calls
   ↓
Refresh Token
   ↓
New Access Token
```

---

# API Endpoints

## Authentication

| Method | Endpoint |
|---------|----------|
| POST | /api/v1/Auth/login |
| POST | /api/v1/Auth/refresh |

---

## Products

| Method | Endpoint |
|---------|----------|
| GET | /api/v1/Products |
| GET | /api/v1/Products/{id} |
| POST | /api/v1/Products |
| PUT | /api/v1/Products/{id} |
| DELETE | /api/v1/Products/{id} |

---

# Running the Project

## Clone Repository

```bash
git clone https://github.com/YourGitHubUserName/ProductApi.git
```

---

## Open Solution

Open

```
ProductApi.sln
```

using Visual Studio 2022.

---

## Update Connection String

Inside

```
appsettings.json
```

Update

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=ProductSampleDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## Apply Database Migration

Open Package Manager Console

```powershell
Update-Database
```

---

## Run Application

Press

```
F5
```

or

```
Ctrl + F5
```

Swagger will open automatically.

---

# Running Tests

Using Test Explorer

or

```bash
dotnet test
```

---

# Docker

Build

```bash
docker build -t productapi .
```

Run

```bash
docker run -p 8080:80 productapi
```

---

# Swagger

Swagger UI

```
https://localhost:xxxx/swagger
```

---

# Sample Product Request

```json
{
  "productName": "Nike T Shirt"
}
```

---

# Sample Response

```json
{
  "id": 1,
  "productName": "Nike T Shirt",
  "createdBy": "Admin",
  "createdOn": "2026-07-04T10:30:00Z"
}
```

---

# Testing

The solution contains unit tests for

- Product Controller
- Product Service
- Repository

Frameworks

- xUnit
- Moq

---

# Logging

Structured logging implemented using Serilog.

Logs are written to

- Console
- File

---

# Security

- JWT Authentication
- Refresh Token Authentication
- HTTPS
- Role Based Authorization
- Global Exception Handling
- FluentValidation

---

# Performance

- Async/Await
- AsNoTracking()
- Pagination
- Response Compression

---

# Author

Sushil Danawale
