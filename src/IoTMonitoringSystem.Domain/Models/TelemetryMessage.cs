namespace IoTMonitoringSystem.Domain.Models;

public class TelemetryMessage
{
    public string DeviceId { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public float Temperature { get; set; }
    public float Humidity { get; set; }
  
    public int RetryCount { get; set; } = 0;
}