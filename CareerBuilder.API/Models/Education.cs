using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CareerBuilder.API.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public string Institution { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string? FieldOfStudy { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentlyEnrolled { get; set; }
        public decimal? GPA { get; set; }
        public string? Achievements { get; set; }

        public virtual Resume Resume { get; set; } = null!;

        [JsonIgnore]
        public bool IsCurrent => IsCurrentlyEnrolled;
    }
} 