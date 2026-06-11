namespace Revenue_Recognition_System.Controllers
{
    using global::Revenue_Recognition_System.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;

    namespace Revenue_Recognition_System.Controllers
    {
        [ApiController]
        [Route("api/auth")]
        public class AuthController : ControllerBase
        {
            private readonly IConfiguration _config;
            private readonly UserManager<User> _userManager;

            public AuthController(IConfiguration config, UserManager<User> userManager)
            {
                _config = config;
                _userManager = userManager;
            }

            public record LoginDto(string Username, string Password);
            public record TokensDto(string AccessToken, string RefreshToken);

            [HttpPost("login")]
            public async Task<IActionResult> Login(LoginDto dto)
            {
                var user = await _userManager.FindByNameAsync(dto.Username);

                if (user is null)
                    return Unauthorized("User not found.");

                if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                    return Unauthorized("Wrong password.");

                var tokens = await GenerateTokens(user);

                user.RefreshToken = tokens.RefreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                await _userManager.UpdateAsync(user);

                return Ok(tokens);
            }

            [HttpPost("refresh")]
            public async Task<IActionResult> Refresh([FromBody] string refreshToken)
            {
                var user = _userManager.Users
                    .FirstOrDefault(u => u.RefreshToken == refreshToken);

                if (user is null || user.RefreshTokenExpiry < DateTime.UtcNow)
                    return Unauthorized("Invalid or expired refresh token.");

                var tokens = await GenerateTokens(user);

                user.RefreshToken = tokens.RefreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                await _userManager.UpdateAsync(user);

                return Ok(tokens);
            }

            private async Task<TokensDto> GenerateTokens(User user)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Name, user.UserName!),
                };

                foreach (var role in roles)
                    claims.Add(new(ClaimTypes.Role, role));

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var jwt = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(15),
                    signingCredentials: creds);

                var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
                var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

                return new TokensDto(accessToken, refreshToken);
            }
        }
    }
}
