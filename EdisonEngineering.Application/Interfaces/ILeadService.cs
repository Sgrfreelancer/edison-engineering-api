using EdisonEngineering.Application.DTOs;

public interface ILeadService
{
    Task<bool> CreateAsync(CreateLeadDto dto);
}