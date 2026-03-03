using FacultySports.Contracts.Section;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Section.Delete;

public record DeleteSectionCommand(int Id) : IRequest<Result<SectionDto>>;
