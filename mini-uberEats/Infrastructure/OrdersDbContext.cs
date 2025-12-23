using Microsoft.EntityFrameworkCore;
using mini_uberEats.Domain;

namespace mini_uberEats.Infrastructure;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
}
