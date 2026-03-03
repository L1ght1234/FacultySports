using FluentValidation;

namespace FacultySports.Application.Commands.City.Delete;

public class DeleteCityValidator : AbstractValidator<DeleteCityCommand>
{
    public DeleteCityValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}