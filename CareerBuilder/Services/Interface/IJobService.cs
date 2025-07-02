using CareerBuilder.Models;

namespace CareerBuilder.Services.Interface;

public interface IJobService
{
    // Job Management
    Task<List<Job>> GetJobsAsync();
    Task<Job?> GetJobByIdAsync(int jobId);
    Task<List<Job>> GetMyJobsAsync(); // For employers to see their posted jobs
    Task<JobResponse> CreateJobAsync(CreateJobRequest request);
    Task<JobResponse> UpdateJobAsync(int jobId, UpdateJobRequest request);
    Task<JobResponse> DeleteJobAsync(int jobId);
    Task<List<Job>> SearchJobsAsync(string searchTerm, string location, string category);

    // Job Applications
    Task<JobApplicationResponse> ApplyToJobAsync(int jobId, CreateJobApplicationRequest request);
    Task<List<JobApplication>> GetJobApplicationsAsync(int jobId); // For employers to see applications for their jobs
    Task<List<JobApplication>> GetMyApplicationsAsync(); // For job seekers to see their applications
    Task<JobApplicationResponse> UpdateApplicationStatusAsync(int applicationId, string status);
    Task<bool> HasUserAppliedAsync(int jobId); // Check if current user has applied to a job
}
