using CareerBuilder.API.Data;
using CareerBuilder.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerBuilder.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<JobController> _logger;

    public JobController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<JobController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<JobPostingDto>>> GetJobs()
    {
        try
        {
            var jobs = await _context.JobPostings
                .Include(j => j.User)
                .Where(j => j.IsActive)
                .OrderByDescending(j => j.CreatedAt)
                .Select(j => new JobPostingDto
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    CompanyName = j.CompanyName,
                    SalaryMin = j.SalaryMin,
                    SalaryMax = j.SalaryMax,
                    CreatedAt = j.CreatedAt,
                    ExpiresAt = j.ExpiresAt,
                    IsActive = j.IsActive,
                    PostedByName = $"{j.User.FirstName} {j.User.LastName}",
                    PostedById = j.UserId
                })
                .ToListAsync();

            return Ok(jobs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving jobs");
            return StatusCode(500, "An error occurred while retrieving jobs");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobPostingDto>> GetJob(int id)
    {
        try
        {
            var job = await _context.JobPostings
                .Include(j => j.User)
                .Where(j => j.Id == id && j.IsActive)
                .Select(j => new JobPostingDto
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    CompanyName = j.CompanyName,
                    SalaryMin = j.SalaryMin,
                    SalaryMax = j.SalaryMax,
                    CreatedAt = j.CreatedAt,
                    ExpiresAt = j.ExpiresAt,
                    IsActive = j.IsActive,
                    PostedByName = $"{j.User.FirstName} {j.User.LastName}",
                    PostedById = j.UserId
                })
                .FirstOrDefaultAsync();

            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving job with ID {JobId}", id);
            return StatusCode(500, "An error occurred while retrieving the job");
        }
    }

    [HttpGet("my-jobs")]
    [Authorize]
    public async Task<ActionResult<List<JobPostingDto>>> GetMyJobs()
    {
        try
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var jobs = await _context.JobPostings
                .Include(j => j.User)
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.CreatedAt)
                .Select(j => new JobPostingDto
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    CompanyName = j.CompanyName,
                    SalaryMin = j.SalaryMin,
                    SalaryMax = j.SalaryMax,
                    CreatedAt = j.CreatedAt,
                    ExpiresAt = j.ExpiresAt,
                    IsActive = j.IsActive,
                    PostedByName = $"{j.User.FirstName} {j.User.LastName}",
                    PostedById = j.UserId
                })
                .ToListAsync();

            return Ok(jobs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user jobs");
            return StatusCode(500, "An error occurred while retrieving your jobs");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Employer")]
    public async Task<ActionResult<JobPostingDto>> CreateJob([FromBody] CreateJobPostingRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized();
            }

            var jobPosting = new JobPosting
            {
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                CompanyName = request.CompanyName,
                SalaryMin = request.SalaryMin,
                SalaryMax = request.SalaryMax,
                ExpiresAt = request.ExpiresAt,
                IsActive = true,
                UserId = userId
            };

            _context.JobPostings.Add(jobPosting);
            await _context.SaveChangesAsync();

            var jobDto = new JobPostingDto
            {
                Id = jobPosting.Id,
                Title = jobPosting.Title,
                Description = jobPosting.Description,
                Location = jobPosting.Location,
                CompanyName = jobPosting.CompanyName,
                SalaryMin = jobPosting.SalaryMin,
                SalaryMax = jobPosting.SalaryMax,
                CreatedAt = jobPosting.CreatedAt,
                ExpiresAt = jobPosting.ExpiresAt,
                IsActive = jobPosting.IsActive,
                PostedByName = $"{user.FirstName} {user.LastName}",
                PostedById = userId
            };

            return CreatedAtAction(nameof(GetJob), new { id = jobPosting.Id }, jobDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating job");
            return StatusCode(500, "An error occurred while creating the job");
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Employer")]
    public async Task<ActionResult<JobPostingDto>> UpdateJob(int id, [FromBody] UpdateJobPostingRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var jobPosting = await _context.JobPostings
                .Include(j => j.User)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jobPosting == null)
            {
                return NotFound();
            }

            // Check if user owns this job or is admin
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && jobPosting.UserId != userId)
            {
                return Forbid();
            }

            jobPosting.Title = request.Title;
            jobPosting.Description = request.Description;
            jobPosting.Location = request.Location;
            jobPosting.CompanyName = request.CompanyName;
            jobPosting.SalaryMin = request.SalaryMin;
            jobPosting.SalaryMax = request.SalaryMax;
            jobPosting.ExpiresAt = request.ExpiresAt;
            jobPosting.IsActive = request.IsActive;

            await _context.SaveChangesAsync();

            var jobDto = new JobPostingDto
            {
                Id = jobPosting.Id,
                Title = jobPosting.Title,
                Description = jobPosting.Description,
                Location = jobPosting.Location,
                CompanyName = jobPosting.CompanyName,
                SalaryMin = jobPosting.SalaryMin,
                SalaryMax = jobPosting.SalaryMax,
                CreatedAt = jobPosting.CreatedAt,
                ExpiresAt = jobPosting.ExpiresAt,
                IsActive = jobPosting.IsActive,
                PostedByName = $"{jobPosting.User.FirstName} {jobPosting.User.LastName}",
                PostedById = jobPosting.UserId
            };

            return Ok(jobDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating job with ID {JobId}", id);
            return StatusCode(500, "An error occurred while updating the job");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Employer")]
    public async Task<ActionResult> DeleteJob(int id)
    {
        try
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var jobPosting = await _context.JobPostings.FirstOrDefaultAsync(j => j.Id == id);

            if (jobPosting == null)
            {
                return NotFound();
            }

            // Check if user owns this job or is admin
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && jobPosting.UserId != userId)
            {
                return Forbid();
            }

            // Soft delete by setting IsActive to false
            jobPosting.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting job with ID {JobId}", id);
            return StatusCode(500, "An error occurred while deleting the job");
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<JobPostingDto>>> SearchJobs(
        [FromQuery] string? searchTerm,
        [FromQuery] string? location,
        [FromQuery] string? category)
    {
        try
        {
            var query = _context.JobPostings
                .Include(j => j.User)
                .Where(j => j.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(j =>
                    j.Title.Contains(searchTerm) ||
                    j.Description.Contains(searchTerm) ||
                    j.CompanyName.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(j => j.Location.Contains(location));
            }

            var jobs = await query
                .OrderByDescending(j => j.CreatedAt)
                .Select(j => new JobPostingDto
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    CompanyName = j.CompanyName,
                    SalaryMin = j.SalaryMin,
                    SalaryMax = j.SalaryMax,
                    CreatedAt = j.CreatedAt,
                    ExpiresAt = j.ExpiresAt,
                    IsActive = j.IsActive,
                    PostedByName = $"{j.User.FirstName} {j.User.LastName}",
                    PostedById = j.UserId
                })
                .ToListAsync();

            return Ok(jobs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching jobs");
            return StatusCode(500, "An error occurred while searching jobs");
        }
    }
}

// DTOs for API responses
public class JobPostingDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public string PostedByName { get; set; } = string.Empty;
    public string PostedById { get; set; } = string.Empty;
}

public class CreateJobPostingRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

public class UpdateJobPostingRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; } = true;
} 