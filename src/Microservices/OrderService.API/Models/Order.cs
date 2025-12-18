namespace OrderService.API.Models;

public class Order
{
    public Guid Id { get; set; }
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public string? Status { get; set; }
}