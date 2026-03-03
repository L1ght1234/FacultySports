using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.CompetitionStatus;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Application.Constants;
using Entities = FacultySports.Domain.Entities;

namespace FacultySports.Application.Commands.CompetitionStatus.Update;

public class UpdateCompetitionStatusHandler : IRequestHandler<UpdateCompetitionStatusCommand, Result<CompetitionStatusDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<UpdateCompetitionStatusCommand> _validator;

    public UpdateCompetitionStatusHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<UpdateCompetitionStatusCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<CompetitionStatusDto>> Handle(UpdateCompetitionStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var entity = await _repositoryWrapper.CompetitionStatuses.GetByIdAsync(request.UpdateCompetitionStatusDto.Id);
            if (entity == null) return Result.Fail<CompetitionStatusDto>(ErrorMessagesConstants.NotFound(request.UpdateCompetitionStatusDto.Id, typeof(Entities.CompetitionStatus)));

            _mapper.Map(request.UpdateCompetitionStatusDto, entity);
            await _repositoryWrapper.CompetitionStatuses.UpdateAsync(entity);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<CompetitionStatusDto>(entity);
                return Result.Ok(dto);
            }
            return Result.Fail<CompetitionStatusDto>(ErrorMessagesConstants.FailedToUpdateEntity(typeof(Entities.CompetitionStatus)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<CompetitionStatusDto>(ErrorMessagesConstants.FailedToUpdateEntityInDatabase(typeof(Entities.CompetitionStatus)));
        }
    }
}
