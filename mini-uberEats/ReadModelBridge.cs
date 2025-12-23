using Microsoft.EntityFrameworkCore;
using ReadModel.Worker;

namespace mini_uberEats.Infrastructure;

public static class ReadModelBridge
{
    public static void AddReadModel(this IServiceCollection services)
    {
        services.AddDbContext<ReadDbContext>(options =>
            options.UseSqlite("Data Source=readmodel.db"));
    }
}
