using FacultySports.Domain.Entities;
using FacultySports.Infrastructure.Context;
using FacultySports.Infrastructure.Repositories.Interfaces;
using FacultySports.Infrastructure.Repositories.Realizations.Base;
using Microsoft.EntityFrameworkCore;

namespace FacultySports.Infrastructure.Repositories.Realizations;

public class CompetitionRepository : Repository<Competition>, ICompetitionRepository
{
    public CompetitionRepository(SportsDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Competition>> GetAllAsync()
    {
        return await _dbSet
            .Include(c => c.Section)
            .Include(c => c.Location)
            .Include(c => c.Status)
            .ToListAsync();
    }

    public override async Task<Competition?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Section)
            .Include(c => c.Location)
            .Include(c => c.Status)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
