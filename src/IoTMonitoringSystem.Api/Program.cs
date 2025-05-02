using IoTMonitoringSystem.Domain.Interfaces;
using IoTMonitoringSystem.Infrastructure.Queues;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITelemetryQueue, TelemetryQueue>();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () =>
{
    Log.Information("Главная страница API вызвана");
    return "IoT Monitoring System API is running!";
});
  

app.Run();