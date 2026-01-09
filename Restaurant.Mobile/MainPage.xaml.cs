using Restaurant.Mobile.Models;
using Restaurant.Mobile.Services;

namespace Restaurant.Mobile;

public partial class MainPage : ContentPage
{
    private readonly ApiService _api;

    private readonly List<CartLine> _cart = new();

    private class CartLine
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public MainPage(ApiService api)
    {
        InitializeComponent();
        _api = api;

        RefreshCartUI();
    }

    private async void OnLoadMenu(object sender, EventArgs e)
    {
        try
        {
            var items = await _api.GetMenuItemsAsync();
            MenuList.ItemsSource = items;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnLoadOrders(object sender, EventArgs e)
    {
        try
        {
            var orders = await _api.GetOrdersAsync();
            OrdersList.ItemsSource = orders.OrderByDescending(o => o.CreatedAt).ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void OnAddToCart(object sender, EventArgs e)
    {
        if (sender is not Button btn || btn.CommandParameter is not MenuItemDto item)
            return;

        var existing = _cart.FirstOrDefault(x => x.MenuItemId == item.Id);

        if (existing == null)
        {
            _cart.Add(new CartLine
            {
                MenuItemId = item.Id,
                Name = item.Name,
                Price = item.Price,
                Quantity = 1
            });
        }
        else
        {
            existing.Quantity++;
        }

        RefreshCartUI();
    }

    private void OnCartIncrease(object sender, EventArgs e)
    {
        if (sender is not Button btn || btn.CommandParameter is not CartLine line)
            return;

        line.Quantity++;
        RefreshCartUI();
    }

    private void OnCartDecrease(object sender, EventArgs e)
    {
        if (sender is not Button btn || btn.CommandParameter is not CartLine line)
            return;

        line.Quantity--;
        if (line.Quantity <= 0)
            _cart.Remove(line);

        RefreshCartUI();
    }

    private async void OnPlaceOrder(object sender, EventArgs e)
    {
        try
        {
            if (!int.TryParse(CustomerIdEntry.Text, out var customerId))
            {
                await DisplayAlert("Validation", "CustomerId invalid", "OK");
                return;
            }

            if (_cart.Count == 0)
            {
                await DisplayAlert("Validation", "Cart is empty", "OK");
                return;
            }

            var dto = new OrderCreateDto
            {
                CustomerId = customerId,
                Items = _cart.Select(x => new OrderItemCreateDto
                {
                    MenuItemId = x.MenuItemId,
                    Quantity = x.Quantity
                }).ToList()
            };

            var id = await _api.CreateOrderAsync(dto);

            _cart.Clear();
            RefreshCartUI();

            await DisplayAlert("OK", $"Order created: {id}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void RefreshCartUI()
    {
        CartList.ItemsSource = null;
        CartList.ItemsSource = _cart;

        var total = _cart.Sum(x => x.Price * x.Quantity);
        TotalLabel.Text = $"Total: {total:0.00}";
    }
}
