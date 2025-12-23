using Microsoft.EntityFrameworkCore;
using mini_uberEats.Infrastructure;
// using ReadModel.Worker; // keep commented — we register via bridge

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Swagger UI (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core + SQLite
builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseSqlite("Data Source=orders.db"));

// Register ReadModel DbContext
builder.Services.AddDbContext<ReadModel.Worker.ReadDbContext>(options =>
    options.UseSqlite("Data Source=readmodel.db"));

var app = builder.Build();

// Ensure DB created (dev)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    db.Database.EnsureCreated();

    var readDb = scope.ServiceProvider.GetService<ReadModel.Worker.ReadDbContext>();
    readDb?.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
