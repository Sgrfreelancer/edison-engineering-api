using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;
using EdisonEngineering.Infrastructure.Persistence;

public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly AppDbContext _context;

    public JobApplicationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(JobApplication application)
    {
        _context.JobApplications.Add(application);
        await _context.SaveChangesAsync();
    }
}