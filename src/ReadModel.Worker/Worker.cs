namespace ReadModel.Worker;

public class Worker : IHostedService
{
    private readonly IServiceProvider _provider;

    public Worker(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Ensure DB created
        using var scope = _provider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ReadDbContext>();
        await db.Database.EnsureCreatedAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
