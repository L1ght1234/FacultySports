namespace FacultySports.Contracts.Location;

public class UpdateLocationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? CityId { get; set; }
}
