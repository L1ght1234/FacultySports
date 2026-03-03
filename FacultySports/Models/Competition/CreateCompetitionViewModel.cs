using System.ComponentModel.DataAnnotations;

namespace FacultySports.MVC.Models.Competition;

public class CreateCompetitionViewModel
{
    public int? SectionId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateOnly Date { get; set; }

    [Required]
    [DataType(DataType.Time)]
    public TimeOnly StartTime { get; set; }

    public int? StatusId { get; set; }
    public int? LocationId { get; set; }
}
