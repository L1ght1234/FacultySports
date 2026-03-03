using FluentValidation;

namespace FacultySports.Application.Commands.Enrollment.Create;

public class CreateEnrollmentValidator : AbstractValidator<CreateEnrollmentCommand>
{
    public CreateEnrollmentValidator()
    {
        RuleFor(x => x.CreateEnrollmentDto).NotNull();
        RuleFor(x => x.CreateEnrollmentDto.SectionId).GreaterThan(0);
        RuleFor(x => x.CreateEnrollmentDto.ParticipantId).GreaterThan(0);
        RuleFor(x => x.CreateEnrollmentDto.Date).NotNull();
    }
}
