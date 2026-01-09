using Microsoft.EntityFrameworkCore;
using Restaurant.API.Models;
//using Restaurant.Web.Models;

namespace Restaurant.API.Data
{
    public class RestaurantApiDbContext : DbContext
    {
        public RestaurantApiDbContext(DbContextOptions<RestaurantApiDbContext> options) : base(options) { }

        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
