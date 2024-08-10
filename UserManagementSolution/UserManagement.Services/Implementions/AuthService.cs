using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Data;
using UserManagement.Models.DTOs;
using UserManagement.Models.Entities;
using UserManagement.Services.Contracts;

namespace UserManagement.Services.Implementions;

public class AuthService : IAuthService
{
    private readonly DapperContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(DapperContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CurrentUserModel> GetCurrentUser()
    {

        return new CurrentUserModel()
        {
            Id = long.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
            UserName = _httpContextAccessor.HttpContext.User.Identity.Name
        };

    }

    // اینجا اعتبارسنجی کاربر انجام می‌شود و توکن JWT تولید می‌شود
    public async Task<string> Authenticate(LoginModel user)
    {
        User User = new();
        var query = $"SELECT * FROM Users WHERE UserName = @UserName";
        using (var connection = _context.CreateConnection())
        {
            User = await connection.QueryFirstAsync<User>(query, new { UserName = user.Username });
        }

        if (user != null)
        {
            bool verified = BCrypt.Net.BCrypt.Verify(user.Password, User.PasswordHash);
            if (verified)
            {
                // تولید توکن JWT
                var token = GenerateJwtToken(User);
                return token;
            }
        }

        return null;
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
        };
        var jwtToken = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(GlobalParameters.jwtKey)
                ),
                SecurityAlgorithms.HmacSha256Signature)
        );
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}