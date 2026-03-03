using FluentValidation;

namespace FacultySports.Application.Commands.CompetitionStatus.Create;

public class CreateCompetitionStatusValidator : AbstractValidator<CreateCompetitionStatusCommand>
{
    public CreateCompetitionStatusValidator()
    {
        RuleFor(x => x.CreateCompetitionStatusDto.Code).NotEmpty();
        RuleFor(x => x.CreateCompetitionStatusDto.Name).NotEmpty();
    }
}
