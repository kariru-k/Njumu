using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
{
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductBrand>> GetAllBrands()
    {
        return await _context
            .Brands
            .Find(brand => true)
            .ToListAsync();
    }

    public async Task<Pagination<Product>> GetAllProducts(CatalogSpecParams catalogSpecParams)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrEmpty(catalogSpecParams.Search))
            filter &= builder.Where(p =>
                p.Name.Contains(catalogSpecParams.Search, StringComparison.CurrentCultureIgnoreCase));

        if (!string.IsNullOrEmpty(catalogSpecParams.BrandId))
        {
            var brandFilter = builder.Eq(p => p.Brands.Id, catalogSpecParams.BrandId);
            filter &= brandFilter;
        }

        if (!string.IsNullOrEmpty(catalogSpecParams.TypeId))
        {
            var typeFilter = builder.Eq(p => p.Types.Id, catalogSpecParams.TypeId);
            filter &= typeFilter;
        }

        var totalItems = await _context.Products.CountDocumentsAsync(filter);


        var data = await DataFilter(catalogSpecParams, filter);


        // var data = await _context.Products.Find(filter)
        //     .Skip((catalogSpecParams.PageIndex - 1) * catalogSpecParams.PageSize)
        //     .Limit(catalogSpecParams.PageSize)
        return new Pagination<Product>(
            catalogSpecParams.PageIndex,
            catalogSpecParams.PageSize,
            (int)totalItems,
            data
        );
    }


    public async Task<Product> GetProduct(string id)
    {
        return await _context
            .Products
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByName(string name)
    {
        return await _context
            .Products
            .Find(p => p.Name.ToLower() == name.ToLower())
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByBrand(string brand)
    {
        return await _context
            .Products
            .Find(p => p.Brands.Name.ToLower() == brand.ToLower())
            .ToListAsync();
    }

    public async Task<Product> CreateProduct(Product product)
    {
        await _context
            .Products
            .InsertOneAsync(product);

        return product;
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updatedProduct = await _context
            .Products
            .ReplaceOneAsync(p => p.Id == product.Id, product);

        return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var deletedProduct = await _context
            .Products
            .DeleteOneAsync(p => p.Id == id);

        return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
    }

    public async Task<IEnumerable<ProductType>> GetAllTypes()
    {
        return await _context
            .Types
            .Find(type => true)
            .ToListAsync();
    }


    private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams,
        FilterDefinition<Product> filter)
    {
        var sortDefn = Builders<Product>.Sort.Ascending("Name"); //The default choice

        if (!string.IsNullOrEmpty(catalogSpecParams.Sort))
            switch (catalogSpecParams.Sort)
            {
                case "priceAsc":
                    sortDefn = Builders<Product>.Sort.Ascending(p => p.Price);
                    break;
                case "priceDesc":
                    sortDefn = Builders<Product>.Sort.Descending(p => p.Price);
                    break;
            }

        return await _context
            .Products
            .Find(filter)
            .Sort(sortDefn)
            .Skip((catalogSpecParams.PageIndex - 1) * catalogSpecParams.PageSize)
            .Limit(catalogSpecParams.PageSize)
            .ToListAsync();
    }
}