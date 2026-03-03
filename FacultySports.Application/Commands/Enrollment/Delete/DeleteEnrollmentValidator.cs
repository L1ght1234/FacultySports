using FluentValidation;

namespace FacultySports.Application.Commands.Enrollment.Delete;

public class DeleteEnrollmentValidator : AbstractValidator<DeleteEnrollmentCommand>
{
    public DeleteEnrollmentValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
