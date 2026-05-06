using Microsoft.AspNetCore.Mvc;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Application.DTOs;

namespace EdisonEngineering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolarCalculatorController : ControllerBase
{
    private readonly ISolarCalculatorService _service;

    public SolarCalculatorController(ISolarCalculatorService service)
    {
        _service = service;
    }

    // POST: /api/solarcalculator
    [HttpPost]
    public async Task<IActionResult> Calculate([FromBody] SolarCalculatorRequestDto request)
    {
        if (request.MonthlyBill <= 0)
        {
            return BadRequest(new { message = "Monthly bill must be greater than 0" });
        }

        var result = await _service.CalculateAsync(request);

        return Ok(result);
    }
}