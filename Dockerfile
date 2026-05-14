# ============================================================================
#  InstallFlow – Multi-stage Dockerfile
#  Optimerad för både produktion och Visual Studio fast-mode debugging.
#  Se https://aka.ms/customizecontainer för mer info om VS container-bygge.
# ============================================================================


# ----------------------------------------------------------------------------
# Stage 1: BASE – minimal runtime-miljö som används i slutsteget
# ----------------------------------------------------------------------------
# Innehåller bara ASP.NET Core runtime, inte SDK (liten och säker, ~200MB).
# När VS kör i "fast mode" (F5 Debug) stannar bygget här – VS mountar din
# kompilerade kod från värddatorn istället för att bygga om hela imagen.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

# Kör som icke-root användare – viktigt för säkerhet i produktion.
# $APP_UID är fördefinierad i Microsofts base-image (vanligtvis UID 1654).
USER $APP_UID

# Arbetskatalog inuti containern där appen kommer ligga
WORKDIR /app

# Dokumenterar att appen lyssnar på port 8080 (HTTP) och 8081 (HTTPS).
# Öppnar inte faktiska portar – det sköts av `docker run -p` eller Azure.
EXPOSE 8080
EXPOSE 8081


# ----------------------------------------------------------------------------
# Stage 2: BUILD – kompilera applikationen
# ----------------------------------------------------------------------------
# SDK-imagen är stor (~800MB) men behövs bara här, inte i slutresultatet.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# ARG = byggtidsvariabel. Tillåter `docker build --build-arg BUILD_CONFIGURATION=Debug`
# Standard är Release, men kan överridas för att bygga en debug-version.
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Kopierar BARA projektfilen först – Docker cachar detta lager separat.
# Om .csproj inte ändrats hoppar Docker över restore vid nästa bygge.
COPY ["InstallFlow.csproj", "."]

# Laddar ner NuGet-paket – körs bara om om .csproj har ändrats (cache-trick)
RUN dotnet restore "./InstallFlow.csproj"

# Kopierar resten av källkoden EFTER restore – bättre cache-utnyttjande.
# Ändringar i .cs-filer triggar inte ny restore.
COPY . .

WORKDIR "/src/."

# Bygger projektet. Separerat från publish för att VS ska kunna återanvända
# build-cachen även om publish-stegets argument ändras.
RUN dotnet build "./InstallFlow.csproj" -c $BUILD_CONFIGURATION -o /app/build


# ----------------------------------------------------------------------------
# Stage 3: PUBLISH – publicera till produktionsklar output
# ----------------------------------------------------------------------------
# Eget steg (ärver från build) – ger VS möjlighet att optimera cachning
# mellan build- och publish-faserna.
FROM build AS publish

ARG BUILD_CONFIGURATION=Release

# /p:UseAppHost=false = generera ingen .exe – onödig i Linux-container,
# vi startar appen med `dotnet InstallFlow.dll` ändå.
RUN dotnet publish "./InstallFlow.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# ----------------------------------------------------------------------------
# Stage 4: FINAL – den image som faktiskt körs i produktion
# ----------------------------------------------------------------------------
# Byggs från `base` (runtime only) – SDK och källkod lämnas bakom.
# Slutimagen blir liten och säker: bara runtime + din publicerade app.
FROM base AS final

WORKDIR /app

# Kopierar bara den färdiga publicerade outputen från publish-steget.
# Ingen källkod, ingen SDK, inga byggartefakter hamnar i slutimagen.
COPY --from=publish /app/publish .

# Startkommando när containern körs.
ENTRYPOINT ["dotnet", "InstallFlow.dll"]




# ------------------------------
# CLI kommandon
# ------------------------------

## Bygg produktions-image
#docker build -t installflow:latest .
#
## Kör den
#docker run -p 8080:8080 installflow:latest
#
## Bygg en debug-version
#docker build --build-arg BUILD_CONFIGURATION=Debug -t installflow:debug .

# ------------------------------