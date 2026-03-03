using AutoMapper;
using FluentResults;
using MediatR;
using FacultySports.Contracts.Competition;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Application.Constants;
using Entities = FacultySports.Domain.Entities;

namespace FacultySports.Application.Queries.Competition.GetById;

public class GetCompetitionByIdHandler : IRequestHandler<GetCompetitionByIdQuery, Result<CompetitionDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetCompetitionByIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<CompetitionDto>> Handle(GetCompetitionByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repositoryWrapper.Competitions.GetByIdAsync(request.Id);
        if (entity == null) return Result.Fail<CompetitionDto>(ErrorMessagesConstants.NotFound(request.Id, typeof(Entities.Competition)));

        var dto = _mapper.Map<CompetitionDto>(entity);
        return Result.Ok(dto);
    }
}
