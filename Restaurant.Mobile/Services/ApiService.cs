using System.Net.Http.Headers;
using System.Net.Http.Json;
using Restaurant.Mobile.Models;

namespace Restaurant.Mobile.Services;

public class ApiService
{
    private const string ApiPort = "7165";
    private readonly HttpClient _http;

    public ApiService()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };

        _http = new HttpClient(handler)
        {
#if ANDROID
            BaseAddress = new Uri($"https://10.0.2.2:{ApiPort}/")
#else
            BaseAddress = new Uri($"https://localhost:{ApiPort}/")
#endif
        };
    }

    public void SetToken(string token)
    {
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var resp = await _http.PostAsJsonAsync("api/Auth/login", new
        {
            email,
            password
        });

        resp.EnsureSuccessStatusCode();

        var data = await resp.Content.ReadFromJsonAsync<LoginResponseDto>();

        if (data == null || string.IsNullOrWhiteSpace(data.Token))
            throw new Exception("Invalid login response (missing token).");

        SetToken(data.Token);
        return data.Token;
    }

    public async Task<List<MenuItemDto>> GetMenuItemsAsync()
        => await _http.GetFromJsonAsync<List<MenuItemDto>>("api/MenuItems") ?? new();

    public async Task<List<OrderDto>> GetOrdersAsync()
        => await _http.GetFromJsonAsync<List<OrderDto>>("api/Orders") ?? new();

    public async Task<int> CreateOrderAsync(OrderCreateDto order)
    {
        var response = await _http.PostAsJsonAsync("api/Orders", order);
        response.EnsureSuccessStatusCode();

        var id = await response.Content.ReadFromJsonAsync<int>();
        return id;
    }

    private class LoginResponseDto
    {
        public string Token { get; set; } = "";
    }
}
