using FacultySports.Contracts.Schedule;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Schedule.Update;

public record UpdateScheduleCommand(UpdateScheduleDto UpdateScheduleDto) : IRequest<Result<ScheduleDto>>;
