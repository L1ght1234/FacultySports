using AutoMapper;
using FacultySports.Contracts.CompetitionStatus;
using FacultySports.Domain.Entities;

namespace FacultySports.Application.Mappings;

public class CompetitionStatusMappingProfile : Profile
{
    public CompetitionStatusMappingProfile()
    {
        CreateMap<CreateCompetitionStatusDto, CompetitionStatus>();
        CreateMap<UpdateCompetitionStatusDto, CompetitionStatus>();
        CreateMap<CompetitionStatus, CompetitionStatusDto>();
    }
}
