using FluentValidation;

namespace FacultySports.Application.Commands.CompetitionStatus.Update;

public class UpdateCompetitionStatusValidator : AbstractValidator<UpdateCompetitionStatusCommand>
{
    public UpdateCompetitionStatusValidator()
    {
        RuleFor(x => x.UpdateCompetitionStatusDto.Id).GreaterThan(0);
        RuleFor(x => x.UpdateCompetitionStatusDto.Code).NotEmpty();
        RuleFor(x => x.UpdateCompetitionStatusDto.Name).NotEmpty();
    }
}
