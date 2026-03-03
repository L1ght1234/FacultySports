using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Contracts.Section;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Application.Constants;

namespace FacultySports.Application.Commands.Section.Update;

public class UpdateSectionHandler : IRequestHandler<UpdateSectionCommand, Result<SectionDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<UpdateSectionCommand> _validator;

    public UpdateSectionHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<UpdateSectionCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<SectionDto>> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var existing = await _repositoryWrapper.Sections.GetByIdAsync(request.UpdateSectionDto.Id);
            if (existing == null)
                return Result.Fail<SectionDto>(ErrorMessagesConstants.NotFound(request.UpdateSectionDto.Id, typeof(Entities.Section)));

            _mapper.Map(request.UpdateSectionDto, existing);
            await _repositoryWrapper.Sections.UpdateAsync(existing);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<SectionDto>(existing);
                return Result.Ok(dto);
            }

            return Result.Fail<SectionDto>(ErrorMessagesConstants.FailedToUpdateEntity(typeof(Entities.Section)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<SectionDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<SectionDto>(ErrorMessagesConstants.FailedToUpdateEntityInDatabase(typeof(Entities.Section)));
        }
    }
}
