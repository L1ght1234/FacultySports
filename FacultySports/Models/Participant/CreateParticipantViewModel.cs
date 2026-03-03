using System.ComponentModel.DataAnnotations;

namespace FacultySports.MVC.Models.Participant;

public class CreateParticipantViewModel
{
    [Required]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Phone")]
    public string? Phone { get; set; }
}
