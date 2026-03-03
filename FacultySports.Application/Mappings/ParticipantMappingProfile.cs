using AutoMapper;
using FacultySports.Contracts.Participant;
using FacultySports.Contracts.Enrollment;
using FacultySports.Domain.Entities;

namespace FacultySports.Application.Mappings;

public class ParticipantMappingProfile : Profile
{
    public ParticipantMappingProfile()
    {
        CreateMap<CreateParticipantDto, Participant>();
        CreateMap<UpdateParticipantDto, Participant>();
        CreateMap<Participant, ParticipantDto>();

        CreateMap<CreateEnrollmentDto, Enrollment>();
        CreateMap<Enrollment, EnrollmentDto>();
    }
}
