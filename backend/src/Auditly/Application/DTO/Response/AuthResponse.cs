using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Response
{
    internal class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }         
        public string? RefreshToken { get; set; }
    }
}
