namespace FacultySports.Contracts.Schedule;

public class CreateScheduleDto
{
    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int? LocationId { get; set; }
}
