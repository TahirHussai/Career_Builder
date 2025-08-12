using CareerBuilder.API.Models;
using System.Threading.Tasks;

namespace CareerBuilder.API.Services.Interface
{
    public interface ITextExtractor
    {
        Task<TextExtractionResult> ExtractTextAsync(Stream stream, string extension, string? contentType, CancellationToken ct = default);
    }
} 