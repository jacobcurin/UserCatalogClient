# Fase base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Fase build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UserCatalogMvc.csproj", "."]
RUN dotnet restore "./UserCatalogMvc.csproj"
COPY . .
RUN dotnet build "./UserCatalogMvc.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Fase publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserCatalogMvc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Fase final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserCatalogMvc.dll"]
