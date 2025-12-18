using Microsoft.EntityFrameworkCore;
using NotificationService.Worker.Models;

namespace NotificationService.Worker.Data;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
    {
    }

    public DbSet<NotificationLog> NotificationLogs { get; set; }
}