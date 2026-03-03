namespace FacultySports.Contracts.Enrollment;

public class EnrollmentDto
{
    public int Id { get; set; }
    public int SectionId { get; set; }
    public int ParticipantId { get; set; }
    public DateTime? Date { get; set; }
}
