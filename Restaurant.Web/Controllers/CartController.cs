using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Web.Data;
using Restaurant.Web.Models;
using System.Text.Json;

namespace Restaurant.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartKey = "CART";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(GetCart());
        }

        [HttpPost]
        public async Task<IActionResult> Add(int menuItemId, int quantity = 1)
        {
            if (quantity < 1) quantity = 1;

            var menuItem = await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == menuItemId);
            if (menuItem == null) return RedirectToAction("Index", "Menu");

            var cart = GetCart();
            var existing = cart.FirstOrDefault(c => c.MenuItemId == menuItemId);

            if (existing != null)
                existing.Quantity += quantity;
            else
                cart.Add(new CartItem
                {
                    MenuItemId = menuItem.Id,
                    Name = menuItem.Name,
                    Price = menuItem.Price,
                    Quantity = quantity
                });

            SaveCart(cart);
            return RedirectToAction("Index", "Menu");
        }

        [HttpPost]
        public IActionResult Remove(int menuItemId)
        {
            var cart = GetCart();
            cart.RemoveAll(c => c.MenuItemId == menuItemId);
            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Clear()
        {
            SaveCart(new List<CartItem>());
            return RedirectToAction(nameof(Index));
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
