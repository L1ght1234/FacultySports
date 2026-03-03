using AutoMapper;
using FacultySports.Contracts.Location;
using FacultySports.Domain.Entities;

namespace FacultySports.Application.Mappings;

public class LocationMappingProfile : Profile
{
    public LocationMappingProfile()
    {
        CreateMap<CreateLocationDto, Location>();
        CreateMap<UpdateLocationDto, Location>();
        CreateMap<Location, LocationDto>();
    }
}
