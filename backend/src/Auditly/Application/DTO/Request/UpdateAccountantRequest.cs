using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTO.Request
{
    public class UpdateAccountantRequest
    {
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? Password { get; set; }

        [MaxLength(200)]
        public string? CompanyName { get; set; }

        [MaxLength(14)]
        public string? RegistrationNumber { get; set; }

        [MaxLength(30)]
        public string? Phone { get; set; }
    }
}