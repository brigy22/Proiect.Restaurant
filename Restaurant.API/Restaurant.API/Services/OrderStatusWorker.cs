using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;
using Restaurant.API.Models;

namespace Restaurant.API.Services;

public class OrderStatusWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public OrderStatusWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<RestaurantApiDbContext>();

            var now = DateTime.UtcNow;

            var orders = await db.Orders.ToListAsync(stoppingToken);

            foreach (var o in orders)
            {
                if (o.Status == OrderStatus.Cancelled || o.Status == OrderStatus.Delivered)
                    continue;

                var age = now - o.CreatedAt;

                if (o.Status == OrderStatus.New && age >= TimeSpan.FromMinutes(1))
                    o.Status = OrderStatus.Preparing;
                else if (o.Status == OrderStatus.Preparing && age >= TimeSpan.FromMinutes(2))
                    o.Status = OrderStatus.Ready;
                else if (o.Status == OrderStatus.Ready && age >= TimeSpan.FromMinutes(3))
                    o.Status = OrderStatus.Delivered;
            }

            await db.SaveChangesAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
