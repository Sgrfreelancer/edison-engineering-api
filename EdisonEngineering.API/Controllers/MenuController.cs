using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Application.Common;
using EdisonEngineering.Application.DTOs;
using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;

namespace EdisonEngineering.API.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[EnableRateLimiting("fixed")]
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
    [OutputCache(
        PolicyName = "menu-cache")]
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
