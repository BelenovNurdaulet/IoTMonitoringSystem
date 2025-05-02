
using Microsoft.Extensions.Hosting;
using IoTMonitoringSystem.Domain.Interfaces;
using IoTMonitoringSystem.Domain.Models;
using Serilog;


namespace IoTMonitoringSystem.Infrastructure.Services;

public class TelemetryProcessorService : BackgroundService
{
    private readonly ITelemetryQueue _queue;
    private readonly INotificationService _notifier;
    private readonly ResilientTelemetrySaver _saver;

    
    public TelemetryProcessorService(ITelemetryQueue queue, INotificationService notifier, ResilientTelemetrySaver saver)
    {
        _queue = queue;
        _notifier = notifier;
        _saver = saver;
    }


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            foreach (var message in _queue.GetConsumingEnumerable(stoppingToken))
            {
                try
                {
                    ProcessMessage(message);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Ошибка при обработке сообщения от {DeviceId}", message.DeviceId);
                }
            }
        }, stoppingToken);
    }

    private async Task ProcessMessage(TelemetryMessage message)
    {
        Log.Information("Обрабатывается сообщение от {DeviceId}", message.DeviceId);
        if (message.Temperature > 100)
        {
            Log.Warning("Земля тебе пухом! Устройство: {DeviceId}, Температура: {Temp}", message.DeviceId, message.Temperature);
            _notifier.SendAsync(message.DeviceId, $"Высокая температура: {message.Temperature}", CancellationToken.None).Wait();
        }
        if (message.Temperature > 30)
        {
            Log.Warning("Температура превышена! Устройство: {DeviceId}, Температура: {Temp}", message.DeviceId, message.Temperature);
            _notifier.SendAsync(message.DeviceId, $"Высокая температура: {message.Temperature}", CancellationToken.None).Wait();
        }
        
        if (message.Temperature < 18)
        {
            Log.Warning("Температура слишком низкая! Устройство: {DeviceId}, Температура: {Temp}", message.DeviceId, message.Temperature);
            _notifier.SendAsync(message.DeviceId, $"Низкая температура: {message.Temperature}", CancellationToken.None).Wait();
        }

        if (message.Humidity < 20)
        {
            Log.Warning("Влажность слишком низкая! Устройство: {DeviceId}, Влажность: {Humidity}", message.DeviceId, message.Humidity);
            _notifier.SendAsync(message.DeviceId, $"Низкая влажность: {message.Humidity}", CancellationToken.None).Wait();
        }
        
        if (message.Humidity > 60)
        {
            Log.Warning("Ты нафига датчик в бане поставил! Устройство: {DeviceId}, Влажность: {Humidity}", message.DeviceId, message.Humidity);
            _notifier.SendAsync(message.DeviceId, $"Высокая влажность: {message.Humidity}", CancellationToken.None).Wait();
        }
        await _saver.TrySaveAsync(message, CancellationToken.None);

    }

}