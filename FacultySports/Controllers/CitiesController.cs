using AutoMapper;
using FacultySports.Application.Queries.City.GetAll;
using FacultySports.Application.Queries.City.GetById;
using FacultySports.Application.Commands.City.Create;
using FacultySports.Application.Commands.City.Update;
using FacultySports.Application.Commands.City.Delete;
using FacultySports.Contracts.City;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FacultySports.MVC.Models.City;

namespace FacultySports.MVC.Controllers;

public class CitiesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CitiesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var res = await _mediator.Send(new GetAllCitiesQuery());
        var dtos = res.IsSuccess ? res.Value : Enumerable.Empty<CityDto>();
        var vm = _mapper.Map<IEnumerable<CityViewModel>>(dtos);
        return View(vm);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCityViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = _mapper.Map<CreateCityDto>(model);
        var result = await _mediator.Send(new CreateCityCommand(dto));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var res = await _mediator.Send(new GetCityByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<CityViewModel>(res.Value);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var res = await _mediator.Send(new GetCityByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<CreateCityViewModel>(res.Value);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CreateCityViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = _mapper.Map<UpdateCityDto>(model);
        dto.Id = id;
        var result = await _mediator.Send(new UpdateCityCommand(dto));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var res = await _mediator.Send(new GetCityByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<CityViewModel>(res.Value);
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _mediator.Send(new DeleteCityCommand(id));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        var res = await _mediator.Send(new GetCityByIdQuery(id));
        var vm = res.IsSuccess ? _mapper.Map<CityViewModel>(res.Value) : null;
        return View(vm);
    }
}
