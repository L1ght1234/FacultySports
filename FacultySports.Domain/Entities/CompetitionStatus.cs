namespace FacultySports.Domain.Entities;

public partial class CompetitionStatus
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();
}
