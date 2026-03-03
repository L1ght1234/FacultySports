using FluentValidation;

namespace FacultySports.Application.Commands.Section.Create;

public class CreateSectionValidator : AbstractValidator<CreateSectionCommand>
{
    public CreateSectionValidator()
    {
        RuleFor(x => x.CreateSectionDto).NotNull();
        RuleFor(x => x.CreateSectionDto.Name).NotEmpty().MaximumLength(255);
    }
}
