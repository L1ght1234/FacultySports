using FluentValidation;

namespace FacultySports.Application.Commands.Location.Create;

public class CreateLocationValidator : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationValidator()
    {
        RuleFor(x => x.CreateLocationDto).NotNull();
        RuleFor(x => x.CreateLocationDto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CreateLocationDto.CityId).NotEmpty().GreaterThan(0);
    }
}
