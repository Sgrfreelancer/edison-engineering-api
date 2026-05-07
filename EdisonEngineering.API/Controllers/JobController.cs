using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EdisonEngineering.Application.Common;

namespace EdisonEngineering.API.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobController : ControllerBase
{
    private readonly IJobService _service;
    private readonly ILogger<JobController> _logger;

    public JobController(
        IJobService service,
        ILogger<JobController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: /api/jobs
    [HttpGet]
    public async Task<IActionResult> GetJobs()
    {
        _logger.LogInformation("Fetching all jobs");

        var jobs = await _service.GetJobsAsync();

        if (jobs == null || !jobs.Any())
        {
            _logger.LogWarning("No jobs found");

            return NotFound(new ApiResponse<List<JobDto>>
            {
                Success = false,
                Message = "No jobs found",
                Data = new List<JobDto>()
            });
        }

        _logger.LogInformation(
            "Jobs fetched successfully. Count: {Count}",
            jobs.Count);

        return Ok(new ApiResponse<List<JobDto>>
        {
            Success = true,
            Message = "Jobs fetched successfully",
            Data = jobs
        });
    }

    // POST: /api/jobs/apply
    [HttpPost("apply")]
    public async Task<IActionResult> Apply([FromBody] ApplyJobDto dto)
    {
        _logger.LogInformation(
            "Job application request received for email: {Email}",
            dto?.Email);

        if (dto == null)
        {
            _logger.LogWarning("Job application request body was null");

            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Invalid request"
            });
        }

        await _service.ApplyAsync(dto);

        _logger.LogInformation(
            "Job application submitted successfully for email: {Email}",
            dto.Email);

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Application submitted successfully"
        });
    }
}