namespace FacultySports.Contracts.Participant;

public class CreateParticipantDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Phone { get; set; }
}
