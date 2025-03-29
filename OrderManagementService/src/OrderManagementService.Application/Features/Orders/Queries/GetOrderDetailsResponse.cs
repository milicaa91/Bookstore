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

        public Guid OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } // Enum converted to string for readability
        public List<OrderItemResponse> Items { get; set; }
    }

    public class OrderItemResponse
    {
        public Guid ItemId { get; set; }
        public string ProductName { get; set; } // If fetching from Book Catalog Service
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
