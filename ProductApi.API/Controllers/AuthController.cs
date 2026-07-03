using Microsoft.AspNetCore.Mvc;
using ProductApi.Infrastructure.Identity;

namespace ProductApi.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController(TokenService tokenService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (dto.Username != "admin" || dto.Password != "password")
            return Unauthorized(new { error = "Invalid credentials." });

        var accessToken = tokenService.GenerateAccessToken(dto.Username, "Admin");
        var refreshToken = await tokenService.GenerateRefreshTokenAsync(dto.Username);

        return Ok(new { accessToken, refreshToken, expiresIn = 900 });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshDto dto)
    {
        var stored = await tokenService.ValidateRefreshTokenAsync(dto.RefreshToken);
        if (stored is null) return Unauthorized(new { error = "Invalid or expired refresh token." });

        var newAccess = tokenService.GenerateAccessToken(stored.Username, "Admin");
        var newRefresh = await tokenService.GenerateRefreshTokenAsync(stored.Username);

        return Ok(new { accessToken = newAccess, refreshToken = newRefresh, expiresIn = 900 });
    }
}

public record LoginDto(string Username, string Password);
public record RefreshDto(string RefreshToken);