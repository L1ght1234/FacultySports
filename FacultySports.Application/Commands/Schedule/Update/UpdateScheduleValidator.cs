using FluentValidation;

namespace FacultySports.Application.Commands.Schedule.Update;

public class UpdateScheduleValidator : AbstractValidator<UpdateScheduleCommand>
{
    public UpdateScheduleValidator()
    {
        RuleFor(x => x.UpdateScheduleDto.Id).GreaterThan(0);
        RuleFor(x => x.UpdateScheduleDto.DayOfWeek).InclusiveBetween(0, 6);
        RuleFor(x => x.UpdateScheduleDto.StartTime).NotEmpty();
        RuleFor(x => x.UpdateScheduleDto.EndTime).NotEmpty();
    }
}
