using CareerBuilder.API.Models;
using CareerBuilder.API.Services.Interface;
using DocumentFormat.OpenXml.Packaging;
using iText.Kernel.Pdf.Canvas.Parser;
using System;
using System.IO;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace CareerBuilder.API.Services.Implementations
{
    public class TextExtractor : ITextExtractor
    {
        private readonly PdfTextExtractor _pdf;
        private readonly DocxTextExtractor _docx;
        private readonly ImageTextExtractor _image;
        public TextExtractor(PdfTextExtractor pdf, DocxTextExtractor docx, ImageTextExtractor image)
        {
            _pdf = pdf;
            _docx = docx;
            _image = image;
        }
        public Task<TextExtractionResult> ExtractTextAsync(Stream stream, string extension, string? contentType, CancellationToken ct = default)
        {
            extension = (extension ?? string.Empty).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => _pdf.ExtractTextAsync(stream, extension, contentType, ct),
                ".docx" => _docx.ExtractTextAsync(stream, extension, contentType, ct),
                ".jpg" or ".jpeg" or ".png" or ".bmp" or ".tif" or ".tiff" or ".webp"
                        => _image.ExtractTextAsync(stream, extension, contentType, ct),
                _ => throw new NotSupportedException($"Unsupported extension {extension}")
            };
        }

    }
} 