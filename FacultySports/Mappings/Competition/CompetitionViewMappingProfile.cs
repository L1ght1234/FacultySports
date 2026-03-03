using AutoMapper;
using FacultySports.Contracts.Competition;
using FacultySports.MVC.Models.Competition;

namespace FacultySports.MVC.Mappings.Competition;

public class CompetitionViewMappingProfile : Profile
{
    public CompetitionViewMappingProfile()
    {
        CreateMap<CompetitionDto, CompetitionViewModel>().ReverseMap();
        CreateMap<CreateCompetitionViewModel, CreateCompetitionDto>().ReverseMap();
        CreateMap<CreateCompetitionViewModel, UpdateCompetitionDto>().ReverseMap();
        CreateMap<UpdateCompetitionDto, CompetitionDto>().ReverseMap();
        CreateMap<CompetitionDto, CreateCompetitionViewModel>().ReverseMap();
    }
}
