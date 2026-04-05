using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FacultySports.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DiagramsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
