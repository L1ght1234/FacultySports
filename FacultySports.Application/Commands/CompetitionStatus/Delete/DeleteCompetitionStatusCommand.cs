using FacultySports.Contracts.CompetitionStatus;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.CompetitionStatus.Delete;

public record DeleteCompetitionStatusCommand(int Id) : IRequest<Result<CompetitionStatusDto>>;
