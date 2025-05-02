namespace IoTMonitoringSystem.Domain.Interfaces;

public interface INotificationService
{
    Task SendAsync(string deviceId, string message, CancellationToken cancellationToken);
}