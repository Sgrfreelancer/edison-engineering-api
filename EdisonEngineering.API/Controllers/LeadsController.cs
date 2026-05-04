using Microsoft.AspNetCore.Mvc;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Application.DTOs;

namespace EdisonEngineering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeadsController : ControllerBase
{
    private readonly ILeadService _service;

    public LeadsController(ILeadService service)
    {
        _service = service;
    }

    // POST: /api/leads
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLeadDto dto)
    {
        var result = await _service.CreateAsync(dto);

        if (!result)
        {
            return BadRequest(new
            {
                message = "Invalid data. Please fill all required fields."
            });
        }

        return Ok(new
        {
            message = "Your request has been submitted successfully."
        });
    }
}