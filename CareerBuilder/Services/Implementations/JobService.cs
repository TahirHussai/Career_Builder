using CareerBuilder.Models;
using CareerBuilder.Services.Interface;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CareerBuilder.Services.Implementations;

public class JobService : IJobService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;
    private readonly ILogger<JobService> _logger;
    private static readonly List<Job> _jobs = new();
    private static readonly List<JobApplication> _applications = new();
    private static int _nextJobId = 1000;
    private static int _nextApplicationId = 1;
    private const string ApiBaseUrl = "http://localhost:5047/api";

    static JobService()
    {
        // Initialize with sample data if empty
        if (!_jobs.Any())
        {
            InitializeSampleData();
        }
    }

    public JobService(HttpClient httpClient, IAuthService authService, ILogger<JobService> logger)
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

    public async Task<List<Job>> GetJobsAsync()
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/jobs");
            
            if (response.IsSuccessStatusCode)
            {
                var jobs = await response.Content.ReadFromJsonAsync<List<Job>>();
                return jobs ?? new List<Job>();
            }
            else
            {
                _logger.LogError("API call failed with status {StatusCode}", response.StatusCode);
                // Fallback to sample data if API fails
                return GetSampleJobs();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting jobs");
            // Fallback to sample data if API fails
            return GetSampleJobs();
        }
    }

    public async Task<Job?> GetJobByIdAsync(int jobId)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/jobs/{jobId}");
            
            if (response.IsSuccessStatusCode)
            {
                var job = await response.Content.ReadFromJsonAsync<Job>();
                return job;
            }
            else
            {
                _logger.LogError("API call failed with status {StatusCode}", response.StatusCode);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting job by ID");
            return null;
        }
    }

    public async Task<List<Job>> GetMyJobsAsync()
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/jobs/my-jobs");
            
            if (response.IsSuccessStatusCode)
            {
                var jobs = await response.Content.ReadFromJsonAsync<List<Job>>();
                return jobs ?? new List<Job>();
            }
            else
            {
                _logger.LogError("API call failed with status {StatusCode}", response.StatusCode);
                return new List<Job>();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting my jobs");
            return new List<Job>();
        }
    }

    public async Task<JobResponse> CreateJobAsync(CreateJobRequest request)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            
            var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/jobs", request);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JobResponse>();
                return result ?? new JobResponse { Success = false, Message = "Invalid response from server" };
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("API call failed with status {StatusCode}: {Content}", response.StatusCode, errorContent);
                return new JobResponse { Success = false, Message = "Failed to create job. Please try again." };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating job");
            return new JobResponse { Success = false, Message = "An error occurred while posting the job" };
        }
    }

    public async Task<JobResponse> UpdateJobAsync(int jobId, UpdateJobRequest request)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            
            var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/jobs/{jobId}", request);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JobResponse>();
                return result ?? new JobResponse { Success = false, Message = "Invalid response from server" };
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("API call failed with status {StatusCode}: {Content}", response.StatusCode, errorContent);
                return new JobResponse { Success = false, Message = "Failed to update job. Please try again." };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating job");
            return new JobResponse { Success = false, Message = "An error occurred while updating the job" };
        }
    }

    public async Task<JobResponse> DeleteJobAsync(int jobId)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            
            var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/jobs/{jobId}");
            
            if (response.IsSuccessStatusCode)
            {
                return new JobResponse
                {
                    Success = true,
                    Message = "Job deleted successfully!"
                };
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("API call failed with status {StatusCode}: {Content}", response.StatusCode, errorContent);
                return new JobResponse { Success = false, Message = "Failed to delete job. Please try again." };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting job");
            return new JobResponse { Success = false, Message = "An error occurred while deleting the job" };
        }
    }

    public async Task<List<Job>> SearchJobsAsync(string searchTerm, string location, string category)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(searchTerm))
                queryParams.Add($"searchTerm={Uri.EscapeDataString(searchTerm)}");
            if (!string.IsNullOrEmpty(location))
                queryParams.Add($"location={Uri.EscapeDataString(location)}");
            if (!string.IsNullOrEmpty(category))
                queryParams.Add($"category={Uri.EscapeDataString(category)}");
            
            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/jobs/search{queryString}");
            
            if (response.IsSuccessStatusCode)
            {
                var jobs = await response.Content.ReadFromJsonAsync<List<Job>>();
                return jobs ?? new List<Job>();
            }
            else
            {
                _logger.LogError("API call failed with status {StatusCode}", response.StatusCode);
                // Fallback to sample data if API fails
                return GetSampleJobs();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching jobs");
            // Fallback to sample data if API fails
            return GetSampleJobs();
        }
    }

    public async Task<JobApplicationResponse> ApplyToJobAsync(int jobId, CreateJobApplicationRequest request)
    {
        try
        {
            await Task.Delay(1000); // Simulate API call

            var currentUser = await _authService.GetCurrentUserAsync();
            var job = _jobs.FirstOrDefault(j => j.Id == jobId);

            if (job == null)
            {
                return new JobApplicationResponse { Success = false, Message = "Job not found" };
            }

            // Check if user already applied
            if (_applications.Any(a => a.JobId == jobId && a.ApplicantId == currentUser?.Id))
            {
                return new JobApplicationResponse { Success = false, Message = "You have already applied to this job" };
            }

            var newApplication = new JobApplication
            {
                Id = _nextApplicationId++,
                JobId = jobId,
                JobTitle = job.Title,
                Company = job.Company,
                ApplicantId = currentUser?.Id ?? "",
                ApplicantName = $"{currentUser?.FirstName} {currentUser?.LastName}",
                ApplicantEmail = currentUser?.Email ?? "",
                CoverLetter = request.CoverLetter,
                ResumeFileName = request.ResumeFileName,
                AppliedDate = DateTime.Now,
                Status = "Applied"
            };

            _applications.Add(newApplication);

            return new JobApplicationResponse
            {
                Success = true,
                Message = "Application submitted successfully!",
                Application = newApplication
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error applying to job");
            return new JobApplicationResponse { Success = false, Message = "An error occurred while submitting your application" };
        }
    }

    public async Task<List<JobApplication>> GetJobApplicationsAsync(int jobId)
    {
        try
        {
            await Task.Delay(500); // Simulate API call
            return _applications.Where(a => a.JobId == jobId).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting job applications");
            return new List<JobApplication>();
        }
    }

    public async Task<List<JobApplication>> GetMyApplicationsAsync()
    {
        try
        {
            await Task.Delay(500); // Simulate API call
            var currentUser = await _authService.GetCurrentUserAsync();
            return _applications.Where(a => a.ApplicantId == currentUser?.Id).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting my applications");
            return new List<JobApplication>();
        }
    }

    public async Task<JobApplicationResponse> UpdateApplicationStatusAsync(int applicationId, string status)
    {
        try
        {
            await Task.Delay(500); // Simulate API call

            var application = _applications.FirstOrDefault(a => a.Id == applicationId);
            if (application == null)
            {
                return new JobApplicationResponse { Success = false, Message = "Application not found" };
            }

            application.Status = status;

            return new JobApplicationResponse
            {
                Success = true,
                Message = "Application status updated successfully!",
                Application = application
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating application status");
            return new JobApplicationResponse { Success = false, Message = "An error occurred while updating the application status" };
        }
    }

    public async Task<bool> HasUserAppliedAsync(int jobId)
    {
        try
        {
            await Task.Delay(200); // Simulate API call
            var currentUser = await _authService.GetCurrentUserAsync();
            return _applications.Any(a => a.JobId == jobId && a.ApplicantId == currentUser?.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if user has applied");
            return false;
        }
    }

    private static void InitializeSampleData()
    {
        // Initialize sample jobs
        _jobs.AddRange(new List<Job>
        {
            new Job
            {
                Id = 1,
                Title = "Senior Software Engineer",
                Description = "We're looking for a senior software engineer to join our growing team. You'll be responsible for designing and implementing scalable web applications using modern technologies.",
                Company = "TechCorp Inc.",
                Location = "San Francisco, CA",
                JobType = "Full-time",
                Category = "technology",
                SalaryRange = "$120k - $160k",
                Requirements = "5+ years experience with C#, .NET, React, SQL Server. Experience with cloud platforms preferred.",
                Benefits = "Health insurance, 401k matching, flexible work hours, remote work options.",
                PostedDate = DateTime.Now.AddDays(-2),
                ExpiryDate = DateTime.Now.AddDays(28),
                IsActive = true,
                PostedById = "employer-1",
                PostedByName = "TechCorp HR",
                ApplicationCount = 5
            },
            new Job
            {
                Id = 2,
                Title = "Product Marketing Manager",
                Description = "Drive product marketing strategy and go-to-market execution for our SaaS platform. Work closely with product, sales, and engineering teams.",
                Company = "StartupXYZ",
                Location = "Remote",
                JobType = "Full-time",
                Category = "marketing",
                SalaryRange = "$90k - $120k",
                Requirements = "3+ years in product marketing, experience with B2B SaaS, strong analytical skills.",
                Benefits = "Equity package, unlimited PTO, home office stipend, professional development budget.",
                PostedDate = DateTime.Now.AddDays(-5),
                ExpiryDate = DateTime.Now.AddDays(25),
                IsActive = true,
                PostedById = "employer-2",
                PostedByName = "StartupXYZ Team",
                ApplicationCount = 3
            },
            new Job
            {
                Id = 3,
                Title = "UX Designer",
                Description = "Create beautiful and intuitive user experiences for our clients. Work on diverse projects from mobile apps to enterprise software.",
                Company = "Design Studio",
                Location = "New York, NY",
                JobType = "Contract",
                Category = "design",
                SalaryRange = "$80k - $100k",
                Requirements = "Portfolio demonstrating UX/UI skills, proficiency in Figma, user research experience.",
                Benefits = "Flexible schedule, creative environment, opportunity to work with top brands.",
                PostedDate = DateTime.Now.AddDays(-1),
                ExpiryDate = DateTime.Now.AddDays(29),
                IsActive = true,
                PostedById = "employer-3",
                PostedByName = "Design Studio HR",
                ApplicationCount = 2
            }
        });

        // Initialize sample applications
        _applications.AddRange(new List<JobApplication>
        {
            // Applications for Job 1 (Senior Software Engineer)
            new JobApplication
            {
                Id = 1,
                JobId = 1,
                JobTitle = "Senior Software Engineer",
                Company = "TechCorp Inc.",
                ApplicantId = "jobseeker-1",
                ApplicantName = "John Doe",
                ApplicantEmail = "john.doe@email.com",
                CoverLetter = "I am excited to apply for this Senior Software Engineer position. With 5+ years of experience in full-stack development...",
                ResumeFileName = "john_doe_resume.pdf",
                AppliedDate = DateTime.Now.AddDays(-1),
                Status = "Applied"
            },
            new JobApplication
            {
                Id = 2,
                JobId = 1,
                JobTitle = "Senior Software Engineer",
                Company = "TechCorp Inc.",
                ApplicantId = "jobseeker-2",
                ApplicantName = "Sarah Johnson",
                ApplicantEmail = "sarah.johnson@email.com",
                CoverLetter = "Dear Hiring Manager, I am writing to express my strong interest in the Senior Software Engineer role...",
                ResumeFileName = "sarah_johnson_resume.pdf",
                AppliedDate = DateTime.Now.AddDays(-2),
                Status = "Reviewed"
            },
            new JobApplication
            {
                Id = 3,
                JobId = 1,
                JobTitle = "Senior Software Engineer",
                Company = "TechCorp Inc.",
                ApplicantId = "jobseeker-3",
                ApplicantName = "Michael Chen",
                ApplicantEmail = "michael.chen@email.com",
                CoverLetter = "I have been following TechCorp's innovative work in cloud computing and would love to contribute...",
                ResumeFileName = "michael_chen_resume.pdf",
                AppliedDate = DateTime.Now.AddDays(-3),
                Status = "Interview"
            },
            new JobApplication
            {
                Id = 4,
                JobId = 1,
                JobTitle = "Senior Software Engineer",
                Company = "TechCorp Inc.",
                ApplicantId = "jobseeker-4",
                ApplicantName = "Emily Rodriguez",
                ApplicantEmail = "emily.rodriguez@email.com",
                CoverLetter = "As a passionate software engineer with expertise in React and Node.js...",
                ResumeFileName = "emily_rodriguez_resume.pdf",
                AppliedDate = DateTime.Now.AddDays(-4),
                Status = "Applied"
            },
            new JobApplication
            {
                Id = 5,
                JobId = 1,
                JobTitle = "Senior Software Engineer",
                Company = "TechCorp Inc.",
                ApplicantId = "jobseeker-5",
                ApplicantName = "David Kim",
                ApplicantEmail = "david.kim@email.com",
                CoverLetter = "I am thrilled to apply for this opportunity to work with cutting-edge technologies...",
                ResumeFileName = "david_kim_resume.pdf",
                AppliedDate = DateTime.Now.AddDays(-5),
                Status = "Hired"
            },

            // Applications for Job 2 (Product Marketing Manager)
            new JobApplication
            {
                Id = 6,
                JobId = 2,
                JobTitle = "Product Marketing Manager",
                Company = "StartupXYZ",
                ApplicantId = "jobseeker-6",
                ApplicantName = "Lisa Wang",
                ApplicantEmail = "lisa.wang@email.com",
                CoverLetter = "With my background in B2B marketing and product management...",
                ResumeFileName = "lisa_wang_resume.pdf",
                AppliedDate = DateTime.Now.AddDays(-1),
                Status = "Applied"
            },
            new JobApplication
            {
                Id = 7,
                JobId = 2,
                JobTitle = "Product Marketing Manager",
                Company = "StartupXYZ",
                ApplicantId = "jobseeker-7",
                ApplicantName = "James Wilson",
                ApplicantEmail = "james.wilson@email.com",
                CoverLetter = "I am excited about the opportunity to drive product marketing strategy...",
                ResumeFileName = "james_wilson_resume.pdf",
                AppliedDate = DateTime.Now.AddDays(-2),
                Status = "Reviewed"
            },
            new JobApplication
            {
                Id = 8,
                JobId = 2,
                JobTitle = "Product Marketing Manager",
                Company = "StartupXYZ",
                ApplicantId = "jobseeker-8",
                ApplicantName = "Amanda Taylor",
                ApplicantEmail = "amanda.taylor@email.com",
                CoverLetter = "Having successfully launched 3 products in the SaaS space...",
                ResumeFileName = "amanda_taylor_resume.pdf",
                AppliedDate = DateTime.Now.AddDays(-3),
                Status = "Interview"
            },

            // Applications for Job 3 (UX Designer)
            new JobApplication
            {
                Id = 9,
                JobId = 3,
                JobTitle = "UX Designer",
                Company = "Design Studio",
                ApplicantId = "jobseeker-9",
                ApplicantName = "Alex Thompson",
                ApplicantEmail = "alex.thompson@email.com",
                CoverLetter = "As a UX designer passionate about creating intuitive user experiences...",
                ResumeFileName = "alex_thompson_resume.pdf",
                AppliedDate = DateTime.Now.AddDays(-1),
                Status = "Applied"
            },
            new JobApplication
            {
                Id = 10,
                JobId = 3,
                JobTitle = "UX Designer",
                Company = "Design Studio",
                ApplicantId = "jobseeker-10",
                ApplicantName = "Rachel Green",
                ApplicantEmail = "rachel.green@email.com",
                CoverLetter = "I would love to bring my design thinking approach to your team...",
                ResumeFileName = "rachel_green_resume.pdf",
                AppliedDate = DateTime.Now.AddDays(-2),
                Status = "Reviewed"
            }
        });

        _nextJobId = 1000;
        _nextApplicationId = 100;
    }

    private List<Job> GetSampleJobs()
    {
        return _jobs.ToList();
    }

    private List<JobApplication> GetSampleApplications()
    {
        return _applications.ToList();
    }
}
