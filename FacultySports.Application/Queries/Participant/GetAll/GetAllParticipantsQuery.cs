using FluentResults;
using MediatR;
using FacultySports.Contracts.Participant;

namespace FacultySports.Application.Queries.Participant.GetAll;

public record GetAllParticipantsQuery() : IRequest<Result<IEnumerable<ParticipantDto>>>;
