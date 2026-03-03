using AutoMapper;
using FacultySports.Contracts.Enrollment;
using FacultySports.Contracts.Participant;
using FacultySports.MVC.Models.Enrollment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FacultySports.MVC.Controllers;

public class EnrollmentsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public EnrollmentsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _mediator.Send(new Application.Queries.Enrollment.GetAll.GetAllEnrollmentsQuery());
        var dtos = result.IsSuccess ? result.Value : Enumerable.Empty<EnrollmentDto>();
        var vm = _mapper.Map<IEnumerable<EnrollmentViewModel>>(dtos);
        var participantsRes = await _mediator.Send(new Application.Queries.Participant.GetAll.GetAllParticipantsQuery());
        var sectionsRes = await _mediator.Send(new Application.Queries.Section.GetAll.GetAllSectionsQuery());
        var participants = participantsRes.IsSuccess ? participantsRes.Value : Enumerable.Empty<ParticipantDto>();
        var sections = sectionsRes.IsSuccess ? sectionsRes.Value : Enumerable.Empty<Contracts.Section.SectionDto>();

        var participantDict = participants.ToDictionary(p => p.Id, p => p.FirstName + " " + p.LastName);
        var sectionDict = sections.ToDictionary(s => s.Id, s => s.Name);

        foreach (var e in vm)
        {
            participantDict.TryGetValue(e.ParticipantId, out var pname);
            sectionDict.TryGetValue(e.SectionId, out var sname);
            e.ParticipantName = pname ?? string.Empty;
            e.SectionName = sname ?? string.Empty;
        }

        return View(vm);
    }

    public async Task<IActionResult> Create(int? sectionId)
    {
        var model = new CreateEnrollmentViewModel
        {
            Date = DateTime.Now
        };
        if (sectionId.HasValue)
        {
            var sectionRes = await _mediator.Send(new Application.Queries.Section.GetById.GetSectionByIdQuery(sectionId.Value));
            if (sectionRes.IsFailed)
                return NotFound();
            ViewBag.SectionName = sectionRes.Value.Name;
            model.SectionId = sectionId.Value;
        }
        else
        {
            var sectionsRes = await _mediator.Send(new Application.Queries.Section.GetAll.GetAllSectionsQuery());
            ViewBag.Sections = sectionsRes.IsSuccess ? sectionsRes.Value : Enumerable.Empty<Contracts.Section.SectionDto>();
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEnrollmentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var sectionRes = await _mediator.Send(new Application.Queries.Section.GetById.GetSectionByIdQuery(model.SectionId));
            ViewBag.SectionName = sectionRes.IsSuccess ? sectionRes.Value.Name : "";
            return View(model);
        }

        if (string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName))
        {
            ModelState.AddModelError(string.Empty, "Please provide participant first and last name.");
            var sectionRes = await _mediator.Send(new Application.Queries.Section.GetById.GetSectionByIdQuery(model.SectionId));
            ViewBag.SectionName = sectionRes.IsSuccess ? sectionRes.Value.Name : "";
            return View(model);
        }

        var findQuery = new Application.Queries.Participant.GetAll.GetAllParticipantsQuery();
        var allParticipantsRes = await _mediator.Send(findQuery);
        int participantId = 0;
        if (allParticipantsRes.IsSuccess)
        {
            var match = allParticipantsRes.Value.FirstOrDefault(p =>
                string.Equals(p.FirstName.Trim(), model.FirstName.Trim(), StringComparison.OrdinalIgnoreCase)
                && string.Equals(p.LastName.Trim(), model.LastName.Trim(), StringComparison.OrdinalIgnoreCase)
                && (string.IsNullOrWhiteSpace(model.Phone) || string.Equals(p.Phone?.Trim() ?? "", model.Phone.Trim(), StringComparison.OrdinalIgnoreCase))
            );
            if (match != null)
                participantId = match.Id;
        }

        if (participantId == 0)
        {
            var createParticipantDto = new CreateParticipantDto
            {
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                Phone = model.Phone
            };
            var createResult = await _mediator.Send(new Application.Commands.Participant.Create.CreateParticipantCommand(createParticipantDto));
            if (createResult.IsFailed)
            {
                ModelState.AddModelError(string.Empty, string.Join(';', createResult.Errors.Select(e => e.Message)));
                var sectionRes = await _mediator.Send(new Application.Queries.Section.GetById.GetSectionByIdQuery(model.SectionId));
                ViewBag.SectionName = sectionRes.IsSuccess ? sectionRes.Value.Name : "";
                return View(model);
            }
            participantId = createResult.Value.Id;
        }

        var allEnrollmentsRes = await _mediator.Send(new Application.Queries.Enrollment.GetAll.GetAllEnrollmentsQuery());
        bool alreadyEnrolled = false;
        if (allEnrollmentsRes.IsSuccess)
        {
            alreadyEnrolled = allEnrollmentsRes.Value.Any(e => e.SectionId == model.SectionId && e.ParticipantId == participantId);
        }

        if (alreadyEnrolled)
        {
            return RedirectToAction(nameof(Index));
        }

        var dto = new CreateEnrollmentDto
        {
            SectionId = model.SectionId,
            ParticipantId = participantId,
            Date = model.Date
        };

        var result = await _mediator.Send(new Application.Commands.Enrollment.Create.CreateEnrollmentCommand(dto));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        var sectionResFinal = await _mediator.Send(new Application.Queries.Section.GetById.GetSectionByIdQuery(model.SectionId));
        ViewBag.SectionName = sectionResFinal.IsSuccess ? sectionResFinal.Value.Name : "";
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var res = await _mediator.Send(new Application.Queries.Enrollment.GetById.GetEnrollmentByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<CreateEnrollmentViewModel>(res.Value);

        var sections = await _mediator.Send(new Application.Queries.Section.GetAll.GetAllSectionsQuery());
        var participants = await _mediator.Send(new Application.Queries.Participant.GetAll.GetAllParticipantsQuery());
        ViewBag.Sections = sections.IsSuccess ? sections.Value : Enumerable.Empty<FacultySports.Contracts.Section.SectionDto>();
        ViewBag.Participants = participants.IsSuccess ? participants.Value : Enumerable.Empty<FacultySports.Contracts.Participant.ParticipantDto>();

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CreateEnrollmentViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var dto = _mapper.Map<UpdateEnrollmentDto>(model);
        dto.Id = id;
        var result = await _mediator.Send(new Application.Commands.Enrollment.Update.UpdateEnrollmentCommand(dto));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        var sections = await _mediator.Send(new Application.Queries.Section.GetAll.GetAllSectionsQuery());
        var participants = await _mediator.Send(new Application.Queries.Participant.GetAll.GetAllParticipantsQuery());
        ViewBag.Sections = sections.IsSuccess ? sections.Value : Enumerable.Empty<Contracts.Section.SectionDto>();
        ViewBag.Participants = participants.IsSuccess ? participants.Value : Enumerable.Empty<ParticipantDto>();
        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var res = await _mediator.Send(new Application.Queries.Enrollment.GetById.GetEnrollmentByIdQuery(id));
        if (res.IsFailed) return NotFound();
        var vm = _mapper.Map<EnrollmentViewModel>(res.Value);
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _mediator.Send(new Application.Commands.Enrollment.Delete.DeleteEnrollmentCommand(id));
        if (result.IsSuccess) return RedirectToAction(nameof(Index));
        ModelState.AddModelError(string.Empty, string.Join(';', result.Errors.Select(e => e.Message)));
        var res = await _mediator.Send(new Application.Queries.Enrollment.GetById.GetEnrollmentByIdQuery(id));
        var vm = res.IsSuccess ? _mapper.Map<EnrollmentViewModel>(res.Value) : null;
        return View(vm);
    }
}
