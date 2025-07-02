using CareerBuilder.Models;
using CareerBuilder.Services.Interface;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CareerBuilder.Services.Implementations;

public class AdminService : IAdminService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;
    private readonly ILogger<AdminService> _logger;
    private const string ApiBaseUrl = "http://localhost:5047/api";

    public AdminService(HttpClient httpClient, IAuthService authService, ILogger<AdminService> logger)
    {
        _httpClient = httpClient;
        _authService = authService;
        _logger = logger;
    }

    private async Task SetAuthorizationHeaderAsync()
    {
        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<UserInfo>> GetUsersAsync()
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/admin/users");
            
            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<UserInfo>>();
                return users ?? new List<UserInfo>();
            }
            
            return new List<UserInfo>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting users");
            return new List<UserInfo>();
        }
    }

    public async Task<UserInfo?> GetUserByIdAsync(string userId)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/admin/users/{userId}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserInfo>();
            }
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by ID");
            return null;
        }
    }

    public async Task<AuthResponse> CreateUserAsync(CreateUserRequest request)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/admin/users", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return result ?? new AuthResponse { Success = false, Message = "Unknown error occurred" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return new AuthResponse { Success = false, Message = "An error occurred while creating the user" };
        }
    }

    public async Task<AuthResponse> UpdateUserAsync(string userId, UpdateUserRequest request)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/admin/users/{userId}", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return result ?? new AuthResponse { Success = false, Message = "Unknown error occurred" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user");
            return new AuthResponse { Success = false, Message = "An error occurred while updating the user" };
        }
    }

    public async Task<AuthResponse> DeleteUserAsync(string userId)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/admin/users/{userId}");
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return result ?? new AuthResponse { Success = false, Message = "Unknown error occurred" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user");
            return new AuthResponse { Success = false, Message = "An error occurred while deleting the user" };
        }
    }
}
