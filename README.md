# Redarbor Store API

## Objective
**RESTful API** to manage a product inventory system.

---

## Technology Stack

- **.NET 6**
- **ASP.NET Web API**
- **EF Core** (read-only)
- **Dapper** (write-only)
- **SQL Server** 
- **Swagger**
- **xUnit**
- **Docker**

---

## Architecture

The solution is organized by layers following a **Clean Architecture** approach:

- **Redarbor.Domain**
  - Entities, Enums, Repository Interfaces (CQRS).

- **Redarbor.Application**
  - Use cases (CQRS): `Commands`, `Queries`, `Handlers`, `DTOs`.

- **Redarbor.Infrastructure**
  - Persistence:
    - **EF Core** for queries.
    - **Dapper** for commands.
  - Data access configuration.

- **Redarbor.Api**
  - API endpoints, Dockerfile, OAuth2, Swagger.

- **Tests (TDD)**
  - `Redarbor.Domain.Tests`: Domain unit tests.
  - `Redarbor.Application.Tests`: Unit tests for handlers/use cases.

---

## Prerequisites

### Local
- [.NET SDK 6](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker + Docker Compose](https://www.docker.com/products/docker-desktop/)

Verify:
```bash
dotnet --version
docker --version
docker compose version
```
## Environment Variables Configuration

This project uses configuration via `appsettings.json` and environment variables.

---

## Running with Docker (API + DB)

### Start API and DB with initial data
```bash
docker compose up -d --build
```

Services:
- `db`: SQL Server
- `db_init`: Create structure and load principal data
- `api`: RedarborStore API

Swagger:
- `http://localhost:5001/swagger`

### Check status
```bash
docker compose ps
```

### Stop
```bash
docker compose down -v
```
---

## Database

### Eschema
Includes:
- **Products**
- **Categories**
- **Inventory Movements (Entry/Exit)**

> Important: **FOREIGN KEYS are not used** by requirement.  

### Table Creation
SQL script located at `./init.sql` executed when the container starts

---

## Endpoints
### Products
- `GET /Products` → list products *(EF Core - Query)*
- `GET /Products/{id}` → get product by id *(EF Core - Query)*
- `POST /Products` → create product *(Dapper - Command)*
- `PUT /Products/{id}` → update product *(Dapper - Command)*
- `DELETE /Products/{id}` → delete product *(Dapper - Command)*
- `GET /Products/category/{categoryId}` → get products by categoryId *(EF Core - Query)*

### Categories
- `GET /Categories` → list categories *(EF Core - Query)*
- `GET /Categories/{id}` → get category by id  *(EF Core - Query)*
- `POST /Categories` → create category *(Dapper - Command)*
- `PUT /Categories/{id}` → update category *(Dapper - Command)*
- `DELETE /Categories/{id}` → delete category *(Dapper - Command)*

### Inventory Movements
- `POST /InventoryMovements` → register Entry/Exit movement *(Dapper - Command)*
- `GET /InventoryMovements` → list movements *(EF Core - Query)*
- `GET /InventoryMovements/product/{productId}` → list movements by productId *(EF Core - Query)*

---

## Authentication (OAuth2)

Endpoints are protected using OAuth2 (Bearer tokens).

### Swagger + OAuth2
Swagger UI is configured to authenticate using OAuth2 (via the **Authorize** button) when the following are provided:
- `Domain`
- `Audience`
- `ClientId`

---

## Testing

### Run all tests
```bash
dotnet test
```

### TDD Approach
- **Domain.Tests**: Entities rules.
- **Application.Tests**: CQRS handlers.

---

## Local Debugging (VS Code)

1. Open the workspace:
```bash
code .
```

2. Run the API:
```bash
dotnet run --project ./src/Redarbor.Api
```


