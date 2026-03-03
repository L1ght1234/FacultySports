using AutoMapper;
using FacultySports.Application.Queries.Schedule.GetAll;
using FacultySports.Application.Queries.Schedule.GetById;
using FacultySports.Application.Commands.Schedule.Create;
using FacultySports.Application.Commands.Schedule.Update;
using FacultySports.Application.Commands.Schedule.Delete;
using FacultySports.Contracts.Schedule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FacultySports.MVC.Models.Schedule;

namespace FacultySports.MVC.Controllers;

public class SchedulesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SchedulesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var res = await _mediator.Send(new GetAllSchedulesQuery());
        var dtos = res.IsSuccess ? res.Value : Enumerable.Empty<ScheduleDto>();
        var vm = _mapper.Map<IEnumerable<ScheduleViewModel>>(dtos);

        var locationsRes = await _mediator.Send(new Application.Queries.Location.GetAll.GetAllLocationsQuery());
        var locations = locationsRes.IsSuccess ? locationsRes.Value : Enumerable.Empty<Contracts.Location.LocationDto>();
        foreach (var s in vm)
        {
            s.LocationName = locations.FirstOrDefault(l => l.Id == s.LocationId)?.Name;
        }

        return View(vm);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Locations = (await _mediator.Send(new Application.Queries.Location.GetAll.GetAllLocationsQuery())).Value;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateScheduleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Locations = (await _mediator.Send(new Application.Queries.Location.GetAll.GetAllLocationsQuery())).Value;
            return View(model);
        }

        var dto = _mapper.Map<CreateScheduleDto>(model);
        var result = await _mediator.Send(new CreateScheduleCommand(dto));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        ViewBag.Locations = (await _mediator.Send(new Application.Queries.Location.GetAll.GetAllLocationsQuery())).Value;
        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var res = await _mediator.Send(new GetScheduleByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<ScheduleViewModel>(res.Value);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var res = await _mediator.Send(new GetScheduleByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<CreateScheduleViewModel>(res.Value);
        ViewBag.Locations = (await _mediator.Send(new Application.Queries.Location.GetAll.GetAllLocationsQuery())).Value;
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CreateScheduleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Locations = (await _mediator.Send(new Application.Queries.Location.GetAll.GetAllLocationsQuery())).Value;
            return View(model);
        }

        var dto = _mapper.Map<UpdateScheduleDto>(model);
        dto.Id = id;
        var result = await _mediator.Send(new UpdateScheduleCommand(dto));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        ViewBag.Locations = (await _mediator.Send(new Application.Queries.Location.GetAll.GetAllLocationsQuery())).Value;
        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var res = await _mediator.Send(new GetScheduleByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<ScheduleViewModel>(res.Value);
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _mediator.Send(new DeleteScheduleCommand(id));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        var res = await _mediator.Send(new GetScheduleByIdQuery(id));
        var vm = res.IsSuccess ? _mapper.Map<ScheduleViewModel>(res.Value) : null;
        return View(vm);
    }
}
