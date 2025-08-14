using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CareerBuilder.API.Models
{
    public class Resume
    {
        [Key]
        public int ResumeId { get; set; }
        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        public string RawText { get; set; } = string.Empty;

        public string? ProfessionalSummary { get; set; }

        [Required]
        public string CandidateName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public ContactInformation ContactInformation { get; set; } = new();

        public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();

        public ICollection<WorkExperience> Experiences { get; set; } = new List<WorkExperience>();

        public virtual ICollection<Education> Education { get; set; } = new List<Education>();

        public virtual ICollection<Certification> Certifications { get; set; } = new List<Certification>();

        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";

        public DateTime? LastProcessedAt { get; set; }

        public int? ProcessingAttempts { get; set; }

        public string? ProcessingError { get; set; }

        public decimal? MatchScore { get; set; }

        [Required]
        public byte[] FileContent { get; set; } = Array.Empty<byte>();

        [JsonIgnore]
        public string ContentType => FileName.ToLower().EndsWith(".pdf") ? "application/pdf" : "application/octet-stream";

        public virtual ICollection<ResumeKeyword> Keywords { get; set; } = new List<ResumeKeyword>();

        // Alias for UploadDate for backward compatibility
        [JsonIgnore]
        public DateTime UploadedDate { get => UploadDate; }

        public string UserId { get; set; }
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;

        public string FileHash { get; set; } = string.Empty;

        public string? ParsedText { get; set; }

        public DateTime UploadedAt { get; set; }

        public DateTime? LastParsedAt { get; set; }

        public bool IsActive { get; set; }
    }
} 