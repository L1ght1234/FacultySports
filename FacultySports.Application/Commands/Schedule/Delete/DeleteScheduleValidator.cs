using FluentValidation;

namespace FacultySports.Application.Commands.Schedule.Delete;

public class DeleteScheduleValidator : AbstractValidator<DeleteScheduleCommand>
{
    public DeleteScheduleValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
