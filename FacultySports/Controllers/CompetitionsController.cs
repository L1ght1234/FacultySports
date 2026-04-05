using AutoMapper;
using FacultySports.Application.Queries.Competition.GetAll;
using FacultySports.Application.Queries.Competition.GetById;
using FacultySports.Application.Commands.Competition.Create;
using FacultySports.Application.Commands.Competition.Update;
using FacultySports.Application.Commands.Competition.Delete;
using FacultySports.Contracts.Competition;
using FacultySports.Domain.Entities;
using FacultySports.MVC.Models.Competition;
using FacultySports.MVC.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FacultySports.MVC.Controllers;

[Authorize]
public class CompetitionsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IDataPortServiceFactory<Competition> _competitionDataPortServiceFactory;

    public CompetitionsController(IMediator mediator, IMapper mapper, IDataPortServiceFactory<Competition> competitionDataPortServiceFactory)
    {
        _mediator = mediator;
        _mapper = mapper;
        _competitionDataPortServiceFactory = competitionDataPortServiceFactory;
    }

    public async Task<IActionResult> Index()
    {
        var res = await _mediator.Send(new GetAllCompetitionsQuery());
        var dtos = res.IsSuccess ? res.Value : Enumerable.Empty<CompetitionDto>();
        var vm = _mapper.Map<IEnumerable<CompetitionViewModel>>(dtos);
        return View(vm);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Import()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Import(IFormFile competitionsFile, CancellationToken cancellationToken)
    {
        if (competitionsFile is null || competitionsFile.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Please select an Excel file to upload.");
            return View();
        }

        var contentType = competitionsFile.ContentType;
        if (!string.Equals(contentType, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(string.Empty, "Only .xlsx files are supported.");
            return View();
        }

        var importService = _competitionDataPortServiceFactory.GetImportService(contentType);
        using var stream = competitionsFile.OpenReadStream();
        await importService.ImportFromStreamAsync(stream, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Export([FromQuery] string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", CancellationToken cancellationToken = default)
    {
        var exportService = _competitionDataPortServiceFactory.GetExportService(contentType);
        var memoryStream = new MemoryStream();
        await exportService.WriteToAsync(memoryStream, cancellationToken);
        await memoryStream.FlushAsync(cancellationToken);
        memoryStream.Position = 0;

        return new FileStreamResult(memoryStream, contentType)
        {
            FileDownloadName = $"competitions_{DateTime.UtcNow:yyyy-MM-dd}.xlsx"
        };
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Sections = (await _mediator.Send(new Application.Queries.Section.GetAll.GetAllSectionsQuery())).Value;
        ViewBag.Locations = (await _mediator.Send(new Application.Queries.Location.GetAll.GetAllLocationsQuery())).Value;
        ViewBag.Statuses = (await _mediator.Send(new Application.Queries.CompetitionStatus.GetAll.GetAllCompetitionStatusesQuery())).Value;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateCompetitionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Sections = (await _mediator.Send(new Application.Queries.Section.GetAll.GetAllSectionsQuery())).Value;
            ViewBag.Locations = (await _mediator.Send(new Application.Queries.Location.GetAll.GetAllLocationsQuery())).Value;
            ViewBag.Statuses = (await _mediator.Send(new Application.Queries.CompetitionStatus.GetAll.GetAllCompetitionStatusesQuery())).Value;
            return View(model);
        }

        var dto = _mapper.Map<CreateCompetitionDto>(model);
        var result = await _mediator.Send(new CreateCompetitionCommand(dto));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        ViewBag.Sections = (await _mediator.Send(new Application.Queries.Section.GetAll.GetAllSectionsQuery())).Value;
        ViewBag.Locations = (await _mediator.Send(new Application.Queries.Location.GetAll.GetAllLocationsQuery())).Value;
        ViewBag.Statuses = (await _mediator.Send(new Application.Queries.CompetitionStatus.GetAll.GetAllCompetitionStatusesQuery())).Value;
        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var res = await _mediator.Send(new GetCompetitionByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<CompetitionViewModel>(res.Value);
        return View(vm);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var res = await _mediator.Send(new GetCompetitionByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<CreateCompetitionViewModel>(res.Value);

        ViewBag.Sections = (await _mediator.Send(new Application.Queries.Section.GetAll.GetAllSectionsQuery())).Value;
        ViewBag.Locations = (await _mediator.Send(new Application.Queries.Location.GetAll.GetAllLocationsQuery())).Value;
        ViewBag.Statuses = (await _mediator.Send(new Application.Queries.CompetitionStatus.GetAll.GetAllCompetitionStatusesQuery())).Value;
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, CreateCompetitionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Sections = (await _mediator.Send(new Application.Queries.Section.GetAll.GetAllSectionsQuery())).Value;
            ViewBag.Locations = (await _mediator.Send(new Application.Queries.Location.GetAll.GetAllLocationsQuery())).Value;
            ViewBag.Statuses = (await _mediator.Send(new Application.Queries.CompetitionStatus.GetAll.GetAllCompetitionStatusesQuery())).Value;
            return View(model);
        }

        var dto = _mapper.Map<UpdateCompetitionDto>(model);
        dto.Id = id;
        var result = await _mediator.Send(new UpdateCompetitionCommand(dto));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        ViewBag.Sections = (await _mediator.Send(new Application.Queries.Section.GetAll.GetAllSectionsQuery())).Value;
        ViewBag.Locations = (await _mediator.Send(new Application.Queries.Location.GetAll.GetAllLocationsQuery())).Value;
        ViewBag.Statuses = (await _mediator.Send(new Application.Queries.CompetitionStatus.GetAll.GetAllCompetitionStatusesQuery())).Value;
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await _mediator.Send(new GetCompetitionByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<CompetitionViewModel>(res.Value);
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _mediator.Send(new DeleteCompetitionCommand(id));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        var res = await _mediator.Send(new GetCompetitionByIdQuery(id));
        var vm = res.IsSuccess ? _mapper.Map<CompetitionViewModel>(res.Value) : null;
        return View(vm);
    }
}
