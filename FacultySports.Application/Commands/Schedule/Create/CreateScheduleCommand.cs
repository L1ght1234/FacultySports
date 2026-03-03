using FacultySports.Contracts.Schedule;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Schedule.Create;

public record CreateScheduleCommand(CreateScheduleDto CreateScheduleDto) : IRequest<Result<ScheduleDto>>;
