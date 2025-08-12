using CareerBuilder.API.Data;
using CareerBuilder.API.Dtos;
using CareerBuilder.API.Models;
using CareerBuilder.API.Services.Interface;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using System.Text;
using Microsoft.EntityFrameworkCore;

public class ResumeProcessingService : IResumeProcessingService
{
    private readonly ITextExtractor _textExtractor;
    private readonly IMLKeywordExtractionService _keywordExtractor;
    private readonly ApplicationDbContext _context;

    public ResumeProcessingService(
        ITextExtractor textExtractor,
        IMLKeywordExtractionService keywordExtractor,
        ApplicationDbContext context)
    {
        _textExtractor = textExtractor;
        _keywordExtractor = keywordExtractor;
        _context = context;
    }

    public async Task<ResumeDto> ProcessResumeAsync(ResumeUploadDto request, byte[] fileContent)
    {
        var filePath = SaveResumeFile(request.FileName, fileContent);
        string extension = Path.GetExtension(request.FileName).ToLowerInvariant();

        // Extract raw text from file
        var extractedText = await _textExtractor.ExtractTextAsync(new MemoryStream(fileContent), extension, request.FileType);
        var rawText = extractedText.Text;

        // NEW: Full parsing with the updated MLKeywordExtractionService
        var parsedResume = await _keywordExtractor.ParseResumeAsync(
            rawText,
            Path.GetFileName(filePath),
            request.UserId,
            request.FileType,
            fileContent.Length
        );

        // Save Resume entity
        var resumeEntity = new Resume
        {
            UserId = request.UserId,
            FileName = parsedResume.FileName,
            FileType = parsedResume.FileType,
            FileSize = parsedResume.FileSize,
            UploadDate = DateTime.UtcNow,
            LastModifiedDate = DateTime.UtcNow,
            RawText = rawText,
            Status = "Processed",
            Experiences = MapWorkExperience(parsedResume.WorkExperience),
            Education = MapEducation(parsedResume.Education),
            Certifications = MapCertifications(parsedResume.Certifications),
            Skills = parsedResume.Skills.Select(s => new Skill { Name = s.Name }).ToList(),
            CandidateName = parsedResume.CandidateName,
            ProfessionalSummary = parsedResume.ProfessionalSummary
        };

        await _context.AddAsync(resumeEntity);
        await _context.SaveChangesAsync();

        // Return DTO
        parsedResume.Id = resumeEntity.ResumeId;
        parsedResume.UploadDate = resumeEntity.UploadDate;
        parsedResume.Status = resumeEntity.Status;

        return parsedResume;
    }

    private string SaveResumeFile(string originalFileName, byte[] fileContent)
    {
        var resumesRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Resumes");
        if (!Directory.Exists(resumesRoot))
            Directory.CreateDirectory(resumesRoot);

        var uniqueFileName = $"{Path.GetFileNameWithoutExtension(originalFileName)}-{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
        var filePath = Path.Combine(resumesRoot, uniqueFileName);
        File.WriteAllBytes(filePath, fileContent);
        return filePath;
    }

    private List<WorkExperience> MapWorkExperience(List<ExperienceDto> experiences)
    {
        return experiences.Select(e => new WorkExperience
        {
            CompanyName = e.CompanyName,
            Position = e.Position,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            IsCurrentJob = e.IsCurrentJob,
            Description = e.Description
        }).ToList();
    }

    private List<Education> MapEducation(List<EducationDto> educationList)
    {
        return educationList.Select(e => new Education
        {
            Institution = e.Institution,
            Degree = e.Degree,
            FieldOfStudy = e.FieldOfStudy,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            IsCurrentlyEnrolled = e.IsCurrentlyEnrolled
        }).ToList();
    }

    private List<Certification> MapCertifications(List<CertificationDto> certList)
    {
        return certList.Select(c => new Certification
        {
            Name = c.Name,
             Authority = c.IssuingOrganization,
             Date = c.IssueDate,
           // ExpiryDate = c.ExpiryDate,
           // CredentialId = c.CredentialId,
             Url = c.CredentialUrl
        }).ToList();
    }
}
