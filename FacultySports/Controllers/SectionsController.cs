using AutoMapper;
using FacultySports.Application.Queries.Section.GetAll;
using FacultySports.Application.Queries.Section.GetById;
using FacultySports.Application.Commands.Section.Create;
using FacultySports.Application.Commands.Section.Update;
using FacultySports.Application.Commands.Section.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FacultySports.MVC.Models.Section;
using Microsoft.AspNetCore.Authorization;

namespace FacultySports.MVC.Controllers;

[Authorize]
public class SectionsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SectionsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _mediator.Send(new GetAllSectionsQuery());
        if (result.IsFailed) return View(new List<SectionViewModel>());

        var vm = _mapper.Map<IEnumerable<SectionViewModel>>(result.Value);
        return View(vm);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View(new CreateSectionViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateSectionViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = _mapper.Map<Contracts.Section.CreateSectionDto>(model);
        var result = await _mediator.Send(new CreateSectionCommand(dto));

        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        foreach (var err in result.Errors)
            ModelState.AddModelError(string.Empty, err.Message);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var result = await _mediator.Send(new GetSectionByIdQuery(id));
        if (result.IsFailed) return NotFound();

        var vm = _mapper.Map<SectionViewModel>(result.Value);
        return View(vm);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _mediator.Send(new GetSectionByIdQuery(id));
        if (result.IsFailed) return NotFound();

        var vm = _mapper.Map<SectionViewModel>(result.Value);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(SectionViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = _mapper.Map<Contracts.Section.UpdateSectionDto>(model);
        var result = await _mediator.Send(new UpdateSectionCommand(dto));

        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        foreach (var err in result.Errors) ModelState.AddModelError(string.Empty, err.Message);
        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new GetSectionByIdQuery(id));
        if (result.IsFailed) return NotFound();

        var vm = _mapper.Map<SectionViewModel>(result.Value);
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _mediator.Send(new DeleteSectionCommand(id));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        TempData["Error"] = string.Join("; ", result.Errors.Select(e => e.Message));
        return RedirectToAction(nameof(Index));
    }
}