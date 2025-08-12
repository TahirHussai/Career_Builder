using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CareerBuilder.API.Models
{
   public class ResumeKeyword
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ResumeId { get; set; }

        [Required]
        public string Keyword { get; set; } = string.Empty;

        public decimal Weight { get; set; }
        public string Category { get; set; } = string.Empty;

        public virtual Resume  Resume { get; set; } = null!;
    }
}
