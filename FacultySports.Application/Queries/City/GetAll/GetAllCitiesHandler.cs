using AutoMapper;
using FluentResults;
using MediatR;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Contracts.City;


namespace FacultySports.Application.Queries.City.GetAll;

public class GetAllCitiesHandler : IRequestHandler<GetAllCitiesQuery, Result<IEnumerable<CityDto>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllCitiesHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<IEnumerable<CityDto>>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repositoryWrapper.Cities.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<CityDto>>(entities);
        return Result.Ok(dtos);
    }
}
