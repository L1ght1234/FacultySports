using AutoMapper;
using FluentResults;
using MediatR;
using FacultySports.Contracts.Schedule;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using FacultySports.Application.Constants;
using Entities = FacultySports.Domain.Entities;

namespace FacultySports.Application.Queries.Schedule.GetById;

public class GetScheduleByIdHandler : IRequestHandler<GetScheduleByIdQuery, Result<ScheduleDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetScheduleByIdHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<ScheduleDto>> Handle(GetScheduleByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repositoryWrapper.Schedules.GetByIdAsync(request.Id);
        if (entity == null) return Result.Fail<ScheduleDto>(ErrorMessagesConstants.NotFound(request.Id, typeof(Entities.Schedule)));

        var dto = _mapper.Map<ScheduleDto>(entity);
        return Result.Ok(dto);
    }
}
