using CareerBuilder.API.Dtos;
using CareerBuilder.API.Models;
using CareerBuilder.API.Services.Implementations;
using CareerBuilder.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerBuilder.API.Controllers
{
    //[Authorize(Roles = "JobSeeker")]
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly IResumeProcessingService _resumeProcessingService;
        private readonly ILogger<CandidatesController> _logger;

        public CandidatesController(
            IResumeProcessingService resumeProcessingService,
            ILogger<CandidatesController> logger)
        {
            _resumeProcessingService = resumeProcessingService ?? throw new ArgumentNullException(nameof(resumeProcessingService));
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadResume(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponseDto.Fail("No file uploaded"));

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
                return BadRequest(ApiResponseDto.Fail("Invalid file format. Only PDF and Word documents are allowed."));

            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var fileContent = memoryStream.ToArray();

                // TODO: Replace with actual logged-in user ID
                var userId = "33d33925-bb94-4ca0-bb08-dd3c9acb39b4";

                var request = new ResumeUploadDto
                {
                    UserId = userId,
                    FileName = file.FileName,
                    FileType = fileExtension,
                    FileSize = file.Length,
                    ContentType = file.ContentType,
                    FileStream = new MemoryStream(fileContent)
                };

                var resumeDto = await _resumeProcessingService.ProcessResumeAsync(request, fileContent);

                return Ok(ApiResponseDto.Success("Resume uploaded and processed successfully", resumeDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading resume");
                return StatusCode(500, ApiResponseDto.Fail("An error occurred while processing the resume"));
            }
        }
    }


}
