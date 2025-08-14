using CareerBuilder.API.Models;
using CareerBuilder.API.Services.Interface;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Text;

namespace CareerBuilder.API.Services.Implementations
{
    public class PdfTextExtractor : ITextExtractor
    {
        public Task<TextExtractionResult> ExtractTextAsync(Stream stream, string extension, string? contentType, CancellationToken ct = default)
        {
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Position = 0;

            var sb = new StringBuilder();

            using var reader = new PdfReader(ms);
            using var doc = new PdfDocument(reader);

            //for (int i = 1; i <= doc.GetNumberOfPages(); i++)
            //{
            //    ct.ThrowIfCancellationRequested();
            //    var page = doc.GetPage(i);
            //    var text = PdfTextExtractor.GetTextFromPage(page, new SimpleTextExtractionStrategy());
            //    sb.AppendLine(text);
            //}

            return Task.FromResult(new TextExtractionResult { Text = sb.ToString() });
        }
    }
}
