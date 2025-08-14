using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CareerBuilder.API.Models
{
    public class Certification
    {
        [Key]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("authority")]
        public string? Authority { get; set; }

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("credentialId")]
        public string? CredentialId { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonIgnore]
        public int ResumeId { get; set; }

        [JsonIgnore]
        public Resume Resume { get; set; } = null!;
    }
} 