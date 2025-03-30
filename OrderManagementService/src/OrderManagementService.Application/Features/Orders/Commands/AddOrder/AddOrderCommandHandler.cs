using Common.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderManagementService.Domain.Entities;
using OrderManagementService.Domain.Enums;
using OrderManagementService.Domain.Events;
using OrderManagementService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementService.Application.Features.Orders.Commands.AddOrder
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBus _bus;
        private readonly ILogger<AddOrderCommandHandler> _logger;
        public AddOrderCommandHandler(IOrderRepository orderRepository, 
            IBus bus, ILogger<AddOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _bus = bus;
            _logger = logger;
        }

        public async Task<Guid> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            var total = request.Items.Sum(i => i.Quantity * i.Price);

            var order = new Order
            {
                UserId = request.UserId,
                Total = total,
                Status = Status.Pending,
                CreatedAt = DateTime.UtcNow,
                Items = request.Items.Select(i => new OrderItem
                {
                    BookId = i.BookId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            await _orderRepository.CreateOrderAsync(order);

            //TODO delete
            //try
            //{
            //    await _bus.Publish(orderCreatedEvent, cancellationToken);
            //    _logger.LogInformation($"Published OrderCreatedEvent for order ID: {order.Id}");
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogInformation($"Failed to publish event: {ex.Message}");
            //}

            return order.Id; //TODO : Return response
        }
    }
}
