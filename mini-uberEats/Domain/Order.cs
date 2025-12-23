namespace mini_uberEats.Domain
{

    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CustomerName { get; set; } = string.Empty;
        public string Items { get; set; } = string.Empty; // JSON/string פשוט
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "PaymentPending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
