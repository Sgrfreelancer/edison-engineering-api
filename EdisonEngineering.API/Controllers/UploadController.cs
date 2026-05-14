using EdisonEngineering.Application.Common;
using EdisonEngineering.Application.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdisonEngineering.API.Controllers;

[ApiController]
[Route("api/uploads")]
public class UploadController : ControllerBase
{
    private readonly ILogger<UploadController> _logger;

    public UploadController(
        ILogger<UploadController> logger)
    {
        _logger = logger;
    }

    // =========================================================
    // UPLOAD RESUME
    // =========================================================

    [Authorize]

    [HttpPost("resume")]
    public async Task<IActionResult> UploadResume(
        IFormFile file)
    {
        _logger.LogInformation(
            "Resume upload request received");

        // ✅ FILE NULL CHECK

        if (file == null || file.Length == 0)
        {
            return BadRequest(
                new ApiResponse<string>
                {
                    Success = false,
                    Message = "No file uploaded"
                });
        }

        // ✅ ALLOWED EXTENSIONS

        var allowedExtensions =
            new[] { ".pdf", ".doc", ".docx" };

        var extension =
            Path.GetExtension(file.FileName)
                .ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest(
                new ApiResponse<string>
                {
                    Success = false,
                    Message =
                        "Only PDF, DOC, DOCX files are allowed"
                });
        }

        // ✅ FILE SIZE LIMIT (5MB)

        if (file.Length > 5 * 1024 * 1024)
        {
            return BadRequest(
                new ApiResponse<string>
                {
                    Success = false,
                    Message =
                        "File size cannot exceed 5MB"
                });
        }

        // ✅ CREATE FOLDER

        var uploadsFolder =
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "Uploads",
                "resumes");

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(
                uploadsFolder);
        }

        // ✅ UNIQUE FILE NAME

        var uniqueFileName =
            $"{Guid.NewGuid()}{extension}";

        var filePath =
            Path.Combine(
                uploadsFolder,
                uniqueFileName);

        // ✅ SAVE FILE

        using (var stream =
               new FileStream(
                   filePath,
                   FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // ✅ FILE URL

        var fileUrl =
            $"{Request.Scheme}://{Request.Host}/Uploads/resumes/{uniqueFileName}";

        _logger.LogInformation(
            "Resume uploaded successfully: {FileName}",
            uniqueFileName);

        return Ok(
            new ApiResponse<FileUploadResponseDto>
            {
                Success = true,
                Message = "File uploaded successfully",

                Data = new FileUploadResponseDto
                {
                    FileName = uniqueFileName,
                    FileUrl = fileUrl
                }
            });
    }
}