using CareerBuilder.Models;

namespace CareerBuilder.Services.Interface;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request);
    Task<UserInfo?> GetCurrentUserAsync();
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<string?> GetTokenAsync();
    Task SetTokenAsync(string token);
    Task RemoveTokenAsync();
}
