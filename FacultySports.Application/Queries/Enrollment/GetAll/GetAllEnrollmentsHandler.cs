using AutoMapper;
using FacultySports.Contracts.Enrollment;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Queries.Enrollment.GetAll;

public class GetAllEnrollmentsHandler : IRequestHandler<GetAllEnrollmentsQuery, Result<IEnumerable<EnrollmentDto>>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;

    public GetAllEnrollmentsHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<EnrollmentDto>>> Handle(GetAllEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repositoryWrapper.Enrollments.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<EnrollmentDto>>(entities);
        return Result.Ok(dtos);
    }
}
