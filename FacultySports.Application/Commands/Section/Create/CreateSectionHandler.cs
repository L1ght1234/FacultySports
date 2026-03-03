using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.Section;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.Section.Create;

public class CreateSectionHandler : IRequestHandler<CreateSectionCommand, Result<SectionDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<CreateSectionCommand> _validator;

    public CreateSectionHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<CreateSectionCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<SectionDto>> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            Entities.Section entity = _mapper.Map<Entities.Section>(request.CreateSectionDto);
            await _repositoryWrapper.Sections.AddAsync(entity);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                SectionDto responseDto = _mapper.Map<SectionDto>(entity);
                return Result.Ok(responseDto);
            }

            return Result.Fail<SectionDto>(ErrorMessagesConstants.FailedToCreateEntity(typeof(Entities.Section)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<SectionDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<SectionDto>(ErrorMessagesConstants.FailedToCreateEntityInDatabase(typeof(Entities.Section)));
        }
    }
}
