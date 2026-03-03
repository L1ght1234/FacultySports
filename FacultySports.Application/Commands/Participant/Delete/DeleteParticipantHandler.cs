using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Contracts.Participant;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.Participant.Delete;

public class DeleteParticipantHandler : IRequestHandler<DeleteParticipantCommand, Result<ParticipantDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<DeleteParticipantCommand> _validator;

    public DeleteParticipantHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<DeleteParticipantCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<ParticipantDto>> Handle(DeleteParticipantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var existing = await _repositoryWrapper.Participants.GetByIdAsync(request.Id);
            if (existing == null)
                return Result.Fail<ParticipantDto>(ErrorMessagesConstants.NotFound(request.Id, typeof(Entities.Participant)));

            await _repositoryWrapper.Participants.DeleteAsync(existing);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<ParticipantDto>(existing);
                return Result.Ok(dto);
            }

            return Result.Fail<ParticipantDto>(ErrorMessagesConstants.FailedToDeleteEntity(typeof(Entities.Participant)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<ParticipantDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<ParticipantDto>(ErrorMessagesConstants.FailedToDeleteEntityInDatabase(typeof(Entities.Participant)));
        }
    }
}
