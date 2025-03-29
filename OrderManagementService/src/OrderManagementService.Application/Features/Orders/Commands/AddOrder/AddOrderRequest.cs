using OrderManagementService.Domain.Entities;
using OrderManagementService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementService.Application.Features.Orders.Commands.AddOrder
{
    public class AddOrderRequest
    {
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }
        public Status Status { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid BookId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }


    }   
}
