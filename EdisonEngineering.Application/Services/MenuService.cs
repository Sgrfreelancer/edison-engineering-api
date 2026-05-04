using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;

namespace EdisonEngineering.Application.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;

    public MenuService(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<List<MenuDto>> GetMenuTreeAsync()
    {
        var menus = await _menuRepository.GetAllAsync();

        var menuDict = menus.ToDictionary(m => m.Id);

        var result = new List<MenuDto>();

        foreach (var menu in menus)
        {
            if (menu.ParentId == null)
            {
                result.Add(MapToDto(menu, menus));
            }
        }

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