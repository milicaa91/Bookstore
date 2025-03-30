using Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using OrderManagementService.Domain.Entities;
using OrderManagementService.Domain.Enums;
using OrderManagementService.Domain.Events;
using OrderManagementService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrderManagementService.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order, Guid>, IOrderRepository
    {
        //TODO cache for order details private readonly IDistributedCache _cache;
        public OrderRepository(OrderDbContext context) : base(context)
        {
        }
        public async Task<Guid> CreateOrderAsync(Order order)
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
                //TODO : Log exception
                throw;
            }
        }
    }
}
