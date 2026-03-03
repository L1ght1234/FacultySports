namespace FacultySports.Domain.Entities;

public partial class Participant
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();
}
