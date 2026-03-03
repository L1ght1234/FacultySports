using AutoMapper;
using FluentResults;
using MediatR;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Contracts.Participant;

namespace FacultySports.Application.Queries.Participant.GetAll;

public class GetAllParticipantsHandler : IRequestHandler<GetAllParticipantsQuery, Result<IEnumerable<ParticipantDto>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllParticipantsHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<IEnumerable<ParticipantDto>>> Handle(GetAllParticipantsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repositoryWrapper.Participants.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<ParticipantDto>>(entities);
        return Result.Ok(dtos);
    }
}
