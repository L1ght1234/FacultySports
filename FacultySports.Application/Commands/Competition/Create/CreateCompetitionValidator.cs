using FluentValidation;

namespace FacultySports.Application.Commands.Competition.Create;

public class CreateCompetitionValidator : AbstractValidator<CreateCompetitionCommand>
{
    public CreateCompetitionValidator()
    {
        RuleFor(x => x.CreateCompetitionDto.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CreateCompetitionDto.Date).NotEmpty();
        RuleFor(x => x.CreateCompetitionDto.StartTime).NotEmpty();
        RuleFor(x => x.CreateCompetitionDto.StatusId).GreaterThan(0);
        RuleFor(x => x.CreateCompetitionDto.LocationId).GreaterThan(0);
        RuleFor(x => x.CreateCompetitionDto.SectionId).GreaterThan(0);
    }
}