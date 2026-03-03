namespace FacultySports.Contracts.Section;

public class SectionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? ScheduleId { get; set; }
}
