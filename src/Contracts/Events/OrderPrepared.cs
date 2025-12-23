using System;

namespace Contracts.Events;

public record OrderPrepared
{
    public Guid OrderId { get; init; }
    public DateTime PreparedAt { get; init; }
}
