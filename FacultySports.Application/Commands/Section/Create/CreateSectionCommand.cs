using FacultySports.Contracts.Section;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Section.Create;

public record CreateSectionCommand(CreateSectionDto CreateSectionDto)
    : IRequest<Result<SectionDto>>;
