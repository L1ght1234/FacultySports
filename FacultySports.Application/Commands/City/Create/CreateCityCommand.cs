using FacultySports.Contracts.City;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.City.Create;

public record CreateCityCommand(CreateCityDto CreateCityDto) : IRequest<Result<CityDto>>;
