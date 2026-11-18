using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class CreateEntrepreneurRequest
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [MaxLength(13)]
        public string TaxNumber { get; set; }

        [Required]
        [MaxLength(34)]
        public string Iban { get; set; }

        [Required]
        [MaxLength(30)]
        public string Phone { get; set; }
    }
}