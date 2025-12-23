using System;

namespace ReadModel.Worker;

public class OrderView
{
    public Guid OrderId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime LastEventAt { get; set; }
}
