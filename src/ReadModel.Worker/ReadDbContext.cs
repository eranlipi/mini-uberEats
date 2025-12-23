using Microsoft.EntityFrameworkCore;

namespace ReadModel.Worker;

public class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }

    public DbSet<OrderView> Orders => Set<OrderView>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // OrderView has a primary key named OrderId; configure it explicitly so EF Core recognizes it.
        modelBuilder.Entity<OrderView>().HasKey(o => o.OrderId);

        base.OnModelCreating(modelBuilder);
    }
}
