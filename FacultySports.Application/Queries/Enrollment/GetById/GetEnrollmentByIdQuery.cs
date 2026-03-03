using FluentResults;
using MediatR;
using FacultySports.Contracts.Enrollment;

namespace FacultySports.Application.Queries.Enrollment.GetById;

public record GetEnrollmentByIdQuery(int Id) : IRequest<Result<EnrollmentDto>>;
