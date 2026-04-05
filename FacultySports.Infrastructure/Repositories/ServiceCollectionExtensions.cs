using Microsoft.Extensions.DependencyInjection;
using FacultySports.Infrastructure.Context;
using FacultySports.Infrastructure.Repositories.Realizations;
using FacultySports.Infrastructure.Repositories.Interfaces;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Infrastructure.Repositories.Realizations.Base;

namespace FacultySports.Infrastructure.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<ICompetitionRepository, CompetitionRepository>();
        services.AddScoped<ICompetitionStatusRepository, CompetitionStatusRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<ISectionRepository, SectionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

        return services;
    }
}
