using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication1.Models;

public class Machine
{
    public Guid Id { get; set; }

    [Required]
    public string MachineCode { get; set; } = string.Empty;

    [Required]
    public string MachineName { get; set; } = string.Empty;

    [Required]
    public string MachineType { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // [JsonIgnore]
    public List<ProductionResult> ProductionResults { get; set; } = [];
}