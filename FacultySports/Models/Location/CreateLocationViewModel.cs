using System.ComponentModel.DataAnnotations;

namespace FacultySports.MVC.Models.Location;

public class CreateLocationViewModel
{
    [Required]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "City")]
    public int? CityId { get; set; }
}
