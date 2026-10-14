using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public enum TokenType
    {
        EmailVerification = 1,
        PasswordReset = 2,
        AccountInvitation = 3
    }

    public class VerificationToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = string.Empty;
        public TokenType Type { get; set; }

        public DateTime ExpiresAt { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsUsed { get; set; } = false;

        // Kapcsolat
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
