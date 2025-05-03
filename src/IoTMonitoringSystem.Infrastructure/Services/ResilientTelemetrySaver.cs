using IoTMonitoringSystem.Domain.Interfaces;
using IoTMonitoringSystem.Domain.Models;
using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace IoTMonitoringSystem.Infrastructure.Services;

public class ResilientTelemetrySaver : BackgroundService
{
    private readonly ITelemetryRepository _repository;
    private readonly ConcurrentQueue<TelemetryMessage> _retryQueue = new();

    public ResilientTelemetrySaver(ITelemetryRepository repository)
    {
        _repository = repository;
    }

    public async Task TrySaveAsync(TelemetryMessage message, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.SaveAsync(message, cancellationToken);
            Log.Information("Данные успешно сохранены в БД. DeviceId: {DeviceId}", message.DeviceId);
        }
        catch (Exception ex)
        {
            message.RetryCount++;

            if (message.RetryCount <= 5)
            {
                Log.Warning(ex, "Ошибка при сохранении в БД. DeviceId: {DeviceId}, попытка {Retry}",
                    message.DeviceId, message.RetryCount);
                _retryQueue.Enqueue(message); 
            }
            else
            {
                Log.Error(ex, "Превышено число попыток сохранения. DeviceId: {DeviceId}, данные утеряны",
                    message.DeviceId);
  
            }
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_retryQueue.TryDequeue(out var failedMessage))
            {
                await TrySaveAsync(failedMessage, stoppingToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // каждые 10 сек
        }
    }
}