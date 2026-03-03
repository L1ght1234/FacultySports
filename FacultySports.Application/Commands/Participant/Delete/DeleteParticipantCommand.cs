using FacultySports.Contracts.Participant;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Participant.Delete;

public record DeleteParticipantCommand(int Id) : IRequest<Result<ParticipantDto>>;
