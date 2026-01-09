using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Mobile.Models;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Status { get; set; }
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
}
