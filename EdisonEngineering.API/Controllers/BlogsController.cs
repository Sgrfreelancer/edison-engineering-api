using Microsoft.AspNetCore.Mvc;
using EdisonEngineering.Application.Interfaces;

namespace EdisonEngineering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogsController : ControllerBase
{
    private readonly IBlogService _service;

    public BlogsController(IBlogService service)
    {
        _service = service;
    }

    // GET: /api/blogs
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(data);
    }

    // GET: /api/blogs/{slug}
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var data = await _service.GetBySlugAsync(slug);

        if (data == null)
            return NotFound();

        return Ok(data);
    }
}