using Catalog.Application.Responses;
using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Application.Commands;

public class UpdateProductCommand: IRequest<bool>
{
    
    public string Id { get; set; }
    public string Name { get; set; }
    
    public string Summary { get; set; }
    
    public string Description { get; set; }
    
    public string ImageFile { get; set; }
    
    public ProductBrand Brands { get; set; }
    
    public ProductType Types { get; set; }
    
    public List<int> AvailableSizes { get; set; } // List of available sizes
    
    public decimal Price { get; set; }
}