using CareerBuilder.API.Dtos;
using CareerBuilder.API.Models;

namespace CareerBuilder.API.Services.Interface
{
    public interface IMLKeywordExtractionService
    {
        Task<ResumeDto> ParseResumeAsync(string rawText, string fileName, string userId, string fileType, long fileSize);
        //Task<IEnumerable<ExperienceDto>> ExtractWorkExperienceAsync(string text);
        //Task<IEnumerable<string>> ExtractSkillsAsync(string text);
        //Task<IEnumerable<EducationDto>> ExtractEducationAsync(string text);
        //Task<Dictionary<string, double>> ExtractKeywordScoresAsync(string text);
        //Task<double> CalculateMatchScoreAsync(string resumeText, string jobDescription);
    }

}
