using System;
namespace SmartTask.User.Commands.Models
{
    public class JwtTokenDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
