using AutoMapper;
using FluentResults;
using MediatR;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Application.Constants;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Contracts.Section;

namespace FacultySports.Application.Queries.Section.GetById;

public class GetSectionByIdHandler : IRequestHandler<GetSectionByIdQuery, Result<SectionDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetSectionByIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<SectionDto>> Handle(GetSectionByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repositoryWrapper.Sections.GetByIdAsync(request.Id);
        if (entity == null)
            return Result.Fail<SectionDto>(ErrorMessagesConstants.NotFound(request.Id, typeof(Entities.Section)));
        var dto = _mapper.Map<SectionDto>(entity);
        return Result.Ok(dto);
    }
}
