using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

public class GetProductByBrandQuery : IRequest<IList<ProductResponse>>
{
    public GetProductByBrandQuery(string brand)
    {
        Brand = brand;
    }

    public string Brand { get; set; }
}