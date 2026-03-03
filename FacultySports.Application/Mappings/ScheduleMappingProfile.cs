using AutoMapper;
using FacultySports.Contracts.Schedule;
using FacultySports.Domain.Entities;

namespace FacultySports.Application.Mappings;

public class ScheduleMappingProfile : Profile
{
    public ScheduleMappingProfile()
    {
        CreateMap<CreateScheduleDto, Schedule>();
        CreateMap<UpdateScheduleDto, Schedule>();
        CreateMap<Schedule, ScheduleDto>();
    }
}
