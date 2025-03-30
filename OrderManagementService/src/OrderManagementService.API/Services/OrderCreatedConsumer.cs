using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderManagementService.Domain.Enums;
using OrderManagementService.Domain.Events;
using OrderManagementService.Infrastructure;
using System;

namespace OrderManagementService.API.Services
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly OrderDbContext _context;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(OrderDbContext context, ILogger<OrderCreatedConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var orderEvent = context.Message;

            _logger.LogInformation($"OrderCreatedEvent received: OrderId {orderEvent.OrderId}");

            //TODO Simulated logic: Update stock, send notifications, etc.
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderEvent.OrderId);

            if (order != null)
            {
                if (order.Status == Status.Processed)
                {
                    _logger.LogWarning($"Ignoring duplicate order {context.Message.OrderId}");
                    return;
                }

                order.Status = Status.Processed;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Order {orderEvent.OrderId} marked as Processed.");
            }
            else
            {
                _logger.LogError($"Order not found: {context.Message.OrderId}");
            }
        }
    }
}
