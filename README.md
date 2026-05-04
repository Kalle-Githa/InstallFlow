# InstallFlow

Backend-API för ett installationsföretag. Hanterar kunder, uppdrag, jobb,
produkter och en orderkorg-flow för material och arbete. Byggt som
inlämningsuppgift och vidareutvecklas löpande.

## Status

🚧 **Pågående utveckling.** Grundfunktionalitet är på plats — autentisering,
kund-, uppdrags- och jobbhantering. Vissa delar (t.ex. JobTemplate) är inte
exponerade ännu och kommer i kommande iterationer.

## Stack

- C# / ASP.NET Core Web API (.NET 10)
- Entity Framework Core + SQL Server
- JWT-autentisering
- Scalar för API-dokumentation
- Docker
- Tänkt deployment: Azure

## Funktioner

**Auth**
- JWT-baserad inloggning
- Rollbaserad auktorisering (Admin / vanlig användare)
- Lösenord hashade med BCrypt

**Domän**
- Kunder och uppdrag
- Jobb med material- och arbetstidsrader
- Produkter och kategorier
- Orderkorgar kopplade till jobb

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
Byt detta innan produktion.

## Arkitektur

```
InstallFlow/
├── Controllers/      API-endpoints
├── Core/
│   ├── Interfaces/   Service-kontrakt
│   └── Services/     Affärslogik
├── Data/
│   ├── Entities/     EF Core-entiteter
│   ├── Repos/        Repository-lager
│   ├── DTO/          Request/response-modeller
│   └── Migrations/
└── Middleware/       Globalt felhanterings-middleware
```