using FluentValidation;

namespace FacultySports.Application.Commands.Enrollment.Update;

public class UpdateEnrollmentValidator : AbstractValidator<UpdateEnrollmentCommand>
{
    public UpdateEnrollmentValidator()
    {
        RuleFor(x => x.UpdateEnrollmentDto).NotNull();
        RuleFor(x => x.UpdateEnrollmentDto.Id).GreaterThan(0);
        RuleFor(x => x.UpdateEnrollmentDto.SectionId).GreaterThan(0);
        RuleFor(x => x.UpdateEnrollmentDto.ParticipantId).GreaterThan(0);
        RuleFor(x => x.UpdateEnrollmentDto.Date).NotNull();
    }
}