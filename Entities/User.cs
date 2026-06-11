using Microsoft.AspNetCore.Identity;

namespace Revenue_Recognition_System.Entities
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }
}
