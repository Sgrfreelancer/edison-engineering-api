using Microsoft.AspNetCore.Mvc;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Common;

namespace EdisonEngineering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolarCalculatorController : ControllerBase
{
    private readonly ISolarCalculatorService _service;
    private readonly ILogger<SolarCalculatorController> _logger;

    public SolarCalculatorController(
        ISolarCalculatorService service,
        ILogger<SolarCalculatorController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // POST: /api/solarcalculator
    [HttpPost]
    public async Task<IActionResult> Calculate(
        [FromBody] SolarCalculatorRequestDto request)
    {
        if (request == null)
        {
            _logger.LogWarning(
                "Solar calculator request body was null");

            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Invalid request body"
            });
        }

        _logger.LogInformation(
            "Solar calculation request received for city: {City}, MonthlyBill: {MonthlyBill}",
            request.City,
            request.MonthlyBill);

        if (request.MonthlyBill <= 0)
        {
            _logger.LogWarning(
                "Invalid monthly bill received: {MonthlyBill}",
                request.MonthlyBill);

            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Monthly bill must be greater than 0"
            });
        }

        var result = await _service.CalculateAsync(request);

        if (result == null)
        {
            _logger.LogWarning(
                "Solar calculation failed for city: {City}",
                request.City);

            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Unable to calculate solar estimation"
            });
        }

        _logger.LogInformation(
            "Solar calculation completed successfully for city: {City}",
            request.City);

        return Ok(new ApiResponse<SolarCalculatorResponseDto>
        {
            Success = true,
            Message = "Calculation completed successfully",
            Data = result
        });
    }
}