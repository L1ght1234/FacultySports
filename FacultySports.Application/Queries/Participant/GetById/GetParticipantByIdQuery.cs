using FluentResults;
using MediatR;
using FacultySports.Contracts.Participant;

namespace FacultySports.Application.Queries.Participant.GetById;

public record GetParticipantByIdQuery(int Id) : IRequest<Result<ParticipantDto>>;
