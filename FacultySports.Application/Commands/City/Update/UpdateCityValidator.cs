using FluentValidation;

namespace FacultySports.Application.Commands.City.Update;

public class UpdateCityValidator : AbstractValidator<UpdateCityCommand>
{
    public UpdateCityValidator()
    {
        RuleFor(x => x.UpdateCityDto).NotNull();
        RuleFor(x => x.UpdateCityDto.Id).GreaterThan(0);
        RuleFor(x => x.UpdateCityDto.Name).NotEmpty().MaximumLength(100);
    }
}
