using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Request
{
    public class UpdateEntrepreneurRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? CompanyName { get; set; }
        public string? TaxNumber { get; set; }
        public string? Iban { get; set; }
        public string? Phone { get; set; }
    }
}
