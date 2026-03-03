using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.Location;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.Location.Update;

public class UpdateLocationHandler : IRequestHandler<UpdateLocationCommand, Result<LocationDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<UpdateLocationCommand> _validator;

    public UpdateLocationHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<UpdateLocationCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<LocationDto>> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var entity = await _repositoryWrapper.Locations.GetByIdAsync(request.UpdateLocationDto.Id);
            if (entity == null) return Result.Fail<LocationDto>(ErrorMessagesConstants.NotFound(request.UpdateLocationDto.Id, typeof(Entities.Location)));

            _mapper.Map(request.UpdateLocationDto, entity);
            await _repositoryWrapper.Locations.UpdateAsync(entity);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<LocationDto>(entity);
                return Result.Ok(dto);
            }
            return Result.Fail<LocationDto>(ErrorMessagesConstants.FailedToUpdateEntity(typeof(Entities.Location)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<LocationDto>(ErrorMessagesConstants.FailedToUpdateEntityInDatabase(typeof(Entities.Location)));
        }
    }
}
