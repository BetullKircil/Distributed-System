using Common.Shared.Events;
using MassTransit;
using NotificationService.Worker.Data;
using NotificationService.Worker.Models;

namespace NotificationService.Worker.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly NotificationDbContext _context;
    private readonly ILogger<OrderCreatedConsumer> _logger;

    public OrderCreatedConsumer(NotificationDbContext context, ILogger<OrderCreatedConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;

        var logMessage =
            $"[BİLDİRİM] Yeni sipariş yakalandı! Sipariş No: {message.OrderId}, Tutar: {message.TotalAmount}";
        _logger.LogInformation(logMessage);

        _context.NotificationLogs.Add(new NotificationLog
        {
            Message = logMessage,
            CreatedDate = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

    }
}