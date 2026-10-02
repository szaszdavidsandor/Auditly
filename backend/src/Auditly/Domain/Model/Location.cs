using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

public class Location
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string City { get; set; } = string.Empty;
    [Required]
    public string ZipCode { get; set; } = string.Empty;
    [Required]
    public string Address { get; set; } = string.Empty;

    // Audit mezők
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }



    // Visszamutató navigációs kapcsolat
    public ICollection<Entrepreneur> Entrepreneurs { get; set; } = new List<Entrepreneur>();
}