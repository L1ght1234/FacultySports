using FacultySports.Contracts.Enrollment;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Enrollment.Create;

public record CreateEnrollmentCommand(CreateEnrollmentDto CreateEnrollmentDto)
    : IRequest<Result<EnrollmentDto>>;
