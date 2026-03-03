using AutoMapper;
using FacultySports.Application.Constants;
using FacultySports.Contracts.Enrollment;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;
using Entities = FacultySports.Domain.Entities;

namespace FacultySports.Application.Queries.Enrollment.GetById;

public class GetEnrollmentByIdHandler : IRequestHandler<GetEnrollmentByIdQuery, Result<EnrollmentDto>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;

    public GetEnrollmentByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<EnrollmentDto>> Handle(GetEnrollmentByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repositoryWrapper.Enrollments.GetByIdAsync(request.Id);
        if (entity is null) return Result.Fail<EnrollmentDto>(ErrorMessagesConstants.NotFound(request.Id, typeof(Entities.Enrollment)));
        var dto = _mapper.Map<EnrollmentDto>(entity);
        return Result.Ok(dto);
    }
}
