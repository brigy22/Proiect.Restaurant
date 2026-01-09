using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Web.Data;

namespace Restaurant.Web.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _context.MenuItems
                .Include(m => m.MenuCategory)
                .OrderBy(m => m.MenuCategory!.Name)
                .ThenBy(m => m.Name)
                .ToListAsync();

            return View(items);
        }
    }
}
