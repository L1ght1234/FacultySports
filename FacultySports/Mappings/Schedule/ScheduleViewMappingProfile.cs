using AutoMapper;
using FacultySports.Contracts.Schedule;
using FacultySports.MVC.Models.Schedule;

namespace FacultySports.MVC.Mappings.Schedule;

public class ScheduleViewMappingProfile : Profile
{
    public ScheduleViewMappingProfile()
    {
        CreateMap<ScheduleDto, ScheduleViewModel>().ReverseMap();
        CreateMap<CreateScheduleViewModel, CreateScheduleDto>().ReverseMap();
        CreateMap<CreateScheduleViewModel, UpdateScheduleDto>().ReverseMap();
        CreateMap<UpdateScheduleDto, ScheduleDto>().ReverseMap();
        CreateMap<ScheduleDto, CreateScheduleViewModel>().ReverseMap();
    }
}
