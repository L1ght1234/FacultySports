using FluentResults;
using MediatR;
using FacultySports.Contracts.Section;

namespace FacultySports.Application.Queries.Section.GetById;

public record GetSectionByIdQuery(int Id) : IRequest<Result<SectionDto>>;
