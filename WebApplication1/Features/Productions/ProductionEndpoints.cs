using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Features.ProductionsResults;

public static class ProductionResultEndpoints
{
    public static IEndpointRouteBuilder MapProductionResultEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/production-results");

        group.MapGet("/", GetLogs);

        group.MapPost("/", CreateLog);

        return app;
    }

    private static async Task<IResult> GetLogs(
        AppDbContext db)
    {
        var logs = await db.ProductionResults
            .Include(x => x.Machine)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return Results.Ok(logs);
    }

    private static async Task<IResult> CreateLog(
        ProductionResult req,
        AppDbContext db)
    {
        var machineExists = await db.Machines
            .AnyAsync(x => x.Id == req.MachineId);

        if (!machineExists)
        {
            return Results.BadRequest(new
            {
                message = "Machine not found"
            });
        }

        req.Id = Guid.NewGuid();

        req.CreatedAt = DateTime.UtcNow;

        db.ProductionResults.Add(req);

        await db.SaveChangesAsync();

        return Results.Created(
            $"/production-results/{req.Id}",
            req
        );
    }
}