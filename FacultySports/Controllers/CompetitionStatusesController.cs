using AutoMapper;
using FacultySports.Application.Queries.CompetitionStatus.GetAll;
using FacultySports.Application.Queries.CompetitionStatus.GetById;
using FacultySports.Application.Commands.CompetitionStatus.Create;
using FacultySports.Application.Commands.CompetitionStatus.Update;
using FacultySports.Application.Commands.CompetitionStatus.Delete;
using FacultySports.Contracts.CompetitionStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FacultySports.MVC.Models.CompetitionStatus;
using Microsoft.AspNetCore.Authorization;

namespace FacultySports.MVC.Controllers;

[Authorize(Roles = "Admin")]
public class CompetitionStatusesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CompetitionStatusesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var res = await _mediator.Send(new GetAllCompetitionStatusesQuery());
        var dtos = res.IsSuccess ? res.Value : Enumerable.Empty<CompetitionStatusDto>();
        var vm = _mapper.Map<IEnumerable<CompetitionStatusViewModel>>(dtos);
        return View(vm);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCompetitionStatusViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = _mapper.Map<CreateCompetitionStatusDto>(model);
        var result = await _mediator.Send(new CreateCompetitionStatusCommand(dto));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var res = await _mediator.Send(new GetCompetitionStatusByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<CompetitionStatusViewModel>(res.Value);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var res = await _mediator.Send(new GetCompetitionStatusByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<CreateCompetitionStatusViewModel>(res.Value);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CreateCompetitionStatusViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = _mapper.Map<UpdateCompetitionStatusDto>(model);
        dto.Id = id;
        var result = await _mediator.Send(new UpdateCompetitionStatusCommand(dto));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var res = await _mediator.Send(new GetCompetitionStatusByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<CompetitionStatusViewModel>(res.Value);
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _mediator.Send(new DeleteCompetitionStatusCommand(id));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        var res = await _mediator.Send(new GetCompetitionStatusByIdQuery(id));
        var vm = res.IsSuccess ? _mapper.Map<CompetitionStatusViewModel>(res.Value) : null;
        return View(vm);
    }
}
