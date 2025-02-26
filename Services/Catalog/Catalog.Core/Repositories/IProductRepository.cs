using Catalog.Core.Entities;
using Catalog.Core.Specs;

namespace Catalog.Core.Repositories;

public interface IProductRepository
{
    Task<Pagination<Product>> GetAllProducts(CatalogSpecParams catalogSpecParams);

    Task<Product> GetProduct(string id);

    Task<IEnumerable<Product>> GetProductsByName(string name);

    Task<IEnumerable<Product>> GetProductsByBrand(string brand);

    Task<Product> CreateProduct(Product product);

    Task<bool> UpdateProduct(Product product);

    Task<bool> DeleteProduct(string id);
}