using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.City;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.City.Create;

public class CreateCityHandler : IRequestHandler<CreateCityCommand, Result<CityDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<CreateCityCommand> _validator;

    public CreateCityHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<CreateCityCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<CityDto>> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            Entities.City entity = _mapper.Map<Entities.City>(request.CreateCityDto);
            await _repositoryWrapper.Cities.AddAsync(entity);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<CityDto>(entity);
                return Result.Ok(dto);
            }

            return Result.Fail<CityDto>(ErrorMessagesConstants.FailedToCreateEntity(typeof(Entities.City)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<CityDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<CityDto>(ErrorMessagesConstants.FailedToCreateEntityInDatabase(typeof(Entities.City)));
        }
    }
}
