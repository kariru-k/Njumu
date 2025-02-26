﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Catalog/Catalog.API/Catalog.API.csproj", "Catalog/Catalog.API/"]
COPY ["Catalog/Catalog.Core/Catalog.Core.csproj", "Catalog/Catalog.Core/"]
COPY ["Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "Catalog/Catalog.Infrastructure/"]
COPY ["Catalog/Catalog.Application/Catalog.Application.csproj", "Catalog/Catalog.Application/"]
RUN dotnet restore "Catalog/Catalog.API/Catalog.API.csproj"
COPY . .
WORKDIR "Catalog.API"
RUN dotnet build "./Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog/Catalog.API.dll"]
