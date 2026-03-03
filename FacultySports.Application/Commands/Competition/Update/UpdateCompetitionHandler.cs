using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.Competition;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Application.Constants;
using Entities = FacultySports.Domain.Entities;

namespace FacultySports.Application.Commands.Competition.Update;

public class UpdateCompetitionHandler : IRequestHandler<UpdateCompetitionCommand, Result<CompetitionDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<UpdateCompetitionCommand> _validator;

    public UpdateCompetitionHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<UpdateCompetitionCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<CompetitionDto>> Handle(UpdateCompetitionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var entity = await _repositoryWrapper.Competitions.GetByIdAsync(request.UpdateCompetitionDto.Id);
            if (entity == null) return Result.Fail<CompetitionDto>(ErrorMessagesConstants.NotFound(request.UpdateCompetitionDto.Id, typeof(Entities.Competition)));

            _mapper.Map(request.UpdateCompetitionDto, entity);
            await _repositoryWrapper.Competitions.UpdateAsync(entity);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<CompetitionDto>(entity);
                return Result.Ok(dto);
            }
            return Result.Fail<CompetitionDto>(ErrorMessagesConstants.FailedToUpdateEntity(typeof(Entities.Competition)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<CompetitionDto>(ErrorMessagesConstants.FailedToUpdateEntityInDatabase(typeof(Entities.Competition)));
        }
    }
}
