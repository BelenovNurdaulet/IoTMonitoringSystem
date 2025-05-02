using IoTMonitoringSystem.Domain.Interfaces;
using Serilog;

namespace IoTMonitoringSystem.Infrastructure.Notifications;

public class PushNotificationService : INotificationService
{
    private const int MaxAttempts = 3;
    private const int RetryDelaySeconds = 5;

    public async Task SendAsync(string deviceId, string message, CancellationToken cancellationToken)
    {
        for (int attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                // Здесь будет реальный HTTP-запрос к push-сервису
                Log.Information("Попытка {Attempt}: Отправка уведомления для {DeviceId}: {Message}", attempt, deviceId, message);

                // Заглушка вместо реального HTTP-вызова
                await SimulatePushSendAsync(deviceId, message, cancellationToken);

                Log.Information("Уведомление успешно отправлено для {DeviceId}", deviceId);
                return;
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Ошибка при отправке уведомления для {DeviceId}, попытка {Attempt}", deviceId, attempt);
                if (attempt < MaxAttempts)
                    await Task.Delay(TimeSpan.FromSeconds(RetryDelaySeconds), cancellationToken);
            }
        }

        Log.Error("Не удалось отправить уведомление после {MaxAttempts} попыток для {DeviceId}", MaxAttempts, deviceId);
    }

    private async Task SimulatePushSendAsync(string deviceId, string message, CancellationToken cancellationToken)
    {
        // Здесь можно симулировать случайную ошибку
        await Task.Delay(500, cancellationToken);
        if (new Random().Next(0, 4) == 0)
        {
            throw new Exception("Симулированная ошибка отправки");
        }
    }
}