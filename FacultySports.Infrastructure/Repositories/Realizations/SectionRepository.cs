using FacultySports.Domain.Entities;
using FacultySports.Infrastructure.Context;
using FacultySports.Infrastructure.Repositories.Interfaces;
using FacultySports.Infrastructure.Repositories.Realizations.Base;

namespace FacultySports.Infrastructure.Repositories.Realizations;

public class SectionRepository : Repository<Section>, ISectionRepository
{
    public SectionRepository(SportsDbContext context) : base(context)
    {
    }
}
