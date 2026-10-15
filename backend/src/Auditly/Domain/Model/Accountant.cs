using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

public class Accountant
{
   
    public int Id { get; set; }

    // User idegen kulcs
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string CompanyName { get; set; }

    [Required]
    [MaxLength(14)]
    public string RegistrationNumber { get; set; }

    [Required]
    [MaxLength(30)]
    public string Phone { get; set; }




    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
}