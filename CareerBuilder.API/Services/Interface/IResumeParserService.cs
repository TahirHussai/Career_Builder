namespace CareerBuilder.API.Services.Interface
{
    public interface IResumeParserService
    {
        string ExtractTextFromFile(IFormFile file);
         string ExtractWithIronOCR(IFormFile file);
        List<string> ExtractSkills(string text);
        string ExtractFullName(string text);
        string ExtractEmail(string text);
        string ExtractPhone(string text);
        string ExtractSummary(string text);
    }

}
