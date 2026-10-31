using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Request
{
    internal class CreateEntrepreneurRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string TaxNumber { get; set; }
        public string Iban { get; set; }
        public string Phone { get; set; }
    }
}
