using EdisonEngineering.Application.Common;
using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EdisonEngineering.Application.Services;

public class LeadService : ILeadService
{
    private readonly ILeadRepository _repo;
    private readonly ILogger<LeadService> _logger;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly EmailSettings _emailSettings;

    public LeadService(
        ILeadRepository repo,
        IBackgroundTaskQueue backgroundTaskQueue,
        IOptions<EmailSettings> emailSettings,
        ILogger<LeadService> logger)
    {
        _repo = repo;
        _backgroundTaskQueue = backgroundTaskQueue;
        _emailSettings = emailSettings.Value;
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

        _backgroundTaskQueue.QueueEmail(
            new EmailMessageDto
            {
                To = _emailSettings.AdminEmail,

                Subject =
                    "New Lead Received",

                Body =
$"""
<h2>New Lead Received</h2>

<p>
<b>Name:</b> {dto.Name}
</p>

<p>
<b>Phone:</b> {dto.Phone}
</p>

<p>
<b>Email:</b> {dto.Email}
</p>

<p>
<b>City:</b> {dto.City}
</p>

<p>
<b>Service:</b> {dto.ServiceType}
</p>

<p>
<b>Message:</b> {dto.Message}
</p>
"""
            });

        _logger.LogInformation(
            "Lead created successfully for phone: {Phone}",
            dto.Phone);

        return true;
    }
}