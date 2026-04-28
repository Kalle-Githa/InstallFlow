# Stage 1: Bygg applikationen
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["InstallFlow.csproj", "."]
RUN dotnet restore "InstallFlow.csproj"

COPY . .
WORKDIR "/src"
RUN dotnet publish "InstallFlow.csproj" -c Release -o /app/publish

# Stage 2: Kör applikationen
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "InstallFlow.dll"]