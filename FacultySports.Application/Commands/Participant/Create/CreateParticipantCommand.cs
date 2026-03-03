using FacultySports.Contracts.Participant;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Participant.Create;

public record CreateParticipantCommand(CreateParticipantDto CreateParticipantDto)
    : IRequest<Result<ParticipantDto>>;
