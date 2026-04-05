using FacultySports.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace FacultySports.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class ChartsController : ControllerBase
{
    private readonly SportsDbContext _context;

    public ChartsController(SportsDbContext context)
    {
        _context = context;
    }

    private record CompetitionsByCityResponse(string City, int Count);

    [HttpGet("competitionsByCity")]
    public async Task<JsonResult> GetCompetitionsByCity(CancellationToken cancellationToken)
    {
        var responseItems = await _context.Competitions
            .Where(c => c.Location != null && c.Location.City != null)
            .GroupBy(c => c.Location!.City!.Name)
            .Select(g => new CompetitionsByCityResponse(g.Key, g.Count()))
            .ToListAsync(cancellationToken);

        return new JsonResult(responseItems);
    }
    
    private record ParticipantsBySectionResponse(string Section, int Count);
    
    [HttpGet("participantsBySection")]
    public async Task<JsonResult> GetParticipantsBySection(CancellationToken cancellationToken)
    {
        var responseItems = await _context.Enrollments
            .Include(e => e.Section)
            .GroupBy(e => e.Section.Name)
            .Select(g => new ParticipantsBySectionResponse(g.Key, g.Count()))
            .ToListAsync(cancellationToken);

        return new JsonResult(responseItems);
    }
}
