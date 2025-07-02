using CareerBuilder.API.Data;
using CareerBuilder.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerBuilder.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<JobsController> _logger;

    public JobsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<JobsController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> GetJobs()
    {
        try
        {
            var jobs = await _context.JobPostings
                .Include(j => j.User)
                .Where(j => j.IsActive)
                .OrderByDescending(j => j.CreatedAt)
                .Select(j => new
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    Company = j.CompanyName,
                    JobType = j.JobType,
                    Category = j.Category,
                    SalaryRange = j.SalaryMin.HasValue && j.SalaryMax.HasValue ? $"${j.SalaryMin:N0} - ${j.SalaryMax:N0}" : "",
                    Requirements = j.Requirements,
                    Benefits = j.Benefits,
                    PostedDate = j.CreatedAt,
                    ExpiryDate = j.ExpiresAt,
                    IsActive = j.IsActive,
                    PostedById = j.UserId,
                    PostedByName = $"{j.User.FirstName} {j.User.LastName}",
                    ApplicationCount = 0
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

    [HttpPost]
    [Authorize(Roles = "Admin,Employer")]
    public async Task<ActionResult> CreateJob([FromBody] CreateJobRequest request)
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

            // Parse salary range to min/max values
            decimal? salaryMin = null, salaryMax = null;
            if (!string.IsNullOrEmpty(request.SalaryRange))
            {
                var salaryParts = request.SalaryRange.Replace("$", "").Replace(",", "").Split('-', 2);
                if (salaryParts.Length == 2)
                {
                    if (decimal.TryParse(salaryParts[0].Trim().Replace("k", "000"), out var min))
                        salaryMin = min;
                    if (decimal.TryParse(salaryParts[1].Trim().Replace("k", "000"), out var max))
                        salaryMax = max;
                }
            }

            var jobPosting = new JobPosting
            {
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                CompanyName = request.Company,
                JobType = request.JobType,
                Category = request.Category,
                Requirements = request.Requirements,
                Benefits = request.Benefits,
                SalaryMin = salaryMin,
                SalaryMax = salaryMax,
                ExpiresAt = request.ExpiryDate,
                IsActive = true,
                UserId = userId
            };

            _context.JobPostings.Add(jobPosting);
            await _context.SaveChangesAsync();

            var jobResponse = new
            {
                Id = jobPosting.Id,
                Title = jobPosting.Title,
                Description = jobPosting.Description,
                Location = jobPosting.Location,
                Company = jobPosting.CompanyName,
                JobType = request.JobType,
                Category = request.Category,
                SalaryRange = request.SalaryRange,
                Requirements = request.Requirements,
                Benefits = request.Benefits,
                PostedDate = jobPosting.CreatedAt,
                ExpiryDate = jobPosting.ExpiresAt,
                IsActive = jobPosting.IsActive,
                PostedById = userId,
                PostedByName = $"{user.FirstName} {user.LastName}",
                ApplicationCount = 0
            };

            return Ok(new { Success = true, Message = "Job posted successfully!", Job = jobResponse });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating job");
            return StatusCode(500, new { Success = false, Message = "An error occurred while creating the job" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetJob(int id)
    {
        try
        {
            var job = await _context.JobPostings
                .Include(j => j.User)
                .Where(j => j.Id == id && j.IsActive)
                .Select(j => new
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    Company = j.CompanyName,
                    JobType = j.JobType,
                    Category = j.Category,
                    SalaryRange = j.SalaryMin.HasValue && j.SalaryMax.HasValue ? $"${j.SalaryMin:N0} - ${j.SalaryMax:N0}" : "",
                    Requirements = j.Requirements,
                    Benefits = j.Benefits,
                    PostedDate = j.CreatedAt,
                    ExpiryDate = j.ExpiresAt,
                    IsActive = j.IsActive,
                    PostedById = j.UserId,
                    PostedByName = $"{j.User.FirstName} {j.User.LastName}",
                    ApplicationCount = 0
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
    public async Task<ActionResult> GetMyJobs()
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
                .Select(j => new
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    Company = j.CompanyName,
                    JobType = j.JobType,
                    Category = j.Category,
                    SalaryRange = j.SalaryMin.HasValue && j.SalaryMax.HasValue ? $"${j.SalaryMin:N0} - ${j.SalaryMax:N0}" : "",
                    Requirements = j.Requirements,
                    Benefits = j.Benefits,
                    PostedDate = j.CreatedAt,
                    ExpiryDate = j.ExpiresAt,
                    IsActive = j.IsActive,
                    PostedById = j.UserId,
                    PostedByName = $"{j.User.FirstName} {j.User.LastName}",
                    ApplicationCount = 0
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

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Employer")]
    public async Task<ActionResult> UpdateJob(int id, [FromBody] UpdateJobRequest request)
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

            // Parse salary range to min/max values
            decimal? salaryMin = null, salaryMax = null;
            if (!string.IsNullOrEmpty(request.SalaryRange))
            {
                var salaryParts = request.SalaryRange.Replace("$", "").Replace(",", "").Split('-', 2);
                if (salaryParts.Length == 2)
                {
                    if (decimal.TryParse(salaryParts[0].Trim().Replace("k", "000"), out var min))
                        salaryMin = min;
                    if (decimal.TryParse(salaryParts[1].Trim().Replace("k", "000"), out var max))
                        salaryMax = max;
                }
            }

            jobPosting.Title = request.Title;
            jobPosting.Description = request.Description;
            jobPosting.Location = request.Location;
            jobPosting.CompanyName = request.Company;
            jobPosting.JobType = request.JobType;
            jobPosting.Category = request.Category;
            jobPosting.Requirements = request.Requirements;
            jobPosting.Benefits = request.Benefits;
            jobPosting.SalaryMin = salaryMin;
            jobPosting.SalaryMax = salaryMax;
            jobPosting.ExpiresAt = request.ExpiryDate;
            jobPosting.IsActive = request.IsActive;

            await _context.SaveChangesAsync();

            var jobResponse = new
            {
                Id = jobPosting.Id,
                Title = jobPosting.Title,
                Description = jobPosting.Description,
                Location = jobPosting.Location,
                Company = jobPosting.CompanyName,
                JobType = request.JobType,
                Category = request.Category,
                SalaryRange = request.SalaryRange,
                Requirements = request.Requirements,
                Benefits = request.Benefits,
                PostedDate = jobPosting.CreatedAt,
                ExpiryDate = jobPosting.ExpiresAt,
                IsActive = jobPosting.IsActive,
                PostedById = jobPosting.UserId,
                PostedByName = $"{jobPosting.User.FirstName} {jobPosting.User.LastName}",
                ApplicationCount = 0
            };

            return Ok(new { Success = true, Message = "Job updated successfully!", Job = jobResponse });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating job with ID {JobId}", id);
            return StatusCode(500, new { Success = false, Message = "An error occurred while updating the job" });
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

            return Ok(new { Success = true, Message = "Job deleted successfully!" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting job with ID {JobId}", id);
            return StatusCode(500, new { Success = false, Message = "An error occurred while deleting the job" });
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult> SearchJobs([FromQuery] string? searchTerm, [FromQuery] string? location, [FromQuery] string? category)
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
                .Select(j => new
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    Company = j.CompanyName,
                    JobType = j.JobType,
                    Category = j.Category,
                    SalaryRange = j.SalaryMin.HasValue && j.SalaryMax.HasValue ? $"${j.SalaryMin:N0} - ${j.SalaryMax:N0}" : "",
                    Requirements = j.Requirements,
                    Benefits = j.Benefits,
                    PostedDate = j.CreatedAt,
                    ExpiryDate = j.ExpiresAt,
                    IsActive = j.IsActive,
                    PostedById = j.UserId,
                    PostedByName = $"{j.User.FirstName} {j.User.LastName}",
                    ApplicationCount = 0
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

public class CreateJobRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string SalaryRange { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public string Benefits { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
}

public class UpdateJobRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string SalaryRange { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public string Benefits { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; } = true;
} 