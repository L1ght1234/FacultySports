using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.CompetitionStatus;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.CompetitionStatus.Create;

public class CreateCompetitionStatusHandler : IRequestHandler<CreateCompetitionStatusCommand, Result<CompetitionStatusDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<CreateCompetitionStatusCommand> _validator;

    public CreateCompetitionStatusHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<CreateCompetitionStatusCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<CompetitionStatusDto>> Handle(CreateCompetitionStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            Entities.CompetitionStatus entity = _mapper.Map<Entities.CompetitionStatus>(request.CreateCompetitionStatusDto);
            await _repositoryWrapper.CompetitionStatuses.AddAsync(entity);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<CompetitionStatusDto>(entity);
                return Result.Ok(dto);
            }

            return Result.Fail<CompetitionStatusDto>(ErrorMessagesConstants.FailedToCreateEntity(typeof(Entities.CompetitionStatus)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<CompetitionStatusDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<CompetitionStatusDto>(ErrorMessagesConstants.FailedToCreateEntityInDatabase(typeof(Entities.CompetitionStatus)));
        }
    }
}
