using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public class CatalogContext: ICatalogContext
{
    public IMongoCollection<Product> Products { get; }
    
    public IMongoCollection<ProductBrand> Brands { get; }
    
    public IMongoCollection<ProductType> Types { get; }

    public CatalogContext(IConfiguration configuration)
    {
        // Determine the environment (Local or Docker)
        var environment = configuration.GetValue<string>("Environment") ?? "Local";

        // Get the MongoDB settings for the current environment
        var mongoSettings = configuration.GetSection($"MongoDB:{environment}");

        var connectionString = mongoSettings.GetValue<string>("ConnectionString");
        var databaseName = mongoSettings.GetValue<string>("DatabaseName");
        var brandsCollectionName = mongoSettings.GetValue<string>("BrandsCollection");
        var typesCollectionName = mongoSettings.GetValue<string>("TypesCollection");
        var productsCollectionName = mongoSettings.GetValue<string>("ProductsCollection");

        // Initialize the MongoDB client and database
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        // Initialize collections
        Brands = database.GetCollection<ProductBrand>(brandsCollectionName);
        Types = database.GetCollection<ProductType>(typesCollectionName);
        Products = database.GetCollection<Product>(productsCollectionName);

        // Seed data if needed
        BrandContextSeed.SeedData(Brands);
        TypeContextSeed.SeedData(Types);
        CatalogContextSeed.SeedData(Products);
    }
}