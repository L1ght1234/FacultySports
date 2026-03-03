namespace FacultySports.Contracts.Location;

public class CreateLocationDto
{
    public string Name { get; set; } = null!;
    public int? CityId { get; set; }
}
