using FluentValidation;

namespace FacultySports.Application.Commands.Location.Delete;

public class DeleteLocationValidator : AbstractValidator<DeleteLocationCommand>
{
    public DeleteLocationValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}