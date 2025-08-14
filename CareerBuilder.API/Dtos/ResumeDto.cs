using System.ComponentModel.DataAnnotations;

namespace CareerBuilder.API.Dtos
{
    public class ResumeDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        public string RawText { get; set; } = string.Empty;

        public string? ProfessionalSummary { get; set; }

        [Required]
        public string CandidateName { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        [Required]
        public ContactInformationDto ContactInformation { get; set; } = new();

        public List<SkillDto> Skills { get; set; } = new List<SkillDto>();

        public List<ExperienceDto> WorkExperience { get; set; } = new List<ExperienceDto>();

        public List<EducationDto> Education { get; set; } = new List<EducationDto>();

        public List<CertificationDto> Certifications { get; set; } = new List<CertificationDto>();

        public DateTime UploadDate { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime? LastProcessedAt { get; set; }

        public int? ProcessingAttempts { get; set; }

        public string? ProcessingError { get; set; }

        public decimal? MatchScore { get; set; }

        public byte[] FileContent { get; set; } = Array.Empty<byte>();

        public string FileType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string FileHash { get; set; } = string.Empty;

        public string? ParsedText { get; set; }

        public DateTime UploadedAt { get; set; }

        public DateTime? LastParsedAt { get; set; }

        public bool IsActive { get; set; }
    }

    public class ContactInformationDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }
        public string? LinkedIn { get; set; }
        public string? Address { get; set; }
    }

    public class SkillDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public int? YearsOfExperience { get; set; }
        public string? ProficiencyLevel { get; set; }
    }

    public class ExperienceDto
    {
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        public string Position { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public List<string> Responsibilities { get; set; } = new();
        public List<string> Achievements { get; set; } = new();
        public bool IsCurrentJob { get; set; }
    }

    public class EducationDto
    {
        public int Id { get; set; }

        [Required]
        public string Institution { get; set; } = string.Empty;

        [Required]
        public string Degree { get; set; } = string.Empty;

        public string? FieldOfStudy { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal? GPA { get; set; }
        public List<string> Achievements { get; set; } = new();
        public bool IsCurrentlyEnrolled { get; set; }
    }

    public class CertificationDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string IssuingOrganization { get; set; } = string.Empty;

        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string? CredentialId { get; set; }
        public string? CredentialUrl { get; set; }
    }

    public class ProjectDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? Technologies { get; set; }
        public string? Role { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Url { get; set; }
        public List<string> Responsibilities { get; set; } = new();
        public List<string> Achievements { get; set; } = new();
        public bool IsCurrentlyWorking { get; set; }
    }
}