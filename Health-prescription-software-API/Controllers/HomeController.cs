using Microsoft.AspNetCore.Mvc;

namespace Health_prescription_software_API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
