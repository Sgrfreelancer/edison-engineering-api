using EdisonEngineering.Application.DTOs;

namespace EdisonEngineering.Application.Interfaces;

public interface IMenuService
{
    Task<List<MenuDto>> GetMenuTreeAsync();
}