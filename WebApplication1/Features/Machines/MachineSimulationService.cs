using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Hubs;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class MachineSimulationService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<MachineHub> _hubContext;
    private readonly Random _random = new();

    public MachineSimulationService(
        IServiceScopeFactory scopeFactory,
        IHubContext<MachineHub> hubContext)
    {
        _scopeFactory = scopeFactory;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            var machines = await db.Machines
                .Where(x => x.IsActive)
                .ToListAsync(stoppingToken);

            foreach (var machine in machines)
            {
                var status = GenerateStatus();

                var result = new ProductionResult
                {
                    Id = Guid.NewGuid(),
                    MachineId = machine.Id,
                    Status = status,
                    ItemsPerMinute = GenerateProduction(status),
                    Temperature = GenerateTemperature(status),
                    OperatorName = GenerateOperator(machine),
                    CreatedAt = DateTime.UtcNow
                };

                db.ProductionResults.Add(result);

                await _hubContext.Clients.All.SendAsync(
                    "ReceiveMachineUpdate",
                    new
                    {
                        Machine = machine.MachineName,
                        result.Status,
                        result.ItemsPerMinute,
                        result.Temperature,
                        result.OperatorName,
                        result.CreatedAt
                    },
                    stoppingToken
                );
            }

            await db.SaveChangesAsync(stoppingToken);

            await Task.Delay(
                TimeSpan.FromSeconds(60),
                stoppingToken
            );
        }
    }

    private string GenerateStatus()
    {
        var statuses = new[]
        {
            "Running",
            "Idle",
            "Maintenance",
            "Error"
        };

        return statuses[_random.Next(statuses.Length)];
    }

    private int GenerateProduction(string status)
    {
        return status == "Running"
            ? _random.Next(10, 40)
            : 0;
    }

    private decimal GenerateTemperature(string status)
    {
        return status switch
        {
            "Running" => _random.Next(70, 95),
            "Idle" => _random.Next(40, 60),
            "Maintenance" => _random.Next(30, 50),
            "Error" => _random.Next(90, 120),
            _ => 0
        };
    }

    private string GenerateOperator(Machine machine)
    {
        return machine.MachineType switch
        {
            "CNC" => "Budi",
            "Milling" => "Andi",
            "Press" => "Siti",
            _ => "Unknown"
        };
    }
}