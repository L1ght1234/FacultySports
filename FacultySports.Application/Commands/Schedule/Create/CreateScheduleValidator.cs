using FluentValidation;

namespace FacultySports.Application.Commands.Schedule.Create;

public class CreateScheduleValidator : AbstractValidator<CreateScheduleCommand>
{
    public CreateScheduleValidator()
    {
        RuleFor(x => x.CreateScheduleDto.DayOfWeek).InclusiveBetween(0, 6);
        RuleFor(x => x.CreateScheduleDto.StartTime).NotEmpty();
        RuleFor(x => x.CreateScheduleDto.EndTime).NotEmpty();
    }
}
