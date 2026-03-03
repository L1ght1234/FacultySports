using AutoMapper;
using FacultySports.Contracts.Competition;
using FacultySports.Domain.Entities;

namespace FacultySports.Application.Mappings;

public class CompetitionMappingProfile : Profile
{
    public CompetitionMappingProfile()
    {
        CreateMap<CreateCompetitionDto, Competition>();
        CreateMap<UpdateCompetitionDto, Competition>();
        CreateMap<Competition, CompetitionDto>()
            .ForMember(dest => dest.SectionName, opt => opt.MapFrom(src => src.Section.Name))
            .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name));
    }
}
