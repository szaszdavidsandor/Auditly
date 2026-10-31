using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Request
{
    internal class LoginRequest
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
    }
}
