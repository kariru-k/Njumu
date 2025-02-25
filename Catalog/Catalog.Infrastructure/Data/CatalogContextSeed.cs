using System.Text.Json;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public static class CatalogContextSeed
{
    public static void SeedData(IMongoCollection<Product> catalogCollection)
    {
        var checkCatalog = catalogCollection.CountDocuments(FilterDefinition<Product>.Empty) > 0;
        var path = Path.Combine("Data", "SeedData", "products.json");

        if (!checkCatalog)
        {
            var catalogData = File.ReadAllText(path);
            var catalog = JsonSerializer.Deserialize<List<Product>>(catalogData);

            if (catalog != null)
                foreach (var item in catalog)
                    catalogCollection.InsertOneAsync(item);
        }
    }
}