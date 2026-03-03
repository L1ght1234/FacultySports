using System.ComponentModel.DataAnnotations;

namespace FacultySports.MVC.Models.CompetitionStatus;

public class CreateCompetitionStatusViewModel
{
    [Required]
    public string Code { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;
}
