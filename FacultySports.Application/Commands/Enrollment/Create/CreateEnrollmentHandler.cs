using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.Enrollment;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.Enrollment.Create;

public class CreateEnrollmentHandler : IRequestHandler<CreateEnrollmentCommand, Result<EnrollmentDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<CreateEnrollmentCommand> _validator;

    public CreateEnrollmentHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<CreateEnrollmentCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<EnrollmentDto>> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            Entities.Enrollment entity = _mapper.Map<Entities.Enrollment>(request.CreateEnrollmentDto);
            await _repositoryWrapper.Enrollments.AddAsync(entity);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<EnrollmentDto>(entity);
                return Result.Ok(dto);
            }

            return Result.Fail<EnrollmentDto>(ErrorMessagesConstants.FailedToCreateEntity(typeof(Entities.Enrollment)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<EnrollmentDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<EnrollmentDto>(ErrorMessagesConstants.FailedToCreateEntityInDatabase(typeof(Entities.Enrollment)));
        }
    }
}
