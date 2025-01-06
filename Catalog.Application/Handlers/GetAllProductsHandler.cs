using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetAllProductsHandler: IRequestHandler<GetAllProductsQuery, IList<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    
    
    public async Task<IList<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Product> productList = await _productRepository.GetAllProducts();

        var productResponseList = ProductMapper.Mapper.Map<IList<Product>, IList<ProductResponse>>(productList.ToList());

        return productResponseList;
    }
}