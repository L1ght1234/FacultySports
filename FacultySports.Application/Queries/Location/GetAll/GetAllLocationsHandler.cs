using AutoMapper;
using FluentResults;
using MediatR;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Contracts.Location;

namespace FacultySports.Application.Queries.Location.GetAll;

public class GetAllLocationsHandler : IRequestHandler<GetAllLocationsQuery, Result<IEnumerable<LocationDto>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllLocationsHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<IEnumerable<LocationDto>>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repositoryWrapper.Locations.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<LocationDto>>(entities);
        return Result.Ok(dtos);
    }
}
