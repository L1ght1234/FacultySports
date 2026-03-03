using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.Schedule;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.Schedule.Create;

public class CreateScheduleHandler : IRequestHandler<CreateScheduleCommand, Result<ScheduleDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<CreateScheduleCommand> _validator;

    public CreateScheduleHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<CreateScheduleCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<ScheduleDto>> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            Entities.Schedule entity = _mapper.Map<Entities.Schedule>(request.CreateScheduleDto);
            await _repositoryWrapper.Schedules.AddAsync(entity);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<ScheduleDto>(entity);
                return Result.Ok(dto);
            }

            return Result.Fail<ScheduleDto>(ErrorMessagesConstants.FailedToCreateEntity(typeof(Entities.Schedule)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<ScheduleDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<ScheduleDto>(ErrorMessagesConstants.FailedToCreateEntityInDatabase(typeof(Entities.Schedule)));
        }
    }
}
