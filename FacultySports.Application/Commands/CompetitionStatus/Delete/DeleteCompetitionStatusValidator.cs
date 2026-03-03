using FluentValidation;

namespace FacultySports.Application.Commands.CompetitionStatus.Delete;

public class DeleteCompetitionStatusValidator : AbstractValidator<DeleteCompetitionStatusCommand>
{
    public DeleteCompetitionStatusValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}