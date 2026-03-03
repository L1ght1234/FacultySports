using FacultySports.Contracts.Competition;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Competition.Create;

public record CreateCompetitionCommand(CreateCompetitionDto CreateCompetitionDto) : IRequest<Result<CompetitionDto>>;
