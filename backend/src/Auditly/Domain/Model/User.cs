using Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public Role Role { get; set; }  // 'Entrepreneur', 'Accountant', 'Admin'

    public int? LocationId { get; set; }
    public Location? Location { get; set; }

    public int? SpecializationId { get; set; }
    public Specialization? Specialization { get; set; }


    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }



    // Navigációs tulajdonságok az 1:1 kapcsolathoz
    public Entrepreneur? Entrepreneur { get; set; }
    public Accountant? Accountant { get; set; }
}