using FluentResults;
using MediatR;
using FacultySports.Contracts.Location;

namespace FacultySports.Application.Queries.Location.GetAll;

public record GetAllLocationsQuery() : IRequest<Result<IEnumerable<LocationDto>>>;
