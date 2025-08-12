using CareerBuilder.API.Models;
using CareerBuilder.API.Services.Interface;
using Tesseract;

namespace CareerBuilder.API.Services.Implementations
{
    public class ImageTextExtractor : ITextExtractor
    {
        private readonly string _tessDataPath;     // path to tessdata folder
        private readonly string _lang;             // e.g., "eng" or "eng+deu"

        public ImageTextExtractor(IConfiguration config)
        {
            _tessDataPath = config["Ocr:TessDataPath"] ?? "tessdata";
            _lang = config["Ocr:Languages"] ?? "eng";
        }

        public async Task<TextExtractionResult> ExtractTextAsync(Stream stream, string extension, string? contentType, CancellationToken ct = default)
        {
            // Tesseract wrapper typically needs a file or a Pix. Use temp file for simplicity.
            var temp = Path.GetTempFileName() + extension;
            await using (var fs = File.Create(temp))
            {
                await stream.CopyToAsync(fs, ct);
            }

            try
            {
                using var engine = new TesseractEngine(_tessDataPath, _lang, EngineMode.Default);
                using var img = Pix.LoadFromFile(temp);
                using var page = engine.Process(img);
                var text = page.GetText() ?? string.Empty;
                var conf = page.GetMeanConfidence();

                return new TextExtractionResult
                {
                    Text = text,
                    OcrConfidence = conf
                };
            }
            finally
            {
                try { File.Delete(temp); } catch { /* ignore */ }
            }
        }
    }
}
