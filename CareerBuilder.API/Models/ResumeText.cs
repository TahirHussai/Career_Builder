using Microsoft.ML.Data;

namespace SmartResumeScreener.ML.Models;

public class ResumeText
{
    [LoadColumn(0)]
    public string Text { get; set; } = string.Empty;

    [LoadColumn(1)]
    public string[] Keywords { get; set; } = Array.Empty<string>();
}

public class KeywordPrediction
{
    [ColumnName("PredictedKeywords")]
    public string[] Keywords { get; set; } = Array.Empty<string>();
} 