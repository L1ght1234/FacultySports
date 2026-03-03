using FacultySports.Contracts.CompetitionStatus;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.CompetitionStatus.Create;

public record CreateCompetitionStatusCommand(CreateCompetitionStatusDto CreateCompetitionStatusDto) : IRequest<Result<CompetitionStatusDto>>;
