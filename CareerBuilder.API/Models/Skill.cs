using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace  CareerBuilder.API.Models
{
    public class Skill 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("yearsOfExperience")]
        public int YearsOfExperience { get; set; }

        [Required]
        [JsonPropertyName("proficiency")]
        public string Proficiency { get; set; } = string.Empty; // Beginner, Intermediate, Advanced, Expert

        [JsonIgnore]
        public int ResumeId { get; set; }

        [JsonIgnore]
        public Resume Resume { get; set; } = null!;

        [JsonIgnore]
        public string Keyword => Name;

        [JsonIgnore]
        public decimal Weight => GetWeightFromProficiency();

        private decimal GetWeightFromProficiency()
        {
            return Proficiency.ToLower() switch
            {
                "beginner" => 0.25m,
                "intermediate" => 0.5m,
                "advanced" => 0.75m,
                "expert" => 1.0m,
                _ => 0.0m
            };
        }
    }
} 