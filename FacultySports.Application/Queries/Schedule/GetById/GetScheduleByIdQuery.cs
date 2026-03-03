using FluentResults;
using MediatR;
using FacultySports.Contracts.Schedule;

namespace FacultySports.Application.Queries.Schedule.GetById;

public record GetScheduleByIdQuery(int Id) : IRequest<Result<ScheduleDto>>;
