using System.ComponentModel.DataAnnotations;

namespace FacultySports.MVC.Models.City;

public class CreateCityViewModel
{
    [Required]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;
}
