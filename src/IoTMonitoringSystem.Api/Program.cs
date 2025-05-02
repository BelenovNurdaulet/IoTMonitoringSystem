using IoTMonitoringSystem.Domain.Interfaces;
using IoTMonitoringSystem.Infrastructure.Queues;
using Serilog;
using IoTMonitoringSystem.Infrastructure.Services;
using IoTMonitoringSystem.Infrastructure.Notifications;
using IoTMonitoringSystem.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITelemetryQueue, TelemetryQueue>();
builder.Services.AddHostedService<TelemetryProcessorService>();
builder.Services.AddSingleton<INotificationService, PushNotificationService>();
builder.Services.AddSingleton<ITelemetryRepository, TelemetryRepository>();
builder.Services.AddSingleton<ResilientTelemetrySaver>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<ResilientTelemetrySaver>());


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