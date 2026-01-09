using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Mobile.Models;

public class OrderItemCreateDto
{
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }
}
