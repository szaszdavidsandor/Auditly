using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Request
{
    internal class CreateAccountantRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string RegistrationNumber { get; set; }
        public string Phone { get; set; }
    }
}
