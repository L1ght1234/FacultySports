using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Contracts.Competition;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.Competition.Delete;

public class DeleteCompetitionHandler : IRequestHandler<DeleteCompetitionCommand, Result<CompetitionDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<DeleteCompetitionCommand> _validator;

    public DeleteCompetitionHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<DeleteCompetitionCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<CompetitionDto>> Handle(DeleteCompetitionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var existing = await _repositoryWrapper.Competitions.GetByIdAsync(request.Id);
            if (existing == null) return Result.Fail<CompetitionDto>(ErrorMessagesConstants
                    .NotFound(request.Id, typeof(Entities.Competition)));

            await _repositoryWrapper.Competitions.DeleteAsync(existing);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<CompetitionDto>(existing);
                return Result.Ok(dto);
            }

            return Result.Fail<CompetitionDto>(ErrorMessagesConstants.FailedToDeleteEntity(typeof(Entities.Competition)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<CompetitionDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<CompetitionDto>(ErrorMessagesConstants.FailedToDeleteEntityInDatabase(typeof(Entities.Competition)));
        }
    }
}
