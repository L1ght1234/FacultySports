using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.City;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.City.Update;

public class UpdateCityHandler : IRequestHandler<UpdateCityCommand, Result<CityDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<UpdateCityCommand> _validator;

    public UpdateCityHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<UpdateCityCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<CityDto>> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var entity = await _repositoryWrapper.Cities.GetByIdAsync(request.UpdateCityDto.Id);
            if (entity == null) return Result.Fail<CityDto>(ErrorMessagesConstants
                    .NotFound(request.UpdateCityDto.Id, typeof(Entities.City)));

            _mapper.Map(request.UpdateCityDto, entity);
            await _repositoryWrapper.Cities.UpdateAsync(entity);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<CityDto>(entity);
                return Result.Ok(dto);
            }
            return Result.Fail<CityDto>(ErrorMessagesConstants.FailedToUpdateEntity(typeof(Entities.City)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<CityDto>(ErrorMessagesConstants.FailedToUpdateEntityInDatabase(typeof(Entities.City)));
        }
    }
}
