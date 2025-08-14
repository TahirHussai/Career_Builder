namespace CareerBuilder.API.Models
{
    public sealed class TextExtractionResult
    {
        public string Text { get; init; } = string.Empty;
        public string? Language { get; init; }
        public double? OcrConfidence { get; init; } // average or heuristic
    }
}
