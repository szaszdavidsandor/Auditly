using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

public class Specialization
{
    
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Description { get; set; }



    // Audit mezők
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }



    // Visszamutató navigációs kapcsolat
    public ICollection<Entrepreneur> Entrepreneurs { get; set; } = new List<Entrepreneur>();
}