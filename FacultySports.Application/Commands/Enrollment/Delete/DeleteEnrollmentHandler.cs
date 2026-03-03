using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Contracts.Enrollment;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.Enrollment.Delete;

public class DeleteEnrollmentHandler : IRequestHandler<DeleteEnrollmentCommand, Result<EnrollmentDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<DeleteEnrollmentCommand> _validator;

    public DeleteEnrollmentHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<DeleteEnrollmentCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<EnrollmentDto>> Handle(DeleteEnrollmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var existing = await _repositoryWrapper.Enrollments.GetByIdAsync(request.Id);
            if (existing == null) return Result.Fail<EnrollmentDto>(ErrorMessagesConstants
                    .NotFound(request.Id, typeof(Entities.Enrollment)));

            await _repositoryWrapper.Enrollments.DeleteAsync(existing);
            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<EnrollmentDto>(existing);
                return Result.Ok(dto);
            }

            return Result.Fail<EnrollmentDto>(ErrorMessagesConstants.FailedToDeleteEntity(typeof(Entities.Enrollment)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<EnrollmentDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<EnrollmentDto>(ErrorMessagesConstants.FailedToDeleteEntityInDatabase(typeof(Entities.Enrollment)));
        }
    }
}
