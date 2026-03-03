using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.Schedule;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Application.Constants;
using Entities = FacultySports.Domain.Entities;

namespace FacultySports.Application.Commands.Schedule.Update;

public class UpdateScheduleHandler : IRequestHandler<UpdateScheduleCommand, Result<ScheduleDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<UpdateScheduleCommand> _validator;

    public UpdateScheduleHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<UpdateScheduleCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<ScheduleDto>> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var entity = await _repositoryWrapper.Schedules.GetByIdAsync(request.UpdateScheduleDto.Id);
            if (entity == null) return Result.Fail<ScheduleDto>(ErrorMessagesConstants.NotFound(request.UpdateScheduleDto.Id, typeof(Entities.Schedule)));

            _mapper.Map(request.UpdateScheduleDto, entity);
            await _repositoryWrapper.Schedules.UpdateAsync(entity);
    
            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<ScheduleDto>(entity);
                return Result.Ok(dto);
            }
            return Result.Fail<ScheduleDto>(ErrorMessagesConstants.FailedToUpdateEntity(typeof(Entities.Schedule)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<ScheduleDto>(ErrorMessagesConstants.FailedToUpdateEntityInDatabase(typeof(Entities.Schedule)));
        }
    }
}
