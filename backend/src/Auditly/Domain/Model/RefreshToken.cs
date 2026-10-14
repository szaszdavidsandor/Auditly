using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    internal class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        //Revocation
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedByIp { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByToken { get; set; } 

        public bool IsActive => RevokedAt == null && !IsExpired;

        
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
}
