using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication1.Models;

public class ProductionResult
{
    public Guid Id { get; set; }

    [Required]
    public Guid MachineId { get; set; }

    [JsonIgnore]
    [ForeignKey(nameof(MachineId))]
    public Machine Machine { get; set; } = null!;

    [Required]
    public string Status { get; set; } = string.Empty;

    public int ItemsPerMinute { get; set; }

    public decimal Temperature { get; set; }

    public string OperatorName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}