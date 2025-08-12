namespace CareerBuilder.API.Models
{
    public class WorkExperienceKeyword
    {
        public int Id { get; set; }
        public int WorkExperienceId { get; set; }
        public string Keyword { get; set; }
        public decimal Weight { get; set; }
        public string Category { get; set; }
        public virtual WorkExperience WorkExperience { get; set; }
    }
} 