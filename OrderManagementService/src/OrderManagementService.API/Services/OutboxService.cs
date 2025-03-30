using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderManagementService.Domain.Enums;
using OrderManagementService.Domain.Events;
using OrderManagementService.Infrastructure;
using System.Text.Json;

namespace OrderManagementService.API.Services
{
    public class OutboxService
    {
        private readonly OrderDbContext _dbContext;
        private readonly IBus _bus;
        private readonly ILogger<OutboxService> _logger;

        public OutboxService(OrderDbContext dbContext, IBus bus, ILogger<OutboxService> logger)
        {
            _dbContext = dbContext;
            _bus = bus;
            _logger = logger;
        }

        public async Task ProcessOutboxAsync(CancellationToken stoppingToken)
        {
            var pendingEvents = await _dbContext.OutboxEvents
                .Where(e => e.Status == Status.Pending)
                .ToListAsync(stoppingToken);

            foreach (var outboxEvent in pendingEvents)
            {
                var orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(outboxEvent.Payload);

                try
                {
                    await _bus.Publish(orderCreatedEvent, stoppingToken);

                    outboxEvent.Status = Status.Processed;
                    outboxEvent.PublishedAt = DateTime.UtcNow;
                    _dbContext.OutboxEvents.Update(outboxEvent);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to publish event {outboxEvent.Id}: {ex.Message}");
                }
            }

            await _dbContext.SaveChangesAsync(stoppingToken);
        }
    }
}
