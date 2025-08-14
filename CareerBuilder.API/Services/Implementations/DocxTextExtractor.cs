using CareerBuilder.API.Models;
using CareerBuilder.API.Services.Interface;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;

namespace CareerBuilder.API.Services.Implementations
{
    public class DocxTextExtractor : ITextExtractor
    {
        public Task<TextExtractionResult> ExtractTextAsync(Stream stream, string extension, string? contentType, CancellationToken ct = default)
        {
            // OpenXml needs a seekable stream
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Position = 0;

            var sb = new StringBuilder();
            using (var doc = WordprocessingDocument.Open(ms, false))
            {
                var body = doc.MainDocumentPart?.Document?.Body;
                if (body != null)
                {
                    foreach (var para in body.Descendants<Paragraph>())
                    {
                        ct.ThrowIfCancellationRequested();
                        var text = string.Concat(para.Descendants<Text>().Select(t => t.Text));
                        if (!string.IsNullOrWhiteSpace(text))
                            sb.AppendLine(text);
                    }
                }
            }

            return Task.FromResult(new TextExtractionResult { Text = sb.ToString() });
        }
    }

}
