//using CareerBuilder.API.Services.Interface;
//using System.Reflection.PortableExecutable;
//using System.Text.RegularExpressions;
//using System.Text;
//using TikaOnDotNet.TextExtraction;
//using IronOcr;

//namespace CareerBuilder.API.Services.Implementations
//{
//    public class ResumeParserService : IResumeParserService
//    {
//        private readonly string[] knownSkills =
//{
//    // Programming
//    "Java", "Python", "C#", "C++", "Go", "Rust", "Swift", "Kotlin", "Ruby", "PHP",
//    "JavaScript", "TypeScript", "HTML", "CSS", "SQL", "NoSQL", "Bash", "PowerShell",

//    // Web & Frontend
//    "React", "Angular", "Vue", "Svelte", "Bootstrap", "Tailwind",

//    // Frameworks
//    ".NET", "ASP.NET", "Spring", "Django", "Flask", "Laravel", "Node.js",

//    // DevOps & Cloud
//    "Docker", "Kubernetes", "Terraform", "Jenkins", "GitLab CI/CD", "Azure", "AWS", "GCP", "Ansible",

//    // Databases
//    "MySQL", "PostgreSQL", "MongoDB", "Redis", "Oracle", "SQLite",

//    // Data & AI
//    "Pandas", "NumPy", "Scikit-learn", "TensorFlow", "PyTorch", "Machine Learning", "Data Science", "Data Engineering",

//    // Security
//    "OWASP", "Penetration Testing", "Encryption", "OAuth", "IAM", "SIEM",

//    // Healthcare & Lab
//    "Phlebotomy", "Specimen processing", "Clinical Lab", "CPT", "Medical Assistant", "Patient Care",

//    // Tools
//    "MS Office", "Excel", "Power BI", "Tableau", "Jira", "Confluence", "Figma", "Photoshop",

//    // Soft Skills
//    "Communication", "Teamwork", "Leadership", "Time Management", "Problem Solving", "Critical Thinking", "Adaptability",

//    // Misc
//    "Remote Work", "Agile", "Scrum", "Kanban", "QA Testing", "Unit Testing", "Integration Testing"
//};


//        public string ExtractTextFromFile(IFormFile file)
//        {
//            try
//            {
//                using var stream = file.OpenReadStream();
//                using var parser = new GroupDocs.Parser.Parser(stream);

//                if (!parser.Features.Text)
//                    return string.Empty;

//                var sb = new StringBuilder();
//                int pageCount = parser.GetDocumentInfo().PageCount;

//                for (int i = 0; i < pageCount; i++)
//                {
//                    using var pageTextReader = parser.GetText(i);
//                    if (pageTextReader != null)
//                    {
//                        sb.AppendLine(pageTextReader.ReadToEnd());
//                    }
//                }

//                return sb.ToString();
//            }
//            catch (Exception ex)
//            {
//                return $"[Error extracting text: {ex.Message}]";
//            }
//        }

//        public string ExtractWithIronOCR(IFormFile file)
//        {
//            var ocr = new IronOcr.IronTesseract();
//            using var ms = new MemoryStream();
//            file.CopyTo(ms);
//            ms.Position = 0;

//            using var input = new OcrInput(ms);
//            var result = ocr.Read(input);
//            return result.Text;
//        }



//        public List<string> ExtractSkills(string text)
//        {
//            return knownSkills.Where(skill => text.Contains(skill, StringComparison.OrdinalIgnoreCase)).ToList();
//        }

//        public string ExtractSummary(string text)
//        {
//            var summaryStart = text.IndexOf("PROFESSIONAL SUMMARY", StringComparison.OrdinalIgnoreCase);
//            if (summaryStart >= 0)
//            {
//                var snippet = text.Substring(summaryStart);
//                var nextSection = snippet.IndexOf("\n\n");
//                return nextSection > 0 ? snippet[..nextSection].Trim() : snippet.Trim();
//            }
//            return string.Empty;
//        }

//        public string ExtractEmail(string text)
//        {
//            var match = Regex.Match(text, "[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}");
//            return match.Success ? match.Value : string.Empty;
//        }

//        public string ExtractPhone(string text)
//        {
//            var match = Regex.Match(text, "\\(?\\d{3}\\)?[-.\\s]?\\d{3}[-.\\s]?\\d{4}");
//            return match.Success ? match.Value : string.Empty;
//        }

//        public string ExtractFullName(string text)
//        {
//            var lines = text.Split('\n');
//            return lines.Length > 0 ? lines[0].Trim() : "";
//        }
//    }

//}
