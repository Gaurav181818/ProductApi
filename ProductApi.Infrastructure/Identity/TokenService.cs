using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;

namespace ProductApi.Infrastructure.Identity;

public class TokenService(IConfiguration config, ApplicationDbContext context)
{
    public string GenerateAccessToken(string username, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(string username)
    {
        // Revoke any existing tokens for this user
        var existing = context.RefreshTokens
            .Where(r => r.Username == username && !r.IsRevoked);
        foreach (var t in existing) t.IsRevoked = true;

        var token = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Username = username,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };

        context.RefreshTokens.Add(token);
        await context.SaveChangesAsync();
        return token.Token;
    }

    public async Task<RefreshToken?> ValidateRefreshTokenAsync(string token)
    {
        var stored = context.RefreshTokens
            .FirstOrDefault(r => r.Token == token && !r.IsRevoked);

        if (stored is null || stored.ExpiresAt < DateTime.UtcNow) return null;
        return stored;
    }
}