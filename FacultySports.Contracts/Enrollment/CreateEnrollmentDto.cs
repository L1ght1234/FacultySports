namespace FacultySports.Contracts.Enrollment;

public class CreateEnrollmentDto
{
    public int SectionId { get; set; }
    public int ParticipantId { get; set; }
    public DateTime? Date { get; set; }
}
