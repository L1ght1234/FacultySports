using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.Enrollment;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.Enrollment.Update;

public class UpdateEnrollmentHandler : IRequestHandler<UpdateEnrollmentCommand, Result<EnrollmentDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<UpdateEnrollmentCommand> _validator;

    public UpdateEnrollmentHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<UpdateEnrollmentCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<EnrollmentDto>> Handle(UpdateEnrollmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var dto = request.UpdateEnrollmentDto;
            var entity = await _repositoryWrapper.Enrollments.GetByIdAsync(dto.Id);
            if (entity is null)
                return Result.Fail<EnrollmentDto>(ErrorMessagesConstants.NotFound(dto.Id, typeof(Entities.Enrollment)));

            _mapper.Map(dto, entity);
            await _repositoryWrapper.Enrollments.UpdateAsync(entity);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var outDto = _mapper.Map<EnrollmentDto>(entity);
                return Result.Ok(outDto);
            }

            return Result.Fail<EnrollmentDto>(ErrorMessagesConstants.FailedToUpdateEntity(typeof(Entities.Enrollment)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<EnrollmentDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<EnrollmentDto>(ErrorMessagesConstants.FailedToUpdateEntityInDatabase(typeof(Entities.Enrollment)));
        }
    }
}
