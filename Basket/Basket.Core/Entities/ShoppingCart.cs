namespace Basket.Core.Entities;

public class ShoppingCart
{
    public ShoppingCart()
    {
    }

    public ShoppingCart(string userName, List<ShoppingCartItem> items)
    {
        UserName = userName;
        Items = items;
    }

    public string UserName { get; set; }

    public List<ShoppingCartItem> Items { get; set; }
}