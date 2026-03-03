using FluentValidation;

namespace FacultySports.Application.Commands.Participant.Update;

public class UpdateParticipantValidator : AbstractValidator<UpdateParticipantCommand>
{
    public UpdateParticipantValidator()
    {
        RuleFor(x => x.UpdateParticipantDto).NotNull();
        RuleFor(x => x.UpdateParticipantDto.Id).GreaterThan(0);
        RuleFor(x => x.UpdateParticipantDto.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.UpdateParticipantDto.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.UpdateParticipantDto.Phone).NotEmpty().Matches(@"^\d+$");
    }
}
