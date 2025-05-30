# Order Service API

This project is an ASP.NET Core Web API for managing customer orders.

## Features
- CRUD endpoints for `Order` (OrderId, CustomerId, OrderDate, Items, TotalAmount)
- In-memory repository for orders
- RESTful best practices: attribute routing, async methods, proper HTTP status codes
- OpenAPI/Swagger documentation

## Getting Started
1. Build the project:
   ```
   dotnet build order-service/order-service.csproj
   ```
2. Run the project:
   ```
   dotnet run --project order-service/order-service.csproj
   ```
3. Access Swagger UI at `https://localhost:5001/swagger` (default)

---

For workspace-specific Copilot instructions, see `.github/copilot-instructions.md`.
