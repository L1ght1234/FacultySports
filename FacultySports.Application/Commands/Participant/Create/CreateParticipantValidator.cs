using FluentValidation;

namespace FacultySports.Application.Commands.Participant.Create;

public class CreateParticipantValidator : AbstractValidator<CreateParticipantCommand>
{
    public CreateParticipantValidator()
    {
        RuleFor(x => x.CreateParticipantDto).NotNull();
        RuleFor(x => x.CreateParticipantDto.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CreateParticipantDto.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CreateParticipantDto.Phone).NotEmpty().Matches(@"^\d+$");
    }
}
