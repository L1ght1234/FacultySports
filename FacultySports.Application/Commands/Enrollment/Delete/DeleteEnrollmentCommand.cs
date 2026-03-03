using FacultySports.Contracts.Enrollment;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Enrollment.Delete;

public record DeleteEnrollmentCommand(int Id) : IRequest<Result<EnrollmentDto>>;
