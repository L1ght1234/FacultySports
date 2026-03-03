using FluentValidation;

namespace FacultySports.Application.Commands.Participant.Delete;

public class DeleteParticipantValidator : AbstractValidator<DeleteParticipantCommand>
{
    public DeleteParticipantValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
