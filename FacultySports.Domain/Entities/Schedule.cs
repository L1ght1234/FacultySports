namespace FacultySports.Domain.Entities;

public partial class Schedule
{
    public int Id { get; set; }

    public int DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int? LocationId { get; set; }

    public virtual Location? Location { get; set; }

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}
