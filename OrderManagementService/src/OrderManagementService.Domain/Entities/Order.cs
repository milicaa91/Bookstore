using OrderManagementService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementService.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        public decimal Total { get; set; }
        public Status Status { get; set; } = Status.Pending;

        public List<OrderItem> Items { get; set; }
    }
}
