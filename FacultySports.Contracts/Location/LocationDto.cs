namespace FacultySports.Contracts.Location;

public class LocationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? CityId { get; set; }
}
