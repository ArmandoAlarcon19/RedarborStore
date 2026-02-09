# Redarbor Store API

## Objetivo
**API RESTful** para gestionar un sistema de inventario de productos.

---

## Stack Tecnológico

- **.NET 6**
- **ASP.NET Core Web API** (Minimal API)
- **EF Core** (solo lectura)
- **Dapper** (solo escritura)
- **SQL Server** (Docker)
- **Swagger (Swashbuckle)**
- **xUnit** (unit testing)

---

## Arquitectura

La solución está organizada por capas siguiendo un enfoque tipo **Clean Architecture**:

- **Redarbor.Domain**
  - Entidades, Value Objects, reglas de negocio, eventos de dominio.
  - No depende de EF Core, ASP.NET, ni librerías externas.

- **Redarbor.Application**
  - Casos de uso (CQRS): `Commands`, `Queries`, `Handlers`, `DTOs`.
  - Validaciones (FluentValidation).
  - Interfaces (repositorios, UnitOfWork, etc.).

- **Redarbor.Infrastructure**
  - Persistencia:
    - **EF Core** para consultas.
    - **Dapper** para comandos.
  - SQL Server, DbContext, repositorios, migraciones (si aplica).
  - Configuración de acceso a datos.

- **Redarbor.Api**
  - Minimal API endpoints, DI, middleware, autenticación, swagger.

- **Tests**
  - `Redarbor.Domain.Tests`: pruebas unitarias del dominio.
  - `Redarbor.Application.Tests`: pruebas unitarias de handlers/casos de uso.

---

## Requisitos Previos

### Local
- [.NET SDK 6](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker + Docker Compose](https://www.docker.com/products/docker-desktop/)

Verificar:
```bash
dotnet --version
docker --version
docker compose version
```

---

## Configuración de variables de entorno

Este proyecto usa configuración por `appsettings.json`/variables de entorno.

Variables típicas:
- `ConnectionStrings__SqlServer` → Connection string a SQL Server
- Configuración OAuth2 (depende del proveedor):
  - `Auth__Authority`
  - `Auth__Audience`
  - `SwaggerOAuth__ClientId`

> Nota: La configuración exacta de OAuth2 depende del proveedor (Auth0, Keycloak, Azure Entra ID, etc.). Ver sección **OAuth2**.

---

## Ejecución con Docker (API + DB)

### Levantar API y BD con carga inicial
```bash
docker compose up -d --build
```

Servicios:
- `db`: SQL Server
- `api`: RedarborStore API

Swagger:
- `http://localhost:5001/swagger`

### Detener
```bash
docker compose down -v
```

---

## Base de Datos

### Esquema
Incluye al menos:
- **Productos**
- **Categorías**
- **Movimientos de Inventario** (entrada/salida)

> Importante: **No se usan FOREIGN KEYS** por requerimiento.  
La consistencia referencial debe manejarse a nivel de aplicación (validaciones y reglas de negocio).

### Migraciones / creación de tablas
Script SQL en `./init.sql` ejecutado al iniciar el contenedor


---

## Endpoints
### Productos
- `GET /products` → listar productos *(EF Core - Query)*
- `GET /products/{id}` → obtener producto por id *(EF Core - Query)*
- `POST /products` → crear producto *(Dapper - Command)*
- `PUT /products/{id}` → actualizar producto *(Dapper - Command)*
- `DELETE /products/{id}` → eliminar producto *(Dapper - Command)*

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


