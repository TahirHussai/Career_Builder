using CareerBuilder.API.Models;
using CareerBuilder.API.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CareerBuilder.API.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ITokenService tokenService,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "User with this email already exists."
                };
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = true // For demo purposes, skip email confirmation
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            // Assign default role
            await _userManager.AddToRoleAsync(user, "JobSeeker");

            var token = await _tokenService.GenerateTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponse
            {
                Success = true,
                Message = "Registration successful.",
                Token = token,
                TokenExpiration = DateTime.UtcNow.AddHours(24),
                User = new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = roles.ToList()
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred during registration."
            };
        }
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !user.IsActive)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = result.IsLockedOut ? "Account is locked out." : "Invalid email or password."
                };
            }

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var token = await _tokenService.GenerateTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponse
            {
                Success = true,
                Message = "Login successful.",
                Token = token,
                TokenExpiration = DateTime.UtcNow.AddHours(24),
                User = new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = roles.ToList()
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user login");
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred during login."
            };
        }
    }

    public async Task<AuthResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal that the user doesn't exist
                return new AuthResponse
                {
                    Success = true,
                    Message = "If an account with that email exists, a password reset link has been sent."
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            // TODO: Send email with reset token
            // For now, we'll just log it (in production, implement email service)
            _logger.LogInformation($"Password reset token for {user.Email}: {token}");

            return new AuthResponse
            {
                Success = true,
                Message = "If an account with that email exists, a password reset link has been sent."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during forgot password");
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred while processing your request."
            };
        }
    }

    public async Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid reset token."
                };
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            return new AuthResponse
            {
                Success = true,
                Message = "Password has been reset successfully."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset");
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred while resetting your password."
            };
        }
    }

    public async Task<AuthResponse> CreateUserAsync(CreateUserRequest request)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "User with this email already exists."
                };
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            // Assign roles
            foreach (var role in request.Roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            return new AuthResponse
            {
                Success = true,
                Message = "User created successfully."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred while creating the user."
            };
        }
    }

    public async Task<AuthResponse> UpdateUserAsync(string userId, UpdateUserRequest request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            user.Email = request.Email;
            user.UserName = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.IsActive = request.IsActive;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            // Update roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            foreach (var role in request.Roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            return new AuthResponse
            {
                Success = true,
                Message = "User updated successfully."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user");
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred while updating the user."
            };
        }
    }

    public async Task<AuthResponse> DeleteUserAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            return new AuthResponse
            {
                Success = true,
                Message = "User deleted successfully."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user");
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred while deleting the user."
            };
        }
    }

    public async Task<List<UserInfo>> GetUsersAsync()
    {
        try
        {
            var users = await _userManager.Users.ToListAsync();
            var userInfos = new List<UserInfo>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userInfos.Add(new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = roles.ToList()
                });
            }

            return userInfos;
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
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);
            return new UserInfo
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user by ID");
            return null;
        }
    }
}
