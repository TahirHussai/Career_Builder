using CareerBuilder.API.Models;

namespace CareerBuilder.API.Services.Interface;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request);
    Task<AuthResponse> CreateUserAsync(CreateUserRequest request);
    Task<AuthResponse> UpdateUserAsync(string userId, UpdateUserRequest request);
    Task<AuthResponse> DeleteUserAsync(string userId);
    Task<List<UserInfo>> GetUsersAsync();
    Task<UserInfo?> GetUserByIdAsync(string userId);
}
