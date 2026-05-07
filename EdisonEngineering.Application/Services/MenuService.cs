using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EdisonEngineering.Application.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly ILogger<MenuService> _logger;

    public MenuService(
        IMenuRepository menuRepository,
        ILogger<MenuService> logger)
    {
        _menuRepository = menuRepository;
        _logger = logger;
    }

    public async Task<List<MenuDto>> GetMenuTreeAsync()
    {
        _logger.LogInformation("Fetching menu tree from repository");

        var menus = await _menuRepository.GetAllAsync();

        if (menus == null || !menus.Any())
        {
            _logger.LogWarning("No menu data found");

            return new List<MenuDto>();
        }

        var result = new List<MenuDto>();

        foreach (var menu in menus)
        {
            if (menu.ParentId == null)
            {
                result.Add(MapToDto(menu, menus));
            }
        }

        _logger.LogInformation(
            "Menu tree generated successfully. Root count: {Count}",
            result.Count);

        return result;
    }

    private MenuDto MapToDto(Menu menu, List<Menu> allMenus)
    {
        return new MenuDto
        {
            Id = menu.Id,
            Name = menu.Name,
            Slug = menu.Slug,
            Children = allMenus
                .Where(x => x.ParentId == menu.Id)
                .Select(child => MapToDto(child, allMenus))
                .ToList()
        };
    }
}