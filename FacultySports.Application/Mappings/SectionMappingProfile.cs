using AutoMapper;
using FacultySports.Contracts.Section;
using FacultySports.Domain.Entities;

namespace FacultySports.Application.Mappings;

public class SectionMappingProfile : Profile
{
    public SectionMappingProfile()
    {
        CreateMap<CreateSectionDto, Section>();
        CreateMap<UpdateSectionDto, Section>();
        CreateMap<Section, SectionDto>();
    }
}
