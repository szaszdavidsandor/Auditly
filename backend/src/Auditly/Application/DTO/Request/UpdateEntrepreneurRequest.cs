using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTO.Request
{
    public class UpdateEntrepreneurRequest
    {
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? Password { get; set; }

        [MaxLength(100)]
        public string? CompanyName { get; set; }

        [MaxLength(13)]
        public string? TaxNumber { get; set; }

        [MaxLength(34)]
        public string? Iban { get; set; }

        [MaxLength(30)]
        public string? Phone { get; set; }
    }
}