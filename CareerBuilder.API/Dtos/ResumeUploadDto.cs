using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerBuilder.API.Dtos
{
    public class ResumeUploadDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string FileType { get; set; } = string.Empty;

        [Required]
        public Stream FileStream { get; set; } = Stream.Null;

        public string? ContentType { get; set; }

        public long FileSize { get; set; }

        public byte[] GetFileContent()
        {
            if (FileStream == null || !FileStream.CanRead)
                return Array.Empty<byte>();

            using var memoryStream = new MemoryStream();
            FileStream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
