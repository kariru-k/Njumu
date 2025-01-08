using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class UpdateProductCommandHandler: IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productEntity = await _productRepository.UpdateProduct(new Product
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description,
            ImageFile = request.ImageFile,
            AvailableSizes = request.AvailableSizes,
            Brands = request.Brands,
            Summary = request.Summary,
            Price = request.Price,
            Types = request.Types
        });

        return true;
    }
}