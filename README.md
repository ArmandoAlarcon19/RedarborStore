# Redarbor Store API

## Objective
**RESTful API** to manage a product inventory system.

---

## Technology Stack

- **.NET 10**
- **ASP.NET Core Web API** (Minimal API)
- **EF Core** (read-only)
- **Dapper** (write-only)
- **SQL Server** (Docker)
- **Swagger (Swashbuckle)**
- **xUnit** (unit testing)

---

## Architecture

The solution is organized by layers following a **Clean Architecture** approach:

- **Redarbor.Domain**
  - Entities, Value Objects, business rules, domain events.

- **Redarbor.Application**
  - Use cases (CQRS): `Commands`, `Queries`, `Handlers`, `DTOs`.
  - Validations.
  - Interfaces.

- **Redarbor.Infrastructure**
  - Persistence:
    - **EF Core** for queries.
    - **Dapper** for commands..
  - Data access configuration.

- **Redarbor.Api**
  - Minimal API endpoints, DI, middleware, authentication, Swagger.

- **Tests**
  - `Redarbor.Domain.Tests`: domain unit tests.
  - `Redarbor.Application.Tests`: unit tests for handlers/use cases.

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

Typical variables:
- `ConnectionStrings__SqlServer` → SQL Server connection string
- OAuth2 configuration (depends on the provider):
  - `Auth__Authority`
  - `Auth__Audience`
  - `SwaggerOAuth__ClientId`

> Note: The exact OAuth2 configuration depends on the provider (Auth0, Keycloak, Azure Entra ID, etc.). See the **OAuth2** section.

---

## Running with Docker (API + DB)

### Start API and DB with initial data
```bash
docker compose up -d --build
```

Services:
- `db`: SQL Server
- `data init`: Load principal data
- `api`: RedarborStore API

Swagger:
- `http://localhost:5001/swagger`

### Stop
```bash
docker compose down -v
```

---

## Database

### Eschema
Includes at least:
- **Products**
- **Categories**
- **Inventory Movements (inbound/outbound)**

> Important: **FOREIGN KEYS are not used** by requirement.  

### Table Creation
SQL script located at `./init.sql` executed when the container starts

---

## Endpoints
### Productos
- `GET /products` → list products *(EF Core - Query)*
- `GET /products/{id}` → get product by id *(EF Core - Query)*
- `POST /products` → create product *(Dapper - Command)*
- `PUT /products/{id}` → update product *(Dapper - Command)*
- `DELETE /products/{id}` → delete product *(Dapper - Command)*

### Categories
- `GET /categories` → list categories *(EF Core - Query)*
- `GET /categories/{id}` → get category by id  *(EF Core - Query)*
- `POST /categories` → create category *(Dapper - Command)*
- `PUT /categories/{id}` → update category *(Dapper - Command)*
- `DELETE /categories/{id}` → delete category *(Dapper - Command)*

### Inventory Movements
- `POST /inventory/movements` → register inbound/outbound movement *(Dapper - Command)*
- `GET /inventory/movements` → list movements *(EF Core - Query)*

---

## Authentication (OAuth2)

Endpoints are protected using OAuth2 (Bearer tokens).
To consume protected endpoints:

1. Obtain an **access token** from your OAuth2 provider.
2. Call the API with the following header:
```http
Authorization: Bearer <access_token>
```

### Swagger + OAuth2
Swagger UI is configured to authenticate using OAuth2 (via the **Authorize** button) when the following are provided:
- `SwaggerOAuth__ClientId`
- (Opcional) `SwaggerOAuth__ClientSecret` only if required by the provider)
- `Auth__Authority` and authorization/token endpoints

> IMPORTANT: Adjust OAuth2 URLs according to the provider (Auth0 / Keycloak / Entra ID).

---

## Testing

### Run all tests
```bash
dotnet test
```

### TDD Approach
- **Domain.Tests**: business rules and invariants.
- **Application.Tests**: CQRS handlers.

---

## Key Design Decisions

- CQRS:
  - Queries use **EF Core** for reads (projections/DTOs).
  - Commands use Dapper for writes (explicit SQL, performance control).
- No FOREIGN KEYS:
  - Business rules in the domain/application layers ensure consistency.
- Clean Code / SOLID:
  - Handlers have a single responsibility.
  - Dependency inversion (Application defines interfaces, Infrastructure implements them).

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


