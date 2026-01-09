using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Mobile.Models;

public class OrderCreateDto
{
    public int CustomerId { get; set; }
    public List<OrderItemCreateDto> Items { get; set; } = new();
}
