using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Features.Machines;

public static class MachineEndpoints
{
    public static IEndpointRouteBuilder MapMachineEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/machines");

        group.MapGet("/", GetAllMachines);

        group.MapGet("/{id:guid}", GetMachineById);

        group.MapPost("/", CreateMachine);

        return app;
    }

    private static async Task<IResult> GetAllMachines(
        AppDbContext db)
    {
        var machines = await db.Machines.ToListAsync();

        return Results.Ok(machines);
    }

    private static async Task<IResult> GetMachineById(
        Guid id,
        AppDbContext db)
    {
        var machine = await db.Machines
            .Include(x => x.ProductionResults)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (machine is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(machine);
    }

    private static async Task<IResult> CreateMachine(
        Machine req,
        AppDbContext db)
    {
          var existingMachine = await db.Machines
        .AnyAsync(x => x.MachineCode == req.MachineCode);

        if (existingMachine)
        {
            return Results.BadRequest(new
            {
                message = "Kode mesin sudah ada"
            });
        }
        req.Id = Guid.NewGuid();

        db.Machines.Add(req);

        await db.SaveChangesAsync();

        return Results.Created(
            $"/machines/{req.Id}",
            req
        );
    }
}