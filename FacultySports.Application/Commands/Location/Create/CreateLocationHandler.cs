using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.Location;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.Location.Create;

public class CreateLocationHandler : IRequestHandler<CreateLocationCommand, Result<LocationDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<CreateLocationCommand> _validator;

    public CreateLocationHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<CreateLocationCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<LocationDto>> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            Entities.Location entity = _mapper.Map<Entities.Location>(request.CreateLocationDto);
            await _repositoryWrapper.Locations.AddAsync(entity);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<LocationDto>(entity);
                return Result.Ok(dto);
            }

            return Result.Fail<LocationDto>(ErrorMessagesConstants.FailedToCreateEntity(typeof(Entities.Location)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<LocationDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<LocationDto>(ErrorMessagesConstants.FailedToCreateEntityInDatabase(typeof(Entities.Location)));
        }
    }
}
