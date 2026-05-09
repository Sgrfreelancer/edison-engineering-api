using EdisonEngineering.Application.Common;
using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using Asp.Versioning;

namespace EdisonEngineering.API.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/leads")]
[EnableRateLimiting("fixed")]
public class LeadController : ControllerBase
{
    private readonly ILeadService _service;
    private readonly ILogger<LeadController> _logger;

    public LeadController(
        ILeadService service,
        ILogger<LeadController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // POST: /api/leads
    [HttpPost]
    public async Task<IActionResult> CreateLead([FromBody] CreateLeadDto dto)
    {
        if (dto == null)
        {
            _logger.LogWarning("Lead request body was null");

            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Invalid request body"
            });
        }

        _logger.LogInformation(
            "Lead request received for phone: {Phone}",
            dto.Phone);

        await _service.CreateAsync(dto);

        _logger.LogInformation(
            "Lead created successfully for phone: {Phone}",
            dto.Phone);

        return StatusCode(StatusCodes.Status201Created,
            new ApiResponse<string>
            {
                Success = true,
                Message = "Lead submitted successfully"
            });
    }
}