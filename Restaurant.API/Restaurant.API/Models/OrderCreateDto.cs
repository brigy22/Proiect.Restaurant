namespace Restaurant.API.Dtos;

public class OrderCreateDto
{
    public int CustomerId { get; set; }
    public List<OrderItemCreateDto> Items { get; set; } = new();
}

public class OrderItemCreateDto
{
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }
}
