using Microsoft.AspNetCore.Mvc;

namespace BibliotecaCaso1.Controllers
{
    public class PrestamoLibrosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
