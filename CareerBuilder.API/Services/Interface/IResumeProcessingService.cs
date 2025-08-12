using CareerBuilder.API.Dtos;
using CareerBuilder.API.Models;

namespace CareerBuilder.API.Services.Interface
{
    public interface IResumeProcessingService
    {
        Task<ResumeDto> ProcessResumeAsync(ResumeUploadDto request, byte[] fileContent);
        //Task<bool> DeleteResumeAsync(int resumeId);
        //Task<Resume?> GetResumeByIdAsync(int resumeId);
        //Task<IEnumerable<Resume>> GetResumesByUserIdAsync(int userId);
        //Task<IEnumerable<Resume>> GetAllResumesAsync();
    }
}
