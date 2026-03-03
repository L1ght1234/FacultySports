namespace FacultySports.Domain.Entities;

public partial class Enrollment
{
    public int Id { get; set; }

    public int SectionId { get; set; }

    public int ParticipantId { get; set; }

    public DateTime? Date { get; set; }

    public virtual Participant Participant { get; set; } = null!;

    public virtual Section Section { get; set; } = null!;
}
