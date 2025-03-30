using MassTransit;
using OrderManagementService.Background;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

//builder.Services.AddMassTransit(x =>
//{
//    x.SetKebabCaseEndpointNameFormatter();

//    x.UsingRabbitMq((context, cfg) =>
//    {
//        var configuration = context.GetRequiredService<IConfiguration>();

//        cfg.Host(configuration["RabbitMq:EventBusConnection"], h =>
//        {
//            h.Username(configuration["RabbitMQ:Username"]); //TODO add options
//            h.Password(configuration["RabbitMQ:Password"]);

//            // Additional host settings if needed
//            h.Heartbeat(TimeSpan.FromSeconds(60));
//        });

//        // Configure retry policy (optional)
//        cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

//        // Configure endpoints automatically
//        cfg.ConfigureEndpoints(context);

//        // Enable delayed redelivery (optional)
//        cfg.UseDelayedRedelivery(r => r.Interval(5, TimeSpan.FromSeconds(30)));
//    });
//});

//builder.Services.AddHostedService<Worker>();


var host = builder.Build();
host.Run();
