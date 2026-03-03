using FluentResults;
using MediatR;
using FacultySports.Contracts.Location;


namespace FacultySports.Application.Queries.Location.GetById;

public record GetLocationByIdQuery(int Id) : IRequest<Result<LocationDto>>;
