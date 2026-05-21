using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class User
{
    public Guid Id{get;set;}

    [Required]
    public string Name { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string Role {get; set;} = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;
}