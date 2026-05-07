using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EdisonEngineering.Application.Services;

public class JobService : IJobService
{
    private readonly IJobRepository _jobRepo;
    private readonly IJobApplicationRepository _appRepo;
    private readonly ILogger<JobService> _logger;

    public JobService(
        IJobRepository jobRepo,
        IJobApplicationRepository appRepo,
        ILogger<JobService> logger)
    {
        _jobRepo = jobRepo;
        _appRepo = appRepo;
        _logger = logger;
    }

    public async Task<List<JobDto>> GetJobsAsync()
    {
        _logger.LogInformation("Fetching active jobs from repository");

        var jobs = await _jobRepo.GetAllActiveAsync();

        if (jobs == null || !jobs.Any())
        {
            _logger.LogWarning("No active jobs found");

            return new List<JobDto>();
        }

        var result = jobs.Select(j => new JobDto
        {
            Id = j.Id,
            Title = j.Title,
            Department = j.Department,
            Location = j.Location,
            Description = j.Description
        }).ToList();

        _logger.LogInformation(
            "Jobs mapped successfully. Count: {Count}",
            result.Count);

        return result;
    }

    public async Task ApplyAsync(ApplyJobDto dto)
    {
        _logger.LogInformation(
            "Job application received for email: {Email}",
            dto.Email);

        var app = new JobApplication
        {
            JobId = dto.JobId,
            Name = dto.Name,
            Email = dto.Email,
            Mobile = dto.Mobile,
            ResumeUrl = dto.ResumeUrl
        };

        await _appRepo.AddAsync(app);

        _logger.LogInformation(
            "Job application saved successfully for email: {Email}",
            dto.Email);
    }
}