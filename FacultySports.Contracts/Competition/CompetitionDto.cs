namespace FacultySports.Contracts.Competition;

public class CompetitionDto
{
    public int Id { get; set; }
    public int? SectionId { get; set; }
    public string? SectionName { get; set; }
    public string Title { get; set; } = null!;
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public int? StatusId { get; set; }
    public string? StatusName { get; set; }
    public int? LocationId { get; set; }
    public string? LocationName { get; set; }
}
