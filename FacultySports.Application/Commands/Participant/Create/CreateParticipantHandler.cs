using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.Participant;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.Participant.Create;

public class CreateParticipantHandler : IRequestHandler<CreateParticipantCommand, Result<ParticipantDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<CreateParticipantCommand> _validator;

    public CreateParticipantHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<CreateParticipantCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<ParticipantDto>> Handle(CreateParticipantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            Entities.Participant entity = _mapper.Map<Entities.Participant>(request.CreateParticipantDto);
            await _repositoryWrapper.Participants.AddAsync(entity);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<ParticipantDto>(entity);
                return Result.Ok(dto);
            }

            return Result.Fail<ParticipantDto>(ErrorMessagesConstants.FailedToCreateEntity(typeof(Entities.Participant)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<ParticipantDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<ParticipantDto>(ErrorMessagesConstants.FailedToCreateEntityInDatabase(typeof(Entities.Participant)));
        }
    }
}
