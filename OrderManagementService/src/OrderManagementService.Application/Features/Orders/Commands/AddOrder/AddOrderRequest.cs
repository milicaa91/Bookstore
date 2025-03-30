using Newtonsoft.Json;
using OrderManagementService.Domain.Entities;
using OrderManagementService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrderManagementService.Application.Features.Orders.Commands.AddOrder
{
    public class AddOrderRequest
    {
        [JsonProperty("UserId")]
        public string UserId { get; set; }

        [JsonProperty("Items")]
        public List<OrderedItem> Items { get; set; }
    }

    public class OrderedItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("BookId")]
        public Guid BookId { get; set; }

        [JsonProperty("Quantity")]
        public int Quantity { get; set; }

        [JsonProperty("Price")]
        public decimal Price { get; set; }
    }   
}
