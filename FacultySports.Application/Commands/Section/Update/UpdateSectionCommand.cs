using FacultySports.Contracts.Section;
using FluentResults;
using MediatR;

namespace FacultySports.Application.Commands.Section.Update;

public record UpdateSectionCommand(UpdateSectionDto UpdateSectionDto)
    : IRequest<Result<SectionDto>>;
