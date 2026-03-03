using FluentResults;
using MediatR;
using FacultySports.Contracts.CompetitionStatus;

namespace FacultySports.Application.Queries.CompetitionStatus.GetAll;

public record GetAllCompetitionStatusesQuery() : IRequest<Result<IEnumerable<CompetitionStatusDto>>>;
