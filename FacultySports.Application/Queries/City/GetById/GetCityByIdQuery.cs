using FluentResults;
using MediatR;
using FacultySports.Contracts.City;

namespace FacultySports.Application.Queries.City.GetById;

public record GetCityByIdQuery(int Id) : IRequest<Result<CityDto>>;
