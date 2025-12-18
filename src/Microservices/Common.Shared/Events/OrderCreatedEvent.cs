namespace Common.Shared.Events;

public class OrderCreatedEvent
{
    public Guid OrderId { get; set; }
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}