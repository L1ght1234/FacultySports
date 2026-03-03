using AutoMapper;
using FacultySports.Contracts.City;
using FacultySports.Domain.Entities;

namespace FacultySports.Application.Mappings;

public class CityMappingProfile : Profile
{
    public CityMappingProfile()
    {
        CreateMap<CreateCityDto, City>();
        CreateMap<UpdateCityDto, City>();
        CreateMap<City, CityDto>();
    }
}
