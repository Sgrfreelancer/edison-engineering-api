using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Domain.Entities;
using EdisonEngineering.Infrastructure.Persistence;

namespace EdisonEngineering.Infrastructure.Repositories;

public class LeadRepository : ILeadRepository
{
    private readonly AppDbContext _context;

    public LeadRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Lead lead)
    {
        await _context.Leads.AddAsync(lead);
        await _context.SaveChangesAsync();
    }
}