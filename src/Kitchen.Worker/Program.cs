using MassTransit;
using Kitchen.Worker;
using Contracts.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<PaymentSucceededConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("kitchen-paymentsucceeded-queue", e =>
                {
                    e.ConfigureConsumer<PaymentSucceededConsumer>(ctx);
                });
            });
        });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
