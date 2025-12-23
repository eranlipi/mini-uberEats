using MassTransit;
using Contracts.Events;

namespace Payments.Worker;

public class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    private readonly IPublishEndpoint _publisher;
    private readonly ILogger<OrderCreatedConsumer> _logger;
    private static readonly Random _rnd = new();

    public OrderCreatedConsumer(IPublishEndpoint publisher, ILogger<OrderCreatedConsumer> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        _logger.LogInformation("Received OrderCreated for {OrderId}", context.Message.OrderId);

        // Simulate processing time
        await Task.Delay(2000);

        // Random outcome
        var success = _rnd.NextDouble() > 0.2;

        if (success)
        {
            var evt = new PaymentSucceeded
            {
                OrderId = context.Message.OrderId,
                PaymentId = Guid.NewGuid(),
                PaidAt = DateTime.UtcNow
            };

            await _publisher.Publish(evt);
            _logger.LogInformation("Published PaymentSucceeded for {OrderId}", context.Message.OrderId);
        }
        else
        {
            var evt = new PaymentFailed
            {
                OrderId = context.Message.OrderId,
                Reason = "Card declined",
                FailedAt = DateTime.UtcNow
            };

            await _publisher.Publish(evt);
            _logger.LogInformation("Published PaymentFailed for {OrderId}", context.Message.OrderId);
        }
    }
}
