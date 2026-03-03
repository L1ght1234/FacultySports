using AutoMapper;
using FacultySports.Application.Queries.Participant.GetAll;
using FacultySports.Application.Queries.Participant.GetById;
using FacultySports.Application.Commands.Participant.Create;
using FacultySports.Application.Commands.Participant.Update;
using FacultySports.Application.Commands.Participant.Delete;
using FacultySports.Contracts.Participant;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FacultySports.MVC.Models.Participant;

namespace FacultySports.MVC.Controllers;

public class ParticipantsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ParticipantsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var res = await _mediator.Send(new GetAllParticipantsQuery());
        var dtos = res.IsSuccess ? res.Value : Enumerable.Empty<ParticipantDto>();
        var vm = _mapper.Map<IEnumerable<ParticipantViewModel>>(dtos);
        return View(vm);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateParticipantViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = _mapper.Map<CreateParticipantDto>(model);
        var result = await _mediator.Send(new CreateParticipantCommand(dto));
        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var res = await _mediator.Send(new GetParticipantByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<ParticipantViewModel>(res.Value);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var res = await _mediator.Send(new GetParticipantByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<CreateParticipantViewModel>(res.Value);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CreateParticipantViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var dto = _mapper.Map<UpdateParticipantDto>(model);
        dto.Id = id;
        var result = await _mediator.Send(new UpdateParticipantCommand(dto));
        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var res = await _mediator.Send(new GetParticipantByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<ParticipantViewModel>(res.Value);
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _mediator.Send(new DeleteParticipantCommand(id));
        if (result.IsSuccess)
            return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        var res = await _mediator.Send(new GetParticipantByIdQuery(id));
        var vm = res.IsSuccess ? _mapper.Map<ParticipantViewModel>(res.Value) : null;
        return View(vm);
    }
}
