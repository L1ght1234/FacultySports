using AutoMapper;
using FluentResults;
using MediatR;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Application.Constants;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Contracts.Participant;

namespace FacultySports.Application.Queries.Participant.GetById;

public class GetParticipantByIdHandler : IRequestHandler<GetParticipantByIdQuery, Result<ParticipantDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetParticipantByIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<ParticipantDto>> Handle(GetParticipantByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repositoryWrapper.Participants.GetByIdAsync(request.Id);
        if (entity == null) return Result.Fail<ParticipantDto>(ErrorMessagesConstants.NotFound(request.Id, typeof(Entities.Participant)));

        var dto = _mapper.Map<ParticipantDto>(entity);
        return Result.Ok(dto);
    }
}
