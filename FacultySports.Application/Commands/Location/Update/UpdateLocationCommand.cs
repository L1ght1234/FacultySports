using FacultySports.Contracts.Location;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Location.Update;

public record UpdateLocationCommand(UpdateLocationDto UpdateLocationDto) : IRequest<Result<LocationDto>>;
