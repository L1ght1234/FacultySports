using FacultySports.Contracts.City;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Queries.City.GetAll;

public record GetAllCitiesQuery() : IRequest<Result<IEnumerable<CityDto>>>;
