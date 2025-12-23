using MassTransit;
using Contracts.Events;

namespace ReadModel.Worker;

public class OrderCreatedReadModelConsumer : IConsumer<OrderCreated>
{
    private readonly ReadDbContext _db;
    private readonly ILogger<OrderCreatedReadModelConsumer> _logger;

    public OrderCreatedReadModelConsumer(ReadDbContext db, ILogger<OrderCreatedReadModelConsumer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        _logger.LogInformation("ReadModel - OrderCreated {OrderId}", context.Message.OrderId);

        var existing = await _db.Orders.FindAsync(context.Message.OrderId);
        if (existing == null)
        {
            var view = new OrderView
            {
                OrderId = context.Message.OrderId,
                CustomerName = context.Message.CustomerName,
                TotalAmount = context.Message.TotalAmount,
                Status = "PaymentPending",
                LastEventAt = context.Message.CreatedAt
            };

            _db.Orders.Add(view);
            await _db.SaveChangesAsync();
        }
    }
}

public class PaymentSucceededReadModelConsumer : IConsumer<PaymentSucceeded>
{
    private readonly ReadDbContext _db;
    private readonly ILogger<PaymentSucceededReadModelConsumer> _logger;

    public PaymentSucceededReadModelConsumer(ReadDbContext db, ILogger<PaymentSucceededReadModelConsumer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PaymentSucceeded> context)
    {
        _logger.LogInformation("ReadModel - PaymentSucceeded {OrderId}", context.Message.OrderId);
        var view = await _db.Orders.FindAsync(context.Message.OrderId);
        if (view != null)
        {
            view.Status = "Paid";
            view.LastEventAt = context.Message.PaidAt;
            await _db.SaveChangesAsync();
        }
    }
}

public class OrderPreparedReadModelConsumer : IConsumer<OrderPrepared>
{
    private readonly ReadDbContext _db;
    private readonly ILogger<OrderPreparedReadModelConsumer> _logger;

    public OrderPreparedReadModelConsumer(ReadDbContext db, ILogger<OrderPreparedReadModelConsumer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderPrepared> context)
    {
        _logger.LogInformation("ReadModel - OrderPrepared {OrderId}", context.Message.OrderId);
        var view = await _db.Orders.FindAsync(context.Message.OrderId);
        if (view != null)
        {
            view.Status = "Prepared";
            view.LastEventAt = context.Message.PreparedAt;
            await _db.SaveChangesAsync();
        }
    }
}

public class OrderCancelledReadModelConsumer : IConsumer<OrderCancelled>
{
    private readonly ReadDbContext _db;
    private readonly ILogger<OrderCancelledReadModelConsumer> _logger;

    public OrderCancelledReadModelConsumer(ReadDbContext db, ILogger<OrderCancelledReadModelConsumer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCancelled> context)
    {
        _logger.LogInformation("ReadModel - OrderCancelled {OrderId}", context.Message.OrderId);
        var view = await _db.Orders.FindAsync(context.Message.OrderId);
        if (view != null)
        {
            view.Status = "Cancelled";
            view.LastEventAt = context.Message.CancelledAt;
            await _db.SaveChangesAsync();
        }
    }
}
