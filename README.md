# InstallFlow

Backend-API för ett installationsföretag. Hanterar kunder, uppdrag, jobb,
produkter och en orderkorg-flow för material och arbete. Byggt som
inlämningsuppgift i kursen Molnutveckling med Azure och utvecklas vidare
som portfolio-projekt.

## Stack

**Applikation**
- C# / ASP.NET Core Web API (.NET 10)
- Entity Framework Core + SQL Server
- JWT-autentisering (BCrypt för lösenord)
- JSON Patch för partiella uppdateringar
- Scalar + OpenAPI för API-dokumentation
- Newtonsoft.Json (krävs för JSON Patch-stöd)

**Cloud & DevOps**
- Docker (multi-stage build)
- Azure Container Registry (ACR)
- Azure App Service (kör containern)
- Azure SQL Database
- Application Insights för telemetri
- Azure Pipelines (CI/CD från Azure Repos)
- Managed Identity för ACR-pull (ingen credentials-hantering)

## Funktioner

**Auth**
- JWT-baserad inloggning via `POST /api/auth/login`
- Rollbaserad auktorisering (Admin / User)
- Resursbaserad auktorisering (användare hanterar sina egna resurser, admin kan allt)
- Lösenord hashade med BCrypt

**Domän**
- Kunder, uppdrag och jobb (med material- och arbetstidsrader)
- Produkter och kategorier (många-till-många via join-tabell)
- Orderkorgar kopplade till användare

**API-design**
- DTO:er separerade från entiteter — interna modeller exponeras aldrig
- Paginering på listendpoints (`?page=1&pageSize=10`)
- JSON Patch på Products och Categories (`PATCH /api/products/{id}`)
- Koppling mellan produkt och kategori kan tas bort utan att radera någon av dem
  (`DELETE /api/categories/{categoryId}/products/{productId}`) — opererar på
  join-tabellen, inte på entiteterna
- Global exception-middleware som mappar interna fel till HTTP-statuskoder
- Layered arkitektur: Controller → Service → Repository

## Köra lokalt

1. Klona repot
2. Sätt connection string och JWT-key via user-secrets:
```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...;Database=InstallFlowDb;..."
   dotnet user-secrets set "Jwt:Key" "din-långa-hemliga-nyckel"
```
3. Kör migrationer: `dotnet ef database update`
4. Starta: `dotnet run`
5. API-docs på `/scalar/v1`

En seed-admin skapas automatiskt vid första start (`admin` / `admin123`).
Byt detta direkt efter första deploy.

## Köra i Docker

```bash
docker build -t installflow:latest .
docker run -p 8080:8080 \
  -e ConnectionStrings__DefaultConnection="..." \
  -e Jwt__Key="..." \
  installflow:latest
```

Notera dubbel-understrecket (`__`) — det är så .NET översätter env-variabler
till hierarkisk konfiguration.

## Deployment

Pushar mot `master` triggar Azure Pipelines, som:

1. Bygger och testar koden
2. Bygger Docker-imagen via `az acr build` (i ACR, inte på agenten)
3. Taggar imagen med build-id + `latest`
4. Pekar om App Service till nya imagen och startar om appen

App Service autentiserar mot ACR via Managed Identity (rollen `AcrPull`) —
inga secrets lagras i pipelinen eller App Service-konfigurationen.

## Arkitektur

```
InstallFlow/
├── Controllers/         API-endpoints
├── Core/
│   ├── Interfaces/      Service-kontrakt
│   └── Services/        Affärslogik
├── Data/
│   ├── Entities/        EF Core-entiteter
│   ├── Enums/           Domän-enums (UserRole, ProductType, UnitType)
│   ├── Interfaces/      Repository-kontrakt
│   ├── Repos/           Repository-implementationer
│   ├── DTO/             Request/response-modeller per domän
│   └── Migrations/      EF Core-migrationer
├── Middleware/          Global exception-middleware
├── Dockerfile           Multi-stage build
└── azure-pipelines.yml  CI/CD-pipeline
```