using AutoMapper;
using FluentResults;
using MediatR;
using FacultySports.Contracts.CompetitionStatus;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;

namespace FacultySports.Application.Queries.CompetitionStatus.GetAll;

public class GetAllCompetitionStatusesHandler : IRequestHandler<GetAllCompetitionStatusesQuery, Result<IEnumerable<CompetitionStatusDto>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllCompetitionStatusesHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<IEnumerable<CompetitionStatusDto>>> Handle(GetAllCompetitionStatusesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repositoryWrapper.CompetitionStatuses.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<CompetitionStatusDto>>(entities);
        return Result.Ok(dtos);
    }
}
