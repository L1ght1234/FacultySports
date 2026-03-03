using FluentValidation;

namespace FacultySports.Application.Commands.Section.Delete;

public class DeleteSectionValidator : AbstractValidator<DeleteSectionCommand>
{
    public DeleteSectionValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
