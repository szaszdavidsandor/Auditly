using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model;


public class Entrepreneur
{
    
    public int Id { get; set; }

    // User idegen kulcs
    public int UserId { get; set; }
    public User User { get; set; } = null!;


    [Required]
    [MaxLength(30)]
    public string Phone { get; set; }



    [Required]
    [MaxLength(100)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [MaxLength(13)]
    public string TaxNumber { get; set; } = string.Empty;

    [Required]
    
    public string Iban { get; set; } = string.Empty;



    public int? LocationId { get; set; }
    public Location? Location { get; set; }


    public int? SpecializationId { get; set; }
    public Specialization? Specialization { get; set; }



    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
}