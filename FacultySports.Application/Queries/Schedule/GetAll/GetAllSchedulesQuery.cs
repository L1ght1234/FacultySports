using FluentResults;
using MediatR;
using FacultySports.Contracts.Schedule;

namespace FacultySports.Application.Queries.Schedule.GetAll;

public record GetAllSchedulesQuery() : IRequest<Result<IEnumerable<ScheduleDto>>>;
