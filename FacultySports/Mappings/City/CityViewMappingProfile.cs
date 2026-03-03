using AutoMapper;
using FacultySports.Contracts.City;
using FacultySports.MVC.Models.City;

namespace FacultySports.MVC.Mappings.City;

public class CityViewMappingProfile : Profile
{
    public CityViewMappingProfile()
    {
        CreateMap<CityDto, CityViewModel>().ReverseMap();
        CreateMap<CreateCityViewModel, CreateCityDto>().ReverseMap();
        CreateMap<CreateCityViewModel, UpdateCityDto>().ReverseMap();
        CreateMap<UpdateCityDto, CityDto>().ReverseMap();
        CreateMap<CityDto, CreateCityViewModel>().ReverseMap();
    }
}
