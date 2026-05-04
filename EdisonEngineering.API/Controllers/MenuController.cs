using Microsoft.AspNetCore.Mvc;
using EdisonEngineering.Application.Interfaces;

namespace EdisonEngineering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMenu()
    {
        var result = await _menuService.GetMenuTreeAsync();
        return Ok(result);
    }
}