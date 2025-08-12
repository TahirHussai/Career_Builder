using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CareerBuilder.API.Models
{
    public class ContactInformation
    {
        public int Id { get; set; }

        [JsonPropertyName("fullName")]
        [Required]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("linkedin")]
        public string? LinkedIn { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }
    }
} 