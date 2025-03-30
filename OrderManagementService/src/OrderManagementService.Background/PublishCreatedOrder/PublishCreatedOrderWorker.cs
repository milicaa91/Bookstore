using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderManagementService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderManagementService.Background.PublishCreatedOrder
{
    public class PublishCreatedOrderWorker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly IOrderRepository _orderRepository;
        public PublishCreatedOrderWorker(IBus bus, IOrderRepository orderRepository)
        {
            _bus = bus;
            _orderRepository = orderRepository;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                //// Step 1: Get pending outbox events
                //var pendingEvents = await _context.OutboxEvents
                //    .Where(e => e.Status == "Pending")
                //    .ToListAsync(stoppingToken);

                //foreach (var outboxEvent in pendingEvents)
                //{
                //    try
                //    {
                //        // Step 2: Deserialize and publish event to RabbitMQ
                //        var orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(outboxEvent.Payload);
                //        await _bus.Publish(orderCreatedEvent, stoppingToken);

                //        // Step 3: Mark the event as Published
                //        outboxEvent.Status = "Published";
                //        outboxEvent.PublishedAt = DateTime.UtcNow;
                //        _context.OutboxEvents.Update(outboxEvent);
                //        await _context.SaveChangesAsync(stoppingToken);

                //        Console.WriteLine($"Event published: {outboxEvent.Id}");
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine($"Error publishing event {outboxEvent.Id}: {ex.Message}");
                //        outboxEvent.Status = "Failed";
                //        _context.OutboxEvents.Update(outboxEvent);
                //        await _context.SaveChangesAsync(stoppingToken);
                //    }
                }

                // Step 4: Delay to avoid constant polling
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            //}
        }
    }
}
