using Microsoft.AspNetCore.Mvc;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Application.Common;
using EdisonEngineering.Application.DTOs;

namespace EdisonEngineering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;
    private readonly ILogger<MenuController> _logger;

    public MenuController(
        IMenuService menuService,
        ILogger<MenuController> logger)
    {
        _menuService = menuService;
        _logger = logger;
    }

    // GET: /api/menu
    [HttpGet]
    public async Task<IActionResult> GetMenu()
    {
        _logger.LogInformation("Fetching menu tree");

        var result = await _menuService.GetMenuTreeAsync();

        if (result == null || !result.Any())
        {
            _logger.LogWarning("Menu data not found");

            return NotFound(new ApiResponse<List<MenuDto>>
            {
                Success = false,
                Message = "Menu data not found",
                Data = new List<MenuDto>()
            });
        }

        _logger.LogInformation(
            "Menu fetched successfully. Count: {Count}",
            result.Count());

        return Ok(new ApiResponse<IEnumerable<MenuDto>>
        {
            Success = true,
            Message = "Menu fetched successfully",
            Data = result
        });
    }
}