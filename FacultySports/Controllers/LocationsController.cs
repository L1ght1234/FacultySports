using AutoMapper;
using FacultySports.Application.Queries.Location.GetAll;
using FacultySports.Application.Queries.Location.GetById;
using FacultySports.Application.Commands.Location.Create;
using FacultySports.Application.Commands.Location.Update;
using FacultySports.Application.Commands.Location.Delete;
using FacultySports.Application.Queries.City.GetAll;
using FacultySports.Contracts.Location;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FacultySports.MVC.Models.Location;
using Microsoft.AspNetCore.Authorization;

namespace FacultySports.MVC.Controllers;

[Authorize(Roles = "Admin")]
public class LocationsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public LocationsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var res = await _mediator.Send(new GetAllLocationsQuery());
        var dtos = res.IsSuccess ? res.Value : Enumerable.Empty<LocationDto>();
        var vm = _mapper.Map<IEnumerable<LocationViewModel>>(dtos);

        var citiesRes = await _mediator.Send(new GetAllCitiesQuery());
        var cities = citiesRes.IsSuccess ? citiesRes.Value : Enumerable.Empty<FacultySports.Contracts.City.CityDto>();
        var cityDict = cities.ToDictionary(c => c.Id, c => c.Name);
        foreach (var l in vm)
        {
            if (l.CityId.HasValue && cityDict.TryGetValue(l.CityId.Value, out var cname)) l.CityName = cname;
        }

        return View(vm);
    }

    public async Task<IActionResult> Create()
    {
        var citiesRes = await _mediator.Send(new GetAllCitiesQuery());
        ViewBag.Cities = citiesRes.IsSuccess ? citiesRes.Value : Enumerable.Empty<Contracts.City.CityDto>();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateLocationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var citiesRes = await _mediator.Send(new GetAllCitiesQuery());
            ViewBag.Cities = citiesRes.IsSuccess ? citiesRes.Value : Enumerable.Empty<Contracts.City.CityDto>();
            return View(model);
        }

        var dto = _mapper.Map<CreateLocationDto>(model);
        var result = await _mediator.Send(new CreateLocationCommand(dto));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        var citiesRes2 = await _mediator.Send(new GetAllCitiesQuery());
        ViewBag.Cities = citiesRes2.IsSuccess ? citiesRes2.Value : Enumerable.Empty<Contracts.City.CityDto>();
        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var res = await _mediator.Send(new GetLocationByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<LocationViewModel>(res.Value);
        if (vm.CityId.HasValue)
        {
            var cityRes = await _mediator.Send(new Application.Queries.City.GetById.GetCityByIdQuery(vm.CityId.Value));
            if (cityRes.IsSuccess) vm.CityName = cityRes.Value.Name;
        }
        return View(vm);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var res = await _mediator.Send(new GetLocationByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<CreateLocationViewModel>(res.Value);
        var citiesRes = await _mediator.Send(new GetAllCitiesQuery());
        ViewBag.Cities = citiesRes.IsSuccess ? citiesRes.Value : Enumerable.Empty<Contracts.City.CityDto>();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CreateLocationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var citiesRes = await _mediator.Send(new GetAllCitiesQuery());
            ViewBag.Cities = citiesRes.IsSuccess ? citiesRes.Value : Enumerable.Empty<Contracts.City.CityDto>();
            return View(model);
        }

        var dto = _mapper.Map<UpdateLocationDto>(model);
        dto.Id = id;
        var result = await _mediator.Send(new UpdateLocationCommand(dto));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        var citiesRes2 = await _mediator.Send(new GetAllCitiesQuery());
        ViewBag.Cities = citiesRes2.IsSuccess ? citiesRes2.Value : Enumerable.Empty<Contracts.City.CityDto>();
        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var res = await _mediator.Send(new GetLocationByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<LocationViewModel>(res.Value);
        if (vm.CityId.HasValue)
        {
            var cityRes = await _mediator.Send(new Application.Queries.City.GetById.GetCityByIdQuery(vm.CityId.Value));
            if (cityRes.IsSuccess) vm.CityName = cityRes.Value.Name;
        }
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _mediator.Send(new DeleteLocationCommand(id));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        var res = await _mediator.Send(new GetLocationByIdQuery(id));
        var vm = res.IsSuccess ? _mapper.Map<LocationViewModel>(res.Value) : null;
        return View(vm);
    }
}
