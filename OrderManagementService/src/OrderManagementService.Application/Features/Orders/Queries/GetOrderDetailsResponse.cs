using OrderManagementService.Domain.Entities;
using OrderManagementService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementService.Application.Features.Orders.Queries
{
    public class GetOrderDetailsResponse
    {
        public GetOrderDetailsResponse()
        {

        }

        public GetOrderDetailsResponse(Guid id, string userId, Status status, DateTime createdAt, decimal total, List<OrderItemResponse> items)
        {
            OrderId = id;
            UserId = userId;
            Status = status;
            CreatedAt = createdAt;
            Total = total;
            Items = items;
        }

        public Guid OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }
        public Status Status { get; set; } // Enum converted to string for readability
        public List<OrderItemResponse> Items { get; set; }
    }

    public class OrderItemResponse
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
