using AutoMapper;
using FluentResults;
using MediatR;
using FacultySports.Contracts.Competition;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;

namespace FacultySports.Application.Queries.Competition.GetAll;

public class GetAllCompetitionsHandler : IRequestHandler<GetAllCompetitionsQuery, Result<IEnumerable<CompetitionDto>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllCompetitionsHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<IEnumerable<CompetitionDto>>> Handle(GetAllCompetitionsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repositoryWrapper.Competitions.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<CompetitionDto>>(entities);
        return Result.Ok(dtos);
    }
}
