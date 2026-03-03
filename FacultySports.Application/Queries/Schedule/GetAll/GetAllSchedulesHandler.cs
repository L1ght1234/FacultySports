using AutoMapper;
using FluentResults;
using MediatR;
using FacultySports.Contracts.Schedule;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;

namespace FacultySports.Application.Queries.Schedule.GetAll;

public class GetAllSchedulesHandler : IRequestHandler<GetAllSchedulesQuery, Result<IEnumerable<ScheduleDto>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllSchedulesHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<IEnumerable<ScheduleDto>>> Handle(GetAllSchedulesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repositoryWrapper.Schedules.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<ScheduleDto>>(entities);
        return Result.Ok(dtos);
    }
}
