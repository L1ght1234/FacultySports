using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;
using Entities = FacultySports.Domain.Entities;
using FacultySports.Application.Constants;
using FacultySports.Contracts.Section;

namespace FacultySports.Application.Commands.Section.Delete;

public class DeleteSectionHandler : IRequestHandler<DeleteSectionCommand, Result<SectionDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<DeleteSectionCommand> _validator;

    public DeleteSectionHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper, IValidator<DeleteSectionCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<SectionDto>> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var existing = await _repositoryWrapper.Sections.GetByIdAsync(request.Id);
            if (existing == null)
                return Result.Fail<SectionDto>(ErrorMessagesConstants.NotFound(request.Id, typeof(Entities.Section)));

            await _repositoryWrapper.Sections.DeleteAsync(existing);

            if (await _repositoryWrapper.SaveAsync() > 0)
            {
                var dto = _mapper.Map<SectionDto>(existing);
                return Result.Ok(dto);
            }

            return Result.Fail<SectionDto>(ErrorMessagesConstants.FailedToDeleteEntity(typeof(Entities.Section)));
        }
        catch (ValidationException ex)
        {
            return Result.Fail<SectionDto>(ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DbUpdateException)
        {
            return Result.Fail<SectionDto>(ErrorMessagesConstants.FailedToDeleteEntityInDatabase(typeof(Entities.Section)));
        }
    }
}
