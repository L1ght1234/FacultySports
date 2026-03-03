using AutoMapper;
using FacultySports.Contracts.Section;
using FacultySports.MVC.Models.Section;

namespace FacultySports.MVC.Mappings.Section;

public class SectionViewMappingProfile : Profile
{
    public SectionViewMappingProfile()
    {
        CreateMap<SectionDto, SectionViewModel>();
        CreateMap<CreateSectionViewModel, CreateSectionDto>();
        CreateMap<SectionViewModel, UpdateSectionDto>();
        CreateMap<UpdateSectionDto, SectionViewModel>();
        CreateMap<SectionDto, CreateSectionViewModel>();
    }
}
