using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Contracts.CompetitionStatus;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.CompetitionStatus.Delete;

public class DeleteCompetitionStatusHandler : IRequestHandler<DeleteCompetitionStatusCommand, Result<CompetitionStatusDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<DeleteCompetitionStatusCommand> _validator;

    public DeleteCompetitionStatusHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<DeleteCompetitionStatusCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<CompetitionStatusDto>> Handle(DeleteCompetitionStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var existing = await _repositoryWrapper.CompetitionStatuses.GetByIdAsync(request.Id);
            if (existing == null) return Result.Fail<CompetitionStatusDto>(ErrorMessagesConstants
                    .NotFound(request.Id, typeof(Entities.CompetitionStatus)));

            await _repositoryWrapper.CompetitionStatuses.DeleteAsync(existing);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<CompetitionStatusDto>(existing);
                return Result.Ok(dto);
            }

            return Result.Fail<CompetitionStatusDto>(ErrorMessagesConstants.FailedToDeleteEntity(typeof(Entities.CompetitionStatus)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<CompetitionStatusDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<CompetitionStatusDto>(ErrorMessagesConstants.FailedToDeleteEntityInDatabase(typeof(Entities.CompetitionStatus)));
        }
    }
}
