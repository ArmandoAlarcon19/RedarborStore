# Redarbor Store API

## Objective
**RESTful API** to manage a product inventory system.

---

## Technology Stack

- **.NET 6**
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
  - Does not depend on EF Core, ASP.NET, or external libraries.

- **Redarbor.Application**
  - Use cases (CQRS): `Commands`, `Queries`, `Handlers`, `DTOs`.
  - Validations (FluentValidation).
  - Interfaces (repositories, Unit of Work, etc.).

- **Redarbor.Infrastructure**
  - Persistence:
    - **EF Core** for queries.
    - **Dapper** for commands.
  - SQL Server, DbContext, repositories, migrations (if applicable).
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
---
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

### Categorías
- `GET /categories` *(EF Core - Query)*
- `GET /categories/{id}` *(EF Core - Query)*
- `POST /categories` *(Dapper - Command)*
- `PUT /categories/{id}` *(Dapper - Command)*
- `DELETE /categories/{id}` *(Dapper - Command)*

### Movimientos de inventario
- `POST /inventory/movements` → registrar entrada/salida *(Dapper - Command)*
- `GET /inventory/movements` → listar movimientos *(EF Core - Query)*

---

## Autenticación (OAuth2)

Los endpoints se protegen con OAuth2 (Bearer tokens).  
Para consumir endpoints protegidos:

1. Obtén un **access token** desde tu proveedor OAuth2
2. Llama a la API con header:
```http
Authorization: Bearer <access_token>
```

### Swagger + OAuth2
Swagger UI está configurado para autenticar con OAuth2 (botón **Authorize**) si se proveen:
- `SwaggerOAuth__ClientId`
- (Opcional) `SwaggerOAuth__ClientSecret` (solo si el proveedor lo requiere)
- `Auth__Authority` y endpoints de autorización/token

> IMPORTANTE: Ajusta URLs de OAuth2 según proveedor (Auth0/Keycloak/Entra ID).

---

## Pruebas

### Ejecutar todas las pruebas
```bash
dotnet test
```

### Enfoque TDD
- **Domain.Tests**: reglas e invariantes (puro dominio).
- **Application.Tests**: handlers CQRS (mocks de repositorios, etc.).

---

## Decisiones de diseño relevantes

- CQRS:
  - Queries con **EF Core** para lectura (proyecciones/DTOs).
  - Commands con **Dapper** para escritura (SQL explícito, control de performance).
- No FOREIGN KEYS:
  - Reglas de negocio en dominio/aplicación para consistencia.
- Clean Code / SOLID:
  - Handlers con responsabilidad única.
  - Dependencias invertidas (Application define interfaces, Infrastructure implementa).

---

## Cómo depurar en local (VS Code)

1. Abre el workspace:
```bash
code .
```

2. Ejecuta la API:
```bash
dotnet run --project ./Redarbor.Api
```


