using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderManagementService.Domain.Enums;
using OrderManagementService.Domain.Events;
using OrderManagementService.Infrastructure;
using System;
using System.Text.Json;

namespace OrderManagementService.API.Services
{
    public class OutboxPublisher : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OutboxPublisher> _logger;

        public OutboxPublisher(IServiceProvider serviceProvider, ILogger<OutboxPublisher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var handler = scope.ServiceProvider.GetRequiredService<OutboxService>();
                        await handler.ProcessOutboxAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Outbox processing error: {ex.Message}");
                }

                // Delay polling for efficiency
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
