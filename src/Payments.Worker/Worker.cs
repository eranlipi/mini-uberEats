namespace Payments.Worker;

public class Worker : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Nothing else to do; MassTransit handles the consumers
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
