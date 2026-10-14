using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Request
{
    internal class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
