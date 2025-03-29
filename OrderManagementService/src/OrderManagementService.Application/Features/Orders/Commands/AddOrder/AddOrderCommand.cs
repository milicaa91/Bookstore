using MediatR;
using OrderManagementService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementService.Application.Features.Orders.Commands.AddOrder
{
    public class AddOrderCommand : IRequest<Guid>
    {
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }
        public Status Status { get; set; }
        public List<OrderItem> Items { get; set; }

        public AddOrderCommand(string userId, DateTime createdAt, decimal total, Status status, List<OrderItem> items)
        {
            UserId = userId;
            CreatedAt = createdAt;
            Total = total;
            Status = status;
            Items = items;
        }
    }
}
