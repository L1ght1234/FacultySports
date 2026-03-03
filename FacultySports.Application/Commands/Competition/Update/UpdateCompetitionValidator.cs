using FluentValidation;

namespace FacultySports.Application.Commands.Competition.Update;

public class UpdateCompetitionValidator : AbstractValidator<UpdateCompetitionCommand>
{
    public UpdateCompetitionValidator()
    {
        RuleFor(x => x.UpdateCompetitionDto.Id).GreaterThan(0);
        RuleFor(x => x.UpdateCompetitionDto.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.UpdateCompetitionDto.Date).NotEmpty();
        RuleFor(x => x.UpdateCompetitionDto.StartTime).NotEmpty();
        RuleFor(x => x.UpdateCompetitionDto.LocationId).GreaterThan(0);
        RuleFor(x => x.UpdateCompetitionDto.SectionId).GreaterThan(0);
    }
}
