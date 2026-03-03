using FacultySports.Contracts.Participant;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Participant.Update;

public record UpdateParticipantCommand(UpdateParticipantDto UpdateParticipantDto)
    : IRequest<Result<ParticipantDto>>;
