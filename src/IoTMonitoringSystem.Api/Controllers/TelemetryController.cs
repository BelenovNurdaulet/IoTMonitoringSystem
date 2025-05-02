using IoTMonitoringSystem.Api.Models;
using IoTMonitoringSystem.Domain.Interfaces;
using IoTMonitoringSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

[ApiController]
[Route("api/[controller]")]
public class TelemetryController : ControllerBase

{
    private readonly ITelemetryQueue _queue;

    public TelemetryController(ITelemetryQueue queue)
    {
        _queue = queue;
    }

    [HttpPost]
    public IActionResult ReceiveTelemetry([FromBody] TelemetryDto telemetry)
    {
        var message = new TelemetryMessage
        {
            DeviceId = telemetry.DeviceId,
            Timestamp = telemetry.Timestamp,
            Temperature = telemetry.Temperature,
            Humidity = telemetry.Humidity
        };

        _queue.Enqueue(message);

        Log.Information("Данные поставлены в очередь от {DeviceId}", telemetry.DeviceId);

        return Ok(new { message = "Telemetry enqueued" });
    }
}