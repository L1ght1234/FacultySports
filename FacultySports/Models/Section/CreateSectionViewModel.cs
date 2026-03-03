using System.ComponentModel.DataAnnotations;

namespace FacultySports.MVC.Models.Section;

public class CreateSectionViewModel
{
    // Validation attributes work on server and client
    [Required(ErrorMessage = "Section name is required")]
    [MaxLength(255, ErrorMessage = "Section name is too long")]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}