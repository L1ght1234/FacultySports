namespace FacultySports.MVC.Models.Schedule;

public class ScheduleViewModel
{
    public int Id { get; set; }
    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int? LocationId { get; set; }
    public string? LocationName { get; set; }
}
