using OrderManagementService.Application.Features.Orders.Queries;
using OrderManagementService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementService.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Guid> CreateOrder(Order order);
        Task<GetOrderDetailsResponse> GetOrderById(Guid id);
    }
}
