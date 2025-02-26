FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Discount/Discount.API/Discount.API.csproj", "Discount/Discount.API/"]
COPY ["Discount/Discount.Application/Discount.Application.csproj", "Discount/Discount.Application/"]
COPY ["Discount/Discount.Core/Discount.Core.csproj", "Discount/Discount.Core/"]
COPY ["Discount/Discount.Infrastructure/Discount.Infrastructure.csproj", "Discount/Discount.Infrastructure/"]
RUN dotnet restore "Discount/Discount.API/Discount.API.csproj"
COPY . .
WORKDIR "/src/Discount.API"
RUN dotnet build "Discount/Discount.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Discount.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discount/Discount.API.dll"]
