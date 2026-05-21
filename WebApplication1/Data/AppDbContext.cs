using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Machine> Machines => Set<Machine>();

    public DbSet<ProductionResult> ProductionResults => Set<ProductionResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Machine>()
            .HasIndex(x => x.MachineCode)
            .IsUnique();

        modelBuilder.Entity<ProductionResult>()
            .HasOne(x => x.Machine)
            .WithMany(x => x.ProductionResults)
            .HasForeignKey(x => x.MachineId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}