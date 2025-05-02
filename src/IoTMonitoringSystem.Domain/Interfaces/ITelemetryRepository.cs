using IoTMonitoringSystem.Domain.Models;

namespace IoTMonitoringSystem.Domain.Interfaces;

public interface ITelemetryRepository
{
    Task SaveAsync(TelemetryMessage message, CancellationToken cancellationToken);
}