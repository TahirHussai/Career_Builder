using CareerBuilder.API.Models;
using CareerBuilder.API.Services.Interface;
using CareerBuilder.API.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerBuilder.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IAuthService authService, ILogger<AdminController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpGet("users")]
    public async Task<ActionResult<List<UserInfo>>> GetUsers()
    {
        var users = await _authService.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("users/{userId}")]
    public async Task<ActionResult<UserInfo>> GetUser(string userId)
    {
        var user = await _authService.GetUserByIdAsync(userId);
        
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost("users")]
    public async Task<ActionResult<AuthResponse>> CreateUser([FromBody] CreateUserRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.CreateUserAsync(request);
        
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPut("users/{userId}")]
    public async Task<ActionResult<AuthResponse>> UpdateUser(string userId, [FromBody] UpdateUserRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.UpdateUserAsync(userId, request);
        
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("users/{userId}")]
    public async Task<ActionResult<AuthResponse>> DeleteUser(string userId)
    {
        var result = await _authService.DeleteUserAsync(userId);
        
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}
