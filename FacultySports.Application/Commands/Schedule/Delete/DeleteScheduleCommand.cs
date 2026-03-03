using FacultySports.Contracts.Schedule;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Schedule.Delete;

public record DeleteScheduleCommand(int Id) : IRequest<Result<ScheduleDto>>;
