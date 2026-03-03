using AutoMapper;
using FluentResults;
using MediatR;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Contracts.City;
using FacultySports.Application.Constants;
using Entities = FacultySports.Domain.Entities;

namespace FacultySports.Application.Queries.City.GetById;

public class GetCityByIdHandler : IRequestHandler<GetCityByIdQuery, Result<CityDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetCityByIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<CityDto>> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repositoryWrapper.Cities.GetByIdAsync(request.Id);
        if (entity == null) return Result.Fail<CityDto>(ErrorMessagesConstants.NotFound(request.Id, typeof(Entities.City)));
        var dto = _mapper.Map<CityDto>(entity);
        return Result.Ok(dto);
    }
}
