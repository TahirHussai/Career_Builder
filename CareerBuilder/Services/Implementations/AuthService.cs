using CareerBuilder.Models;
using CareerBuilder.Services.Interface;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace CareerBuilder.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<AuthService> _logger;
    private const string TokenKey = "authToken";
    private const string ApiBaseUrl = "http://localhost:5047/api";

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime, ILogger<AuthService> logger)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
        _logger = logger;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/auth/register", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            
            if (result?.Success == true && !string.IsNullOrEmpty(result.Token))
            {
                await SetTokenAsync(result.Token);
                await SetAuthorizationHeader(result.Token);
            }
            
            return result ?? new AuthResponse { Success = false, Message = "Unknown error occurred" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            return new AuthResponse { Success = false, Message = "An error occurred during registration" };
        }
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/auth/login", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            
            if (result?.Success == true && !string.IsNullOrEmpty(result.Token))
            {
                await SetTokenAsync(result.Token);
                await SetAuthorizationHeader(result.Token);
            }
            
            return result ?? new AuthResponse { Success = false, Message = "Unknown error occurred" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return new AuthResponse { Success = false, Message = "An error occurred during login" };
        }
    }

    public async Task<AuthResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/auth/forgot-password", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return result ?? new AuthResponse { Success = false, Message = "Unknown error occurred" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during forgot password");
            return new AuthResponse { Success = false, Message = "An error occurred while processing your request" };
        }
    }

    public async Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/auth/reset-password", request);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return result ?? new AuthResponse { Success = false, Message = "Unknown error occurred" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset");
            return new AuthResponse { Success = false, Message = "An error occurred while resetting your password" };
        }
    }

    public async Task<UserInfo?> GetCurrentUserAsync()
    {
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return null;

            await SetAuthorizationHeader(token);
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/auth/me");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserInfo>();
            }
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user");
            return null;
        }
    }

    public async Task LogoutAsync()
    {
        await RemoveTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting token from localStorage");
            return null;
        }
    }

    public async Task SetTokenAsync(string token)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting token in localStorage");
        }
    }

    public async Task RemoveTokenAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing token from localStorage");
        }
    }

    private async Task SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
