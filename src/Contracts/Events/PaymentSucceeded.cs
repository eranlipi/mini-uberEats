using System;

namespace Contracts.Events;

public record PaymentSucceeded
{
    public Guid OrderId { get; init; }
    public Guid PaymentId { get; init; }
    public DateTime PaidAt { get; init; }
}
