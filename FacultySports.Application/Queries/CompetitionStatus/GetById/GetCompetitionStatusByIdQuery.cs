using FluentResults;
using MediatR;
using FacultySports.Contracts.CompetitionStatus;

namespace FacultySports.Application.Queries.CompetitionStatus.GetById;

public record GetCompetitionStatusByIdQuery(int Id) : IRequest<Result<CompetitionStatusDto>>;
