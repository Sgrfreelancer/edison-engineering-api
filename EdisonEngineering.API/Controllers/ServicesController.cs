using Microsoft.AspNetCore.Mvc;
using EdisonEngineering.Application.Interfaces;

namespace EdisonEngineering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _service;

    public ServicesController(IServiceService service)
    {
        _service = service;
    }

    // GET: /api/services
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(data);
    }

    // GET: /api/services/solar
    [HttpGet("{categorySlug}")]
    public async Task<IActionResult> GetByCategory(string categorySlug)
    {
        var data = await _service.GetBySlugAsync(categorySlug);

        if (data == null)
            return NotFound();

        return Ok(data);
    }

    // GET: /api/services/solar/homes
    [HttpGet("{categorySlug}/{serviceSlug}")]
    public async Task<IActionResult> GetService(string categorySlug, string serviceSlug)
    {
        var data = await _service.GetServiceAsync(categorySlug, serviceSlug);

        if (data == null)
            return NotFound();

        return Ok(data);
    }
}