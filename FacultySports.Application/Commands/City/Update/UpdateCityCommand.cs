using FacultySports.Contracts.City;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.City.Update;

public record UpdateCityCommand(UpdateCityDto UpdateCityDto) : IRequest<Result<CityDto>>;
