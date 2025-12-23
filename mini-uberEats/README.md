# mini-uberEats

Small demo ASP.NET Core Web API for managing orders (EF Core + SQLite). This repository contains a lightweight orders API and a simple read-model bridge used for queries.

## Requirements

- .NET 10 SDK
- Docker & docker-compose (optional — used to run RabbitMQ for background workers)

## Quick start (Windows)

Build the project:

```bash
dotnet build ./mini-uberEats.csproj
```

Run locally (development):

```bash
dotnet run --project ./mini-uberEats.csproj
```

By default Swagger is enabled in Development. Open the URL shown in the console (https://localhost:<port>) to use the API UI.

## Docker (optional)

The repository includes a `docker-compose.yml` that starts a RabbitMQ container used by the read-model worker. To run it:

```bash
docker-compose up -d
```

This exposes RabbitMQ on ports 5672 and 15672 (management UI).

## Database

The app uses SQLite for persistence. During development the app creates the database files automatically via EF Core `EnsureCreated()`.

- Local DB files (development) created in the project folder: `orders.db`, `orders.db-wal`, `orders.db-shm`.
- These files should generally not be committed to source control — see `.gitignore` included in the repo to exclude them.

If you prefer migrations, add EF Core migrations and apply them with `dotnet ef database update`.

## API

Endpoints provided by the project (Controllers/OrdersController.cs):

- POST /api/orders — Create a new order.
  - Request body JSON: `{ "customerName": "Name", "items": ["item1","item2"], "totalAmount": 12.99 }`
  - Response: `{ "orderId": "<guid>" }`

- GET /api/orders/{id} — Retrieve order read-model by id. Returns 200 with the view or 404 if not found.

Example curl (create order):

```bash
curl -s -X POST https://localhost:5001/api/orders \
  -H "Content-Type: application/json" \
  -d '{"customerName":"Alice","items":["pizza","soda"],"totalAmount":19.50}'
```

Example curl (get order):

```bash
curl -s https://localhost:5001/api/orders/<order-guid>
```

## Configuration

- `appsettings.json` contains minimal logging configuration and `AllowedHosts`.
- Development URLs and environment are set in `Properties/launchSettings.json` when running from the IDE.

## Development notes

- Project target: .NET 10 (`TargetFramework: net10.0`).
- NuGet packages: Microsoft.EntityFrameworkCore (SQLite provider), Swashbuckle (Swagger), etc. See `mini-uberEats.csproj`.
- The solution includes project references to external projects (`ReadModel.Worker`, `Contracts`) under `../src` — ensure those projects are available when building the full solution.
- EF Core Tools are present in the project; currently the app uses `EnsureCreated()` for development DB bootstrapping.
- Swagger UI is enabled in development to explore the API.

## Recommendations & next steps

- Remove committed SQLite DB files from source control and keep them local only.
- Add a LICENSE and CONTRIBUTING.md.
- If this will run in production, replace local SQLite with a production-grade DB and move secrets to environment variables or a secret manager.

---

If you want, I can also:

- translate this README to Hebrew,
- add more concrete curl/Postman examples and response schemas,
- add CI/CD notes or a release checklist.

Tell me which of the above to include next.
