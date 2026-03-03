using AutoMapper;
using FacultySports.Contracts.Participant;
using FacultySports.MVC.Models.Participant;

namespace FacultySports.MVC.Mappings.Participant;

public class ParticipantViewMappingProfile : Profile
{
    public ParticipantViewMappingProfile()
    {
        CreateMap<ParticipantDto, ParticipantViewModel>().ReverseMap();
        CreateMap<CreateParticipantViewModel, CreateParticipantDto>().ReverseMap();
        CreateMap<CreateParticipantViewModel, UpdateParticipantDto>().ReverseMap();
        CreateMap<UpdateParticipantDto, ParticipantDto>().ReverseMap();
        CreateMap<ParticipantDto, CreateParticipantViewModel>().ReverseMap();
    }
}
