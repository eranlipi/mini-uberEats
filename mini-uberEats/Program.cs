using Microsoft.EntityFrameworkCore;
using mini_uberEats.Infrastructure;
using ReadModel.Worker;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database contexts
builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseSqlite("Data Source=orders.db"));

builder.Services.AddDbContext<ReadDbContext>(options =>
    options.UseSqlite("Data Source=readmodel.db"));


builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

var app = builder.Build();

// Ensure databases are created
using (var scope = app.Services.CreateScope())
{
    var ordersDb = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    var readDb = scope.ServiceProvider.GetRequiredService<ReadDbContext>();
    
    await ordersDb.Database.EnsureCreatedAsync();
    await readDb.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();