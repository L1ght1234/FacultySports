using System.ComponentModel.DataAnnotations;

namespace FacultySports.MVC.Models.Schedule;

public class CreateScheduleViewModel
{
    [Range(0,6)]
    public int DayOfWeek { get; set; }

    [Required]
    [DataType(DataType.Time)]
    public TimeOnly StartTime { get; set; }

    [Required]
    [DataType(DataType.Time)]
    public TimeOnly EndTime { get; set; }

    public int? LocationId { get; set; }
}
