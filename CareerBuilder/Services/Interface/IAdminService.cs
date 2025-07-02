using CareerBuilder.Models;

namespace CareerBuilder.Services.Interface;

public interface IAdminService
{
    Task<List<UserInfo>> GetUsersAsync();
    Task<UserInfo?> GetUserByIdAsync(string userId);
    Task<AuthResponse> CreateUserAsync(CreateUserRequest request);
    Task<AuthResponse> UpdateUserAsync(string userId, UpdateUserRequest request);
    Task<AuthResponse> DeleteUserAsync(string userId);
}
