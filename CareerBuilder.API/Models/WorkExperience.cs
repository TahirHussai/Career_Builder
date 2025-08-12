using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CareerBuilder.API.Models
{
    public class WorkExperience
    {
        [Key]
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentJob { get; set; }
        public string? Description { get; set; }
        public string? Responsibilities { get; set; }
        public string? Achievements { get; set; }

        public Resume Resume { get; set; } // Navigation property
        public virtual ICollection<WorkExperienceKeyword> Keywords { get; set; } = new List<WorkExperienceKeyword>();

        [JsonIgnore]
        public bool IsCurrent => IsCurrentJob;
    }
} 