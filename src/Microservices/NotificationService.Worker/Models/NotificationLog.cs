namespace NotificationService.Worker.Models;

public class NotificationLog
{
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedDate { get; set; }
}