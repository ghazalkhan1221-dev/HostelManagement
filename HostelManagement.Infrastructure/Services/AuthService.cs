
using HostelManagement.Application.DTOs.Auth;
using HostelManagement.Application.Interfaces;
using HostelManagement.Infrastructure.Services;
using HostelManagement.WebAPI.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace HostelManagement.Infrastructure.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtService _jwtService;

    public AuthService(
        UserManager<IdentityUser> userManager,
        JwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var existingUser =
            await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
        {
            return false;
        }

        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result =
            await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                "; ",
                result.Errors.Select(e => e.Description));

            throw new InvalidOperationException(errors);
        }

        return true;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return null;
        }

        var passwordValid = await _userManager.CheckPasswordAsync(
            user,
            request.Password);

        if (!passwordValid)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        var role = roles.FirstOrDefault() ?? "User";

        var token = _jwtService.GenerateToken(
            user.Id,
            user.Email ?? user.UserName ?? string.Empty,
            role);

        var expiresAtUtc = DateTime.UtcNow.AddMinutes(
            GetJwtExpiryMinutes());

        return new LoginResponse
        {
            Token = token,
            ExpiresAtUtc = expiresAtUtc,
            Email = user.Email ?? string.Empty
        };
    }

    private int GetJwtExpiryMinutes()
    {
        // JWT expiry configuration is read in JwtService.
        // Keep this value aligned with appsettings.json.
        return 60;
    }
}
