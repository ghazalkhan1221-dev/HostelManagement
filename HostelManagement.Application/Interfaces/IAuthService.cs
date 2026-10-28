
using HostelManagement.Application.DTOs.Auth;

namespace HostelManagement.Application.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequest request);
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}
