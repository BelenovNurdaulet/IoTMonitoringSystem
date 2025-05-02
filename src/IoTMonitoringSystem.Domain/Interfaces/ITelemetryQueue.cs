using IoTMonitoringSystem.Domain.Models;

namespace IoTMonitoringSystem.Domain.Interfaces;

public interface ITelemetryQueue
{
    void Enqueue(TelemetryMessage message);
    IEnumerable<TelemetryMessage> GetConsumingEnumerable(CancellationToken cancellationToken);
}