using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Web.Data;
using Restaurant.Web.Models;
using System.Text.Json;

namespace Restaurant.Web.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartKey = "CART";

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var email = User?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email)) return Challenge();

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);

            if (customer == null)
            {
                customer = new Customer
                {
                    FullName = email,
                    Email = email,
                    Phone = ""
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            var cart = GetCart();
            if (!cart.Any()) return RedirectToAction("Index", "Cart");

            var order = new Order
            {
                CustomerId = customer.Id,
                Status = OrderStatus.New,
                CreatedAt = DateTime.UtcNow,
                TotalAmount = cart.Sum(x => x.Price * x.Quantity)
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cart)
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                });
            }

            await _context.SaveChangesAsync();

            SaveCart(new List<CartItem>());
            return RedirectToAction("Index", "MyOrders");
        }

        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString(CartKey);
            if (string.IsNullOrEmpty(json)) return new List<CartItem>();
            return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(CartKey, JsonSerializer.Serialize(cart));
        }
    }
}
