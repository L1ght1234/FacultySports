using AutoMapper;
using FacultySports.Contracts.Location;
using FacultySports.MVC.Models.Location;

namespace FacultySports.MVC.Mappings.Location;

public class LocationViewMappingProfile : Profile
{
    public LocationViewMappingProfile()
    {
        CreateMap<LocationDto, LocationViewModel>().ReverseMap();
        CreateMap<CreateLocationViewModel, CreateLocationDto>().ReverseMap();
        CreateMap<CreateLocationViewModel, UpdateLocationDto>().ReverseMap();
        CreateMap<UpdateLocationDto, LocationDto>().ReverseMap();
        CreateMap<LocationDto, CreateLocationViewModel>().ReverseMap();
    }
}
