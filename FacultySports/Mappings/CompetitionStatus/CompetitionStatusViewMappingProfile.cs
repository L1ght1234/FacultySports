using AutoMapper;
using FacultySports.Contracts.CompetitionStatus;
using FacultySports.MVC.Models.CompetitionStatus;

namespace FacultySports.MVC.Mappings.CompetitionStatus;

public class CompetitionStatusViewMappingProfile : Profile
{
    public CompetitionStatusViewMappingProfile()
    {
        CreateMap<CompetitionStatusDto, CompetitionStatusViewModel>().ReverseMap();
        CreateMap<CreateCompetitionStatusViewModel, CreateCompetitionStatusDto>().ReverseMap();
        CreateMap<CreateCompetitionStatusViewModel, UpdateCompetitionStatusDto>().ReverseMap();
        CreateMap<UpdateCompetitionStatusDto, CompetitionStatusDto>().ReverseMap();
        CreateMap<CompetitionStatusDto, CreateCompetitionStatusViewModel>().ReverseMap();
    }
}
