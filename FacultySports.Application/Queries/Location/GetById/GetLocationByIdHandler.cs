using AutoMapper;
using FluentResults;
using MediatR;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Application.Constants;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Contracts.Location;

namespace FacultySports.Application.Queries.Location.GetById;

public class GetLocationByIdHandler : IRequestHandler<GetLocationByIdQuery, Result<LocationDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetLocationByIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<LocationDto>> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repositoryWrapper.Locations.GetByIdAsync(request.Id);
        if (entity == null) return Result.Fail<LocationDto>(ErrorMessagesConstants.NotFound(request.Id, typeof(Entities.Location)));
        var dto = _mapper.Map<LocationDto>(entity);
        return Result.Ok(dto);
    }
}
