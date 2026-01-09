using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;
using Restaurant.API.Models;
using Restaurant.API.Dtos;

namespace Restaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly RestaurantApiDbContext _context;

        public OrdersController(RestaurantApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            return order;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderCreateDto dto)
        {
            if (dto.CustomerId <= 0 || dto.Items == null || dto.Items.Count == 0)
                return BadRequest();

            var itemIds = dto.Items.Select(i => i.MenuItemId).Distinct().ToList();

            var menuItems = await _context.MenuItems
                .Where(m => itemIds.Contains(m.Id))
                .ToListAsync();

            if (menuItems.Count != itemIds.Count)
                return BadRequest();

            decimal total = 0m;

            foreach (var item in dto.Items)
            {
                var menu = menuItems.First(m => m.Id == item.MenuItemId);
                total += menu.Price * item.Quantity;
            }

            var order = new Order
            {
                CustomerId = dto.CustomerId,
                CreatedAt = DateTime.UtcNow,
                Status = 0,
                TotalAmount = total
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in dto.Items)
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity
                });
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
                return BadRequest();

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
