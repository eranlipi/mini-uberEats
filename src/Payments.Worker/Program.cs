using MassTransit;
using Payments.Worker;
using Contracts.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderCreatedConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("payments-ordercreated-queue", e =>
                {
                    e.ConfigureConsumer<OrderCreatedConsumer>(ctx);
                });
            });
        });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
