using FluentValidation;

namespace FacultySports.Application.Commands.Section.Update;

public class UpdateSectionValidator : AbstractValidator<UpdateSectionCommand>
{
    public UpdateSectionValidator()
    {
        RuleFor(x => x.UpdateSectionDto).NotNull();
        RuleFor(x => x.UpdateSectionDto.Id).GreaterThan(0);
        RuleFor(x => x.UpdateSectionDto.Name).NotEmpty().MaximumLength(255);
    }
}
