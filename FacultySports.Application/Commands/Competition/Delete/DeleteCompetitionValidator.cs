using FluentValidation;

namespace FacultySports.Application.Commands.Competition.Delete;

public class DeleteCompetitionValidator : AbstractValidator<DeleteCompetitionCommand>
{
    public DeleteCompetitionValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}