using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Contracts.Schedule;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.Schedule.Delete;

public class DeleteScheduleHandler : IRequestHandler<DeleteScheduleCommand, Result<ScheduleDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<DeleteScheduleCommand> _validator;

    public DeleteScheduleHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<DeleteScheduleCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<ScheduleDto>> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var existing = await _repositoryWrapper.Schedules.GetByIdAsync(request.Id);
            if (existing == null) return Result.Fail<ScheduleDto>(ErrorMessagesConstants
                    .NotFound(request.Id, typeof(Entities.Schedule)));

            await _repositoryWrapper.Schedules.DeleteAsync(existing);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<ScheduleDto>(existing);
                return Result.Ok(dto);
            }

            return Result.Fail<ScheduleDto>(ErrorMessagesConstants.FailedToDeleteEntity(typeof(Entities.Schedule)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<ScheduleDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<ScheduleDto>(ErrorMessagesConstants.FailedToDeleteEntityInDatabase(typeof(Entities.Schedule)));
        }
    }
}
