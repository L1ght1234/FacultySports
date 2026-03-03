using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Contracts.Location;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.Location.Delete;

public class DeleteLocationHandler : IRequestHandler<DeleteLocationCommand, Result<LocationDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<DeleteLocationCommand> _validator;

    public DeleteLocationHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<DeleteLocationCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<LocationDto>> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var existing = await _repositoryWrapper.Locations.GetByIdAsync(request.Id);
            if (existing == null) return Result.Fail<LocationDto>(ErrorMessagesConstants
                    .NotFound(request.Id, typeof(Entities.Location)));

            await _repositoryWrapper.Locations.DeleteAsync(existing);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<LocationDto>(existing);
                return Result.Ok(dto);
            }

            return Result.Fail<LocationDto>(ErrorMessagesConstants.FailedToDeleteEntity(typeof(Entities.Location)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<LocationDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<LocationDto>(ErrorMessagesConstants.FailedToDeleteEntityInDatabase(typeof(Entities.Location)));
        }
    }
}
