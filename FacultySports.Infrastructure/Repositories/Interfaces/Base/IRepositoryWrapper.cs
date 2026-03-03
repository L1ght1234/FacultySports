using System.Threading.Tasks;

namespace FacultySports.Infrastructure.Repositories.Interfaces.Base;

public interface IRepositoryWrapper
{
    ICityRepository Cities { get; }
    ICompetitionRepository Competitions { get; }
    ICompetitionStatusRepository CompetitionStatuses { get; }
    IEnrollmentRepository Enrollments { get; }
    ILocationRepository Locations { get; }
    IParticipantRepository Participants { get; }
    IScheduleRepository Schedules { get; }
    ISectionRepository Sections { get; }

    Task<int> SaveAsync();
}
