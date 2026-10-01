using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

public class Accountant
{
    [Key]
    public int Id { get; set; }

    // User idegen kulcs
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string? CompanyName { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? Phone { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
}