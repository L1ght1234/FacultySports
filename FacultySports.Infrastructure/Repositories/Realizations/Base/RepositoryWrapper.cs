using System.Threading.Tasks;
using FacultySports.Infrastructure.Context;
using FacultySports.Infrastructure.Repositories.Interfaces;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;

namespace FacultySports.Infrastructure.Repositories.Realizations.Base;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly SportsDbContext _context;

    public ICityRepository Cities { get; }
    public ICompetitionRepository Competitions { get; }
    public ICompetitionStatusRepository CompetitionStatuses { get; }
    public IEnrollmentRepository Enrollments { get; }
    public ILocationRepository Locations { get; }
    public IParticipantRepository Participants { get; }
    public IScheduleRepository Schedules { get; }
    public ISectionRepository Sections { get; }

    public RepositoryWrapper(SportsDbContext context,
        ICityRepository cityRepo,
        ICompetitionRepository competitionRepo,
        ICompetitionStatusRepository competitionStatusRepo,
        IEnrollmentRepository enrollmentRepo,
        ILocationRepository locationRepo,
        IParticipantRepository participantRepo,
        IScheduleRepository scheduleRepo,
        ISectionRepository sectionRepo)
    {
        _context = context;
        Cities = cityRepo;
        Competitions = competitionRepo;
        CompetitionStatuses = competitionStatusRepo;
        Enrollments = enrollmentRepo;
        Locations = locationRepo;
        Participants = participantRepo;
        Schedules = scheduleRepo;
        Sections = sectionRepo;
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
