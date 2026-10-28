
using HostelManagement.Application.DTOs.Auth;
using HostelManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HostelManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // Register
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new
            {
                Message = "Full name, email and password are required."
            });
        }

        var registered = await _authService.RegisterAsync(request);

        if (!registered)
        {
            return Conflict(new
            {
                Message = "Registration failed. The email may already be registered, or the password may not meet the requirements."
            });
        }

        return Ok(new
        {
            Message = "Registration successful."
        });
    }

    // Login
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new
            {
                Message = "Email and password are required."
            });
        }

        var result = await _authService.LoginAsync(request);

        if (result == null)
        {
            return Unauthorized(new
            {
                Message = "Invalid email or password."
            });
        }

        return Ok(result);
    }

    [Authorize(AuthenticationSchemes =
       JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            UserId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,

            Email = User.Identity?.Name
        });
    }
}
