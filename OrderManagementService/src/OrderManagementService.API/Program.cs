using Common.Middlewares;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using OrderManagementService.API.Extensions;
using OrderManagementService.API.Services;
using OrderManagementService.Application.Extensions;
using OrderManagementService.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

builder.Services.AddCorsApp();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddApplication();
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<OrderCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var configuration = context.GetRequiredService<IConfiguration>();

        cfg.Host(configuration["RabbitMQ:EventBusConnection"], h =>
        {
            h.Username(configuration["RabbitMQ:Username"]);
            h.Password(configuration["RabbitMQ:Password"]);           
        });

        cfg.ReceiveEndpoint("order-queue", e =>
        {
            e.ConfigureConsumer<OrderCreatedConsumer>(context);
        });

        cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

        cfg.ConfigureEndpoints(context);

        cfg.UseDelayedRedelivery(r => r.Interval(5, TimeSpan.FromSeconds(30)));
    });
});

builder.Services.AddHostedService<OutboxPublisher>();
builder.Services.AddScoped<OutboxService>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrations();
    app.UseCors("AllowAll");
}
else
    app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
