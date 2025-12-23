using MassTransit;
using ReadModel.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<ReadDbContext>(options =>
            options.UseSqlite("Data Source=readmodel.db"));

        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderCreatedReadModelConsumer>();
            x.AddConsumer<PaymentSucceededReadModelConsumer>();
            x.AddConsumer<OrderPreparedReadModelConsumer>();
            x.AddConsumer<OrderCancelledReadModelConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("readmodel-events-queue", e =>
                {
                    e.ConfigureConsumer<OrderCreatedReadModelConsumer>(ctx);
                    e.ConfigureConsumer<PaymentSucceededReadModelConsumer>(ctx);
                    e.ConfigureConsumer<OrderPreparedReadModelConsumer>(ctx);
                    e.ConfigureConsumer<OrderCancelledReadModelConsumer>(ctx);
                });
            });
        });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
