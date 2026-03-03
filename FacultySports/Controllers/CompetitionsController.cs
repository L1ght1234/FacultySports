using AutoMapper;
using FacultySports.Application.Queries.Competition.GetAll;
using FacultySports.Application.Queries.Competition.GetById;
using FacultySports.Application.Commands.Competition.Create;
using FacultySports.Application.Commands.Competition.Update;
using FacultySports.Application.Commands.Competition.Delete;
using FacultySports.Contracts.Competition;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FacultySports.MVC.Models.Competition;

namespace FacultySports.MVC.Controllers;

public class CompetitionsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CompetitionsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var res = await _mediator.Send(new GetAllCompetitionsQuery());
        var dtos = res.IsSuccess ? res.Value : Enumerable.Empty<CompetitionDto>();
        var vm = _mapper.Map<IEnumerable<CompetitionViewModel>>(dtos);
        return View(vm);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Sections = (await _mediator.Send(new Application.Queries.Section.GetAll.GetAllSectionsQuery())).Value;
        ViewBag.Locations = (await _mediator.Send(new Application.Queries.Location.GetAll.GetAllLocationsQuery())).Value;
        ViewBag.Statuses = (await _mediator.Send(new Application.Queries.CompetitionStatus.GetAll.GetAllCompetitionStatusesQuery())).Value;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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

    public async Task<IActionResult> Delete(int id)
    {
        var res = await _mediator.Send(new GetCompetitionByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<CompetitionViewModel>(res.Value);
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
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
