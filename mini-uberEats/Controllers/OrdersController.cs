using Microsoft.AspNetCore.Mvc;
using mini_uberEats.Domain;
using mini_uberEats.Infrastructure;
using ReadModel.Worker;
using MassTransit; 
using Contracts.Events; 

namespace mini_uberEats.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrdersDbContext _db;
    private readonly ReadDbContext _readDb;
    private readonly IPublishEndpoint _publisher; 

    public OrdersController(OrdersDbContext db, ReadDbContext readDb, IPublishEndpoint publisher) // ✅ הוסף פרמטר
    {
        _db = db;
        _readDb = readDb;
        _publisher = publisher; 
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var order = new Order
        {
            CustomerName = request.CustomerName,
            Items = string.Join(",", request.Items),
            TotalAmount = request.TotalAmount,
            Status = "PaymentPending"
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

       
        await _publisher.Publish(new OrderCreated
        {
            OrderId = order.Id,
            CustomerName = order.CustomerName,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt
        });

        return Ok(new { orderId = order.Id });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var view = await _readDb.Orders.FindAsync(id);
        if (view == null) return NotFound();

        return Ok(view);
    }
}

public record CreateOrderRequest(
    string CustomerName,
    List<string> Items,
    decimal TotalAmount
);