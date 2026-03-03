using FluentResults;
using MediatR;
using FacultySports.Contracts.Competition;

namespace FacultySports.Application.Queries.Competition.GetById;

public record GetCompetitionByIdQuery(int Id) : IRequest<Result<CompetitionDto>>;
