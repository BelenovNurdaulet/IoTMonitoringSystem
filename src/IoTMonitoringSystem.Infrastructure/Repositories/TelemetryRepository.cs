using IoTMonitoringSystem.Domain.Interfaces;
using IoTMonitoringSystem.Domain.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Serilog;

namespace IoTMonitoringSystem.Infrastructure.Repositories;

public class TelemetryRepository : ITelemetryRepository
{
    private readonly string _connectionString;

    public TelemetryRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres")!;
    }

    public async Task SaveAsync(TelemetryMessage message, CancellationToken cancellationToken)
    {
        const string sql = @"INSERT INTO telemetry (device_id, timestamp, temperature, humidity)
                             VALUES (@device_id, @timestamp, @temperature, @humidity);";

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync(cancellationToken);

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("device_id", message.DeviceId);
        cmd.Parameters.AddWithValue("timestamp", message.Timestamp);
        cmd.Parameters.AddWithValue("temperature", message.Temperature);
        cmd.Parameters.AddWithValue("humidity", message.Humidity);

        await cmd.ExecuteNonQueryAsync(cancellationToken);

        Log.Information("Сохранены данные в БД от {DeviceId}", message.DeviceId);
    }
}