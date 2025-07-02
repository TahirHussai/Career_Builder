using CareerBuilder.API.Models;

namespace CareerBuilder.API.Services.Interface;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
    Task<bool> ValidateTokenAsync(string token);
}
