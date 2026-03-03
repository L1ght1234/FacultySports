using FluentResults;
using MediatR;
using FacultySports.Contracts.Competition;

namespace FacultySports.Application.Queries.Competition.GetAll;

public record GetAllCompetitionsQuery() : IRequest<Result<IEnumerable<CompetitionDto>>>;
