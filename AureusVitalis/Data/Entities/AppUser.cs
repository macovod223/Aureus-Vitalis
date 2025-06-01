using System;

namespace AureusVitalis.Data.Entities
{
    public class AppUser
    {
        public int    Id           { get; set; }
        public string Email        { get; set; } = string.Empty;
        public string Username     { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Gender       { get; set; } = string.Empty;
        public DateTime CreatedAt  { get; set; } = DateTime.UtcNow;
    }
}