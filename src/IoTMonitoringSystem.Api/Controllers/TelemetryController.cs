using IoTMonitoringSystem.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace IoTMonitoringSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TelemetryController : ControllerBase
{
    [HttpPost]
    public IActionResult ReceiveTelemetry([FromBody] TelemetryDto telemetry)
    {
        Log.Information("Получены данные от устройства {DeviceId}: температура={Temp}, влажность={Humidity} в {Time}",
            telemetry.DeviceId, telemetry.Temperature, telemetry.Humidity, telemetry.Timestamp);

    
        return Ok(new { message = "Telemetry received" });
    }
}