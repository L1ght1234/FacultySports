using FacultySports.Contracts.CompetitionStatus;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.CompetitionStatus.Update;

public record UpdateCompetitionStatusCommand(UpdateCompetitionStatusDto UpdateCompetitionStatusDto) : IRequest<Result<CompetitionStatusDto>>;
