using AutoMapper;
using FluentResults;
using MediatR;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Contracts.Section;

namespace FacultySports.Application.Queries.Section.GetAll;

public class GetAllSectionsHandler : IRequestHandler<GetAllSectionsQuery, Result<IEnumerable<SectionDto>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllSectionsHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<IEnumerable<SectionDto>>> Handle(GetAllSectionsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repositoryWrapper.Sections.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<SectionDto>>(entities);
        return Result.Ok(dtos);
    }
}
