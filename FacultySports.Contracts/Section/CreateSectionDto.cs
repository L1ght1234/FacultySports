namespace FacultySports.Contracts.Section;

public class CreateSectionDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? ScheduleId { get; set; }
}
