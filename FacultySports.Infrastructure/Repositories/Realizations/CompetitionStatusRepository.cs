using FacultySports.Domain.Entities;
using FacultySports.Infrastructure.Context;
using FacultySports.Infrastructure.Repositories.Interfaces;
using FacultySports.Infrastructure.Repositories.Realizations.Base;

namespace FacultySports.Infrastructure.Repositories.Realizations;

public class CompetitionStatusRepository : Repository<CompetitionStatus>, ICompetitionStatusRepository
{
    public CompetitionStatusRepository(SportsDbContext context) : base(context)
    {
    }
}
