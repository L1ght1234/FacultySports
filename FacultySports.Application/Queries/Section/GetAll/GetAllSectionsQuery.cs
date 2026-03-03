using FluentResults;
using MediatR;
using FacultySports.Contracts.Section;

namespace FacultySports.Application.Queries.Section.GetAll;

public record GetAllSectionsQuery() : IRequest<Result<IEnumerable<SectionDto>>>;
