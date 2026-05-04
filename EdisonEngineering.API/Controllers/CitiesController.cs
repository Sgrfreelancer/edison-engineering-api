using Microsoft.AspNetCore.Mvc;
using EdisonEngineering.Application.Interfaces;

namespace EdisonEngineering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly ICityService _service;

    public CitiesController(ICityService service)
    {
        _service = service;
    }

    // GET: /api/cities
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(data);
    }

    // GET: /api/cities/pune
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var data = await _service.GetBySlugAsync(slug);

        if (data == null)
            return NotFound();

        return Ok(data);
    }

    // GET: /api/cities/pune/projects
    [HttpGet("{slug}/projects")]
    public async Task<IActionResult> GetProjects(string slug)
    {
        var data = await _service.GetProjectsAsync(slug);
        return Ok(data);
    }
}