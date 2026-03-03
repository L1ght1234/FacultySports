namespace FacultySports.Contracts.Competition;

public class CreateCompetitionDto
{
    public int? SectionId { get; set; }
    public string Title { get; set; } = null!;
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public int? StatusId { get; set; }
    public int? LocationId { get; set; }
}
