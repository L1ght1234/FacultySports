namespace FacultySports.Domain.Entities;

public partial class Competition
{
    public int Id { get; set; }

    public int? SectionId { get; set; }

    public string Title { get; set; } = null!;

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public int? StatusId { get; set; }

    public int? LocationId { get; set; }

    public virtual Location? Location { get; set; }

    public virtual Section? Section { get; set; }

    public virtual CompetitionStatus? Status { get; set; }

    public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();
}
