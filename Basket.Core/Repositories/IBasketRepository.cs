using Basket.Core.Entities;

namespace Basket.Core.Repositories;

public interface IBasketRepository
{
    
    Task<ShoppingCart> GetBasket(string userName);
    
    Task<ShoppingCart> UpdateBasket(ShoppingCart basket);

    Task<bool> DeleteBasket(string userName);


}