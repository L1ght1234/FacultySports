using FluentResults;
using MediatR;
using FacultySports.Contracts.Enrollment;

namespace FacultySports.Application.Queries.Enrollment.GetAll;

public record GetAllEnrollmentsQuery() : IRequest<Result<IEnumerable<EnrollmentDto>>>;
