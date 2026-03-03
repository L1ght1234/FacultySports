namespace FacultySports.Contracts.CompetitionStatus;

public class UpdateCompetitionStatusDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}
