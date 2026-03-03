using AutoMapper;
using FluentResults;
using MediatR;
using FacultySports.Contracts.CompetitionStatus;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Application.Constants;
using Entities = FacultySports.Domain.Entities;

namespace FacultySports.Application.Queries.CompetitionStatus.GetById;

public class GetCompetitionStatusByIdHandler : IRequestHandler<GetCompetitionStatusByIdQuery, Result<CompetitionStatusDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetCompetitionStatusByIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<CompetitionStatusDto>> Handle(GetCompetitionStatusByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repositoryWrapper.CompetitionStatuses.GetByIdAsync(request.Id);
        if (entity == null) return Result.Fail<CompetitionStatusDto>(ErrorMessagesConstants.NotFound(request.Id, typeof(Entities.CompetitionStatus)));

        var dto = _mapper.Map<CompetitionStatusDto>(entity);
        return Result.Ok(dto);
    }
}
