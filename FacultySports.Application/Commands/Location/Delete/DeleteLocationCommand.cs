using FacultySports.Contracts.Location;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Location.Delete;

public record DeleteLocationCommand(int Id) : IRequest<Result<LocationDto>>;
