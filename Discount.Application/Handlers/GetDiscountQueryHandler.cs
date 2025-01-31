using Discount.Application.Mappers;
using Discount.Application.Queries;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers;

public class GetDiscountQueryHandler: IRequestHandler<GetDiscountQuery, CouponModel>
{
    private readonly IDiscountRepository _discountRepository;

    public GetDiscountQueryHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }
    
    
    public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
    {
        var discount = await _discountRepository.GetDiscount(request.ProductName);

        if (discount == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Discount not found for the provided product Name = {request.ProductName}"));
        }
        

        var discountResponse = DiscountMapper.Mapper.Map<CouponModel>(discount);

        return discountResponse;
    }
}