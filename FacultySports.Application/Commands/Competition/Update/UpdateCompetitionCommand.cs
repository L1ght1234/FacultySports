using FacultySports.Contracts.Competition;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Competition.Update;

public record UpdateCompetitionCommand(UpdateCompetitionDto UpdateCompetitionDto) : IRequest<Result<CompetitionDto>>;
