namespace IoTMonitoringSystem.Api.Models;

public class TelemetryDto
{
    public string DeviceId { get; set; } = default!;
    public DateTime Timestamp { get; set; }
    public float Temperature { get; set; }
    public float Humidity { get; set; }
}