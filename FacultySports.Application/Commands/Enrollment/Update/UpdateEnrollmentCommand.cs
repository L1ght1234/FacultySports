using FacultySports.Contracts.Enrollment;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Enrollment.Update;

public record UpdateEnrollmentCommand(UpdateEnrollmentDto UpdateEnrollmentDto) : IRequest<Result<EnrollmentDto>>;
