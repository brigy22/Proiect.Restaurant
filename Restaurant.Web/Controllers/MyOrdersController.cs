using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Web.Data;
using Restaurant.Web.Models;

namespace Restaurant.Web.Controllers
{
    [Authorize]
    public class MyOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var email = User?.Identity?.Name;

            if (string.IsNullOrWhiteSpace(email))
                return Challenge();

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


            var orders = await _context.Orders
                .Where(o => o.CustomerId == customer.Id)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return View(orders);
        }
    }
}
