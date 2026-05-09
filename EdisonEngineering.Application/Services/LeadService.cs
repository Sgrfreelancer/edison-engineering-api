using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EdisonEngineering.Application.Services;

public class LeadService : ILeadService
{
    private readonly ILeadRepository _repo;
    private readonly ILogger<LeadService> _logger;

    public LeadService(
        ILeadRepository repo,
        ILogger<LeadService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<bool> CreateAsync(CreateLeadDto dto)
    {
        _logger.LogInformation(
            "Lead creation request received for phone: {Phone}",
            dto.Phone);

        // Basic Validation
        // if (string.IsNullOrWhiteSpace(dto.Name) ||
        //     string.IsNullOrWhiteSpace(dto.Phone) ||
        //     string.IsNullOrWhiteSpace(dto.City) ||
        //     string.IsNullOrWhiteSpace(dto.ServiceType))
        // {
        //     _logger.LogWarning(
        //         "Lead validation failed for phone: {Phone}",
        //         dto.Phone);

        //     return false;
        // }

        var lead = new Lead
        {
            Name = dto.Name,
            Phone = dto.Phone,
            Email = dto.Email,
            City = dto.City,
            ServiceType = dto.ServiceType,
            Message = dto.Message,
            Source = dto.Source
        };

        await _repo.AddAsync(lead);

        _logger.LogInformation(
            "Lead created successfully for phone: {Phone}",
            dto.Phone);

        return true;
    }
}