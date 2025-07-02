using System.ComponentModel.DataAnnotations;

namespace CareerBuilder.Models;

public class Job
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty; // Full-time, Part-time, Contract, Remote
    public string Category { get; set; } = string.Empty;
    public string SalaryRange { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public string Benefits { get; set; } = string.Empty;
    public DateTime PostedDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; }
    public string PostedById { get; set; } = string.Empty; // User ID who posted
    public string PostedByName { get; set; } = string.Empty;
    public int ApplicationCount { get; set; }
}

public class CreateJobRequest
{
    [Required(ErrorMessage = "Job title is required")]
    [StringLength(200, ErrorMessage = "Job title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job description is required")]
    [StringLength(5000, ErrorMessage = "Job description cannot exceed 5000 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Company name is required")]
    [StringLength(200, ErrorMessage = "Company name cannot exceed 200 characters")]
    public string Company { get; set; } = string.Empty;

    [Required(ErrorMessage = "Location is required")]
    [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job type is required")]
    public string JobType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Salary range cannot exceed 100 characters")]
    public string SalaryRange { get; set; } = string.Empty;

    [StringLength(3000, ErrorMessage = "Requirements cannot exceed 3000 characters")]
    public string Requirements { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Benefits cannot exceed 2000 characters")]
    public string Benefits { get; set; } = string.Empty;

    public DateTime? ExpiryDate { get; set; }
}

public class UpdateJobRequest
{
    [Required(ErrorMessage = "Job title is required")]
    [StringLength(200, ErrorMessage = "Job title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job description is required")]
    [StringLength(5000, ErrorMessage = "Job description cannot exceed 5000 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Company name is required")]
    [StringLength(200, ErrorMessage = "Company name cannot exceed 200 characters")]
    public string Company { get; set; } = string.Empty;

    [Required(ErrorMessage = "Location is required")]
    [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job type is required")]
    public string JobType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Salary range cannot exceed 100 characters")]
    public string SalaryRange { get; set; } = string.Empty;

    [StringLength(3000, ErrorMessage = "Requirements cannot exceed 3000 characters")]
    public string Requirements { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Benefits cannot exceed 2000 characters")]
    public string Benefits { get; set; } = string.Empty;

    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; }
}

public class JobApplication
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string ApplicantId { get; set; } = string.Empty;
    public string ApplicantName { get; set; } = string.Empty;
    public string ApplicantEmail { get; set; } = string.Empty;
    public string CoverLetter { get; set; } = string.Empty;
    public string ResumeFileName { get; set; } = string.Empty;
    public DateTime AppliedDate { get; set; }
    public string Status { get; set; } = string.Empty; // Applied, Reviewed, Interview, Rejected, Hired
}

public class CreateJobApplicationRequest
{
    [Required(ErrorMessage = "Cover letter is required")]
    [StringLength(2000, ErrorMessage = "Cover letter cannot exceed 2000 characters")]
    public string CoverLetter { get; set; } = string.Empty;

    public string ResumeFileName { get; set; } = string.Empty;
}

public class JobResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Job? Job { get; set; }
    public List<Job>? Jobs { get; set; }
}

public class JobApplicationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public JobApplication? Application { get; set; }
    public List<JobApplication>? Applications { get; set; }
}
