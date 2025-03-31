using Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using OrderManagementService.Application.Features.Orders.Queries;
using OrderManagementService.Application.Interfaces.Repositories;
using OrderManagementService.Domain.Entities;
using OrderManagementService.Domain.Enums;
using OrderManagementService.Domain.Events;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OrderManagementService.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order, Guid>, IOrderRepository
    {
        //TODO cache for order details private readonly IDistributedCache _cache;
        ILogger<OrderRepository> _logger;
        public OrderRepository(OrderDbContext context, ILogger<OrderRepository> logger) : base(context)
        {
            _logger = logger;
        }
        public async Task<Guid> CreateOrder(Order order)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                var orderCreatedEvent = new OrderCreatedEvent
                {
                    OrderId = order.Id,
                    UserId = order.UserId,
                    CreatedAt = order.CreatedAt               
                };

                var payload = JsonSerializer.Serialize(orderCreatedEvent);

                var outboxEvent = new OutboxEvent
                {
                    EventType = EventType.OrderCreated,
                    Payload = payload,
                    Status = Status.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.OutboxEvents.AddAsync(outboxEvent);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return order.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError($"Failed to create order: {ex.Message}");
                throw;
            }
        }
        public async Task<GetOrderDetailsResponse> GetOrderById(Guid id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
                throw new KeyNotFoundException($"Order with id {id} not found.");

            var orderModel = new GetOrderDetailsResponse(order.Id,
                order.UserId,
                order.Status,
                order.CreatedAt,
                order.Total,
                order.Items.Select(item => new OrderItemResponse
                {
                    ItemId = item.Id,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList());

            return orderModel;
        }
    }
}
