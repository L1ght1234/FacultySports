using FluentValidation;

namespace FacultySports.Application.Commands.City.Create;

public class CreateCityValidator : AbstractValidator<CreateCityCommand>
{
    public CreateCityValidator()
    {
        RuleFor(x => x.CreateCityDto).NotNull();
        RuleFor(x => x.CreateCityDto.Name).NotEmpty().MaximumLength(100);
    }
}
