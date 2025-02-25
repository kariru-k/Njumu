namespace Basket.Application.Responses;

public class ShoppingCartResponse
{
    public ShoppingCartResponse()
    {
    }

    public ShoppingCartResponse(string userName)
    {
        UserName = userName;
    }

    public string UserName { get; set; }

    public List<ShoppingCartItemResponse> Items { get; set; } = new();

    public decimal TotalPrice
    {
        get { return Items.Sum(x => x.Price * x.Quantity); }
    }
}