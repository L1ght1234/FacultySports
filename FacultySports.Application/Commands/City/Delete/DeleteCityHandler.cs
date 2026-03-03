using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Contracts.City;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.City.Delete;

public class DeleteCityHandler : IRequestHandler<DeleteCityCommand, Result<CityDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<DeleteCityCommand> _validator;

    public DeleteCityHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<DeleteCityCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<CityDto>> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var existing = await _repositoryWrapper.Cities.GetByIdAsync(request.Id);
            if (existing == null) return Result.Fail<CityDto>(ErrorMessagesConstants
                    .NotFound(request.Id, typeof(Entities.City)));

            await _repositoryWrapper.Cities.DeleteAsync(existing);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<CityDto>(existing);
                return Result.Ok(dto);
            }

            return Result.Fail<CityDto>(ErrorMessagesConstants.FailedToDeleteEntity(typeof(Entities.City)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<CityDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<CityDto>(ErrorMessagesConstants.FailedToDeleteEntityInDatabase(typeof(Entities.City)));
        }
    }
}
