using FluentResults;
using MediatR;
using FacultySports.Contracts.Competition;


namespace FacultySports.Application.Commands.Competition.Delete;

public record DeleteCompetitionCommand(int Id) : IRequest<Result<CompetitionDto>>;
