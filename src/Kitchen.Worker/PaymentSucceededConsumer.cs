using MassTransit;
using Contracts.Events;

namespace Kitchen.Worker;

public class PaymentSucceededConsumer : IConsumer<PaymentSucceeded>
{
    private readonly IPublishEndpoint _publisher;
    private readonly ILogger<PaymentSucceededConsumer> _logger;

    public PaymentSucceededConsumer(IPublishEndpoint publisher, ILogger<PaymentSucceededConsumer> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PaymentSucceeded> context)
    {
        _logger.LogInformation("Received PaymentSucceeded for {OrderId}", context.Message.OrderId);

        // Simulate preparation
        await Task.Delay(3000);

        var evt = new OrderPrepared
        {
            OrderId = context.Message.OrderId,
            PreparedAt = DateTime.UtcNow
        };

        await _publisher.Publish(evt);
        _logger.LogInformation("Published OrderPrepared for {OrderId}", context.Message.OrderId);
    }
}
