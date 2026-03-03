using FacultySports.Contracts.Location;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Location.Create;

public record CreateLocationCommand(CreateLocationDto CreateLocationDto) : IRequest<Result<LocationDto>>;
