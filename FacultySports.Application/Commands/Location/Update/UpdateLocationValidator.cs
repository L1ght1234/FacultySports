using FluentValidation;

namespace FacultySports.Application.Commands.Location.Update;

public class UpdateLocationValidator : AbstractValidator<UpdateLocationCommand>
{
    public UpdateLocationValidator()
    {
        RuleFor(x => x.UpdateLocationDto).NotNull();
        RuleFor(x => x.UpdateLocationDto.Id).GreaterThan(0);
        RuleFor(x => x.UpdateLocationDto.Name).NotEmpty().MaximumLength(200);
    }
}
