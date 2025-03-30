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
        public List<OrderedItem> Items { get; set; }

        public AddOrderCommand(string userId, List<OrderedItem> items)
        {
            UserId = userId;
            Items = items;
        }
    }
}
