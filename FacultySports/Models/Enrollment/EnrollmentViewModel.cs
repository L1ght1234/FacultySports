namespace FacultySports.MVC.Models.Enrollment;

public class EnrollmentViewModel
{
    public int Id { get; set; }
    public int SectionId { get; set; }
    public string SectionName { get; set; } = string.Empty;
    public int ParticipantId { get; set; }
    public string ParticipantName { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
}
