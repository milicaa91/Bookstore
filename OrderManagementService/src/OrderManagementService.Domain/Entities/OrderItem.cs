using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementService.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; } = new Guid();
        public Guid OrderId { get; set; }
        public Guid BookId { get; set; }
        public int Quantity { get; set; } 
        public decimal Price { get; set; } 
    }
}
