FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["IoTMonitoringSystem.Api/IoTMonitoringSystem.Api.csproj", "IoTMonitoringSystem.Api/"]
COPY ["IoTMonitoringSystem.Application/IoTMonitoringSystem.Application.csproj", "IoTMonitoringSystem.Application/"]
COPY ["IoTMonitoringSystem.Domain/IoTMonitoringSystem.Domain.csproj", "IoTMonitoringSystem.Domain/"]
COPY ["IoTMonitoringSystem.Infrastructure/IoTMonitoringSystem.Infrastructure.csproj", "IoTMonitoringSystem.Infrastructure/"]

RUN dotnet restore "IoTMonitoringSystem.Api/IoTMonitoringSystem.Api.csproj"
COPY . .
WORKDIR "/src/IoTMonitoringSystem.Api"
RUN dotnet build "IoTMonitoringSystem.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IoTMonitoringSystem.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IoTMonitoringSystem.Api.dll"]
