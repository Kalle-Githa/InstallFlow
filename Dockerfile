

# Stage 1: Base-image – minimal runtime-miljö som används i slutsteget
# Innehåller bara ASP.NET Core runtime, inte SDK (liten och säker)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

# Kör som icke-root användare – viktigt för säkerhet i produktion
USER $APP_UID

# Arbetskatalog inuti containern där appen kommer ligga
WORKDIR /app

# Dokumenterar att appen lyssnar på port 8080 (HTTP) och 8081 (HTTPS)
# Öppnar inte faktiska portar – det sköts av docker run eller Azure
EXPOSE 8080
EXPOSE 8081


# Stage 2: Build – kompilera och publicera applikationen
# SDK-imagen är större (~800MB) men behövs bara här, inte i slutresultatet
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Kopierar BARA projektfilen först – Docker cachar detta lager separat
# Om .csproj inte ändrats hoppar Docker över restore vid nästa bygge
COPY ["InstallFlow.csproj", "."]

# Laddar ner NuGet-paket – körs bara om om .csproj har ändrats (cache)
RUN dotnet restore "InstallFlow.csproj"

# Kopierar resten av källkoden efter restore – bättre cache-utnyttjande
COPY . .

# Publicerar appen direkt (dotnet publish bygger automatiskt, inget separat build-steg behövs)
# /p:UseAppHost=false = generera ingen .exe – onödig i Linux-container
RUN dotnet publish "InstallFlow.csproj" -c Release -o /app/publish /p:UseAppHost=false


# Stage 3: Final – den image som faktiskt körs i produktion
# Byggs från base (runtime only, ~200MB) – SDK och källkod lämnas bakom
FROM base AS final

WORKDIR /app

# Kopierar bara den färdiga publicerade outputen från build-steget
# Ingen källkod, ingen SDK, inga byggartefakter hamnar i slutimagen
COPY --from=build /app/publish .

# Startkommando när containern körs
ENTRYPOINT ["dotnet", "InstallFlow.dll"]