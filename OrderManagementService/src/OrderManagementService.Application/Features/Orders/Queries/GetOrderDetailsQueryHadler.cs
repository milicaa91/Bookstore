using MediatR;
using OrderManagementService.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementService.Application.Features.Orders.Queries
{
    public class GetOrderDetailsQueryHadler : IRequestHandler<GetOrderDetailsQuery, GetOrderDetailsResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderDetailsQueryHadler(IOrderRepository bookRepository)
        {
            _orderRepository = bookRepository;
        }

        public async Task<GetOrderDetailsResponse> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
        {
            if (request is null || request.Id == Guid.Empty)
                throw new ArgumentNullException($"Order details can't be fetched");

            return await _orderRepository.GetOrderById(request.Id);
        }
    }
}
