using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Request
{
    internal class RegisterAccountantRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
