using FacultySports.Contracts.City;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.City.Delete;

public record DeleteCityCommand(int Id) : IRequest<Result<CityDto>>;
