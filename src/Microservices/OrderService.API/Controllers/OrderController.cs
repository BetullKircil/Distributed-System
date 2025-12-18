using Common.Shared.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderService.API.Data;
using OrderService.API.Models;

namespace OrderService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly OrderDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderController(OrderDbContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateRequest request)
    {
        if (request.TotalAmount <= 0) return BadRequest("Tutar 0 olamaz.");

        var order = new Order
        {
            CustomerId = request.CustomerId,
            TotalAmount = request.TotalAmount,
            OrderDate = DateTime.UtcNow,
            Status = "Created"
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(); 

        var eventMessage = new OrderCreatedEvent
        {
            OrderId = order.Id, 
            CustomerId = order.CustomerId,
            TotalAmount = order.TotalAmount,
            OrderDate = order.OrderDate
        };

        try
        {
            await _publishEndpoint.Publish(eventMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }


        return Ok(new { OrderId = order.Id, Message = "Sipariş alındı" });
    }

    public class OrderCreateRequest
    {
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}