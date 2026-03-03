using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.Participant;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.Participant.Update;

public class UpdateParticipantHandler : IRequestHandler<UpdateParticipantCommand, Result<ParticipantDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<UpdateParticipantCommand> _validator;

    public UpdateParticipantHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<UpdateParticipantCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<ParticipantDto>> Handle(UpdateParticipantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var existing = await _repositoryWrapper.Participants.GetByIdAsync(request.UpdateParticipantDto.Id);
            if (existing == null) return Result.Fail<ParticipantDto>(ErrorMessagesConstants.NotFound(request.UpdateParticipantDto.Id, typeof(Entities.Participant)));

            _mapper.Map(request.UpdateParticipantDto, existing);
            await _repositoryWrapper.Participants.UpdateAsync(existing);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<ParticipantDto>(existing);
                return Result.Ok(dto);
            }

            return Result.Fail<ParticipantDto>(ErrorMessagesConstants.FailedToUpdateEntity(typeof(Entities.Participant)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<ParticipantDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<ParticipantDto>(ErrorMessagesConstants.FailedToUpdateEntityInDatabase(typeof(Entities.Participant)));
        }
    }
}
