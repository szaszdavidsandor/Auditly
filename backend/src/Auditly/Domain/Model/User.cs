using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

public class User
{
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = string.Empty; // 'Entrepreneur', 'Accountant', 'Admin'

    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    // Navigációs tulajdonságok az 1:1 kapcsolathoz
    //public Entrepreneur? Entrepreneur { get; set; }
    //public Accountant? Accountant { get; set; }
}