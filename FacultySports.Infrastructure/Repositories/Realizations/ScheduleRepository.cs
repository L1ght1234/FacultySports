using FacultySports.Domain.Entities;
using FacultySports.Infrastructure.Context;
using FacultySports.Infrastructure.Repositories.Interfaces;
using FacultySports.Infrastructure.Repositories.Realizations.Base;

namespace FacultySports.Infrastructure.Repositories.Realizations;

public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
{
    public ScheduleRepository(SportsDbContext context) : base(context)
    {
    }
}
