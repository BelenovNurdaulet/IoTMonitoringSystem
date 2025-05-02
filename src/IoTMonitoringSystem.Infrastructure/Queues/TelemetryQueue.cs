using System.Collections.Concurrent;
using IoTMonitoringSystem.Domain.Interfaces;
using IoTMonitoringSystem.Domain.Models;

namespace IoTMonitoringSystem.Infrastructure.Queues;

public class TelemetryQueue : ITelemetryQueue
{
    private readonly BlockingCollection<TelemetryMessage> _queue = new();

    public void Enqueue(TelemetryMessage message)
    {
        _queue.Add(message);
    }

    public IEnumerable<TelemetryMessage> GetConsumingEnumerable(CancellationToken cancellationToken)
    {
        return _queue.GetConsumingEnumerable(cancellationToken);
    }
}