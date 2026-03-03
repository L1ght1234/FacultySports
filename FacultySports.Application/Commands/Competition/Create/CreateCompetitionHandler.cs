using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.Competition;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.Competition.Create;

public class CreateCompetitionHandler : IRequestHandler<CreateCompetitionCommand, Result<CompetitionDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<CreateCompetitionCommand> _validator;

    public CreateCompetitionHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<CreateCompetitionCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<CompetitionDto>> Handle(CreateCompetitionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            Entities.Competition entity = _mapper.Map<Entities.Competition>(request.CreateCompetitionDto);
            await _repositoryWrapper.Competitions.AddAsync(entity);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<CompetitionDto>(entity);
                return Result.Ok(dto);
            }

            return Result.Fail<CompetitionDto>(ErrorMessagesConstants.FailedToCreateEntity(typeof(Entities.Competition)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<CompetitionDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<CompetitionDto>(ErrorMessagesConstants.FailedToCreateEntityInDatabase(typeof(Entities.Competition)));
        }
    }
}
