using System.ComponentModel.DataAnnotations;

namespace FacultySports.MVC.Models.Enrollment;

public class CreateEnrollmentViewModel
{
    [Required]
    [Display(Name = "Section")]
    public int SectionId { get; set; }

    [Display(Name = "Participant")]
    public int ParticipantId { get; set; }

    [Display(Name = "New participant first name")]
    public string? FirstName { get; set; }

    [Display(Name = "New participant last name")]
    public string? LastName { get; set; }

    [Display(Name = "New participant phone")]
    public string? Phone { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date")]
    public DateTime? Date { get; set; }
}
