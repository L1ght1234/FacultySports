using AutoMapper;
using FacultySports.Contracts.Enrollment;
using FacultySports.MVC.Models.Enrollment;

namespace FacultySports.MVC.Mappings.Enrollment;

public class EnrollmentViewMappingProfile : Profile
{
    public EnrollmentViewMappingProfile()
    {
        CreateMap<EnrollmentDto, EnrollmentViewModel>()
            .ForMember(d => d.ParticipantName, opt => opt.Ignore())
            .ForMember(d => d.SectionName, opt => opt.Ignore());

        CreateMap<EnrollmentDto, CreateEnrollmentViewModel>().ReverseMap();
    }
}
