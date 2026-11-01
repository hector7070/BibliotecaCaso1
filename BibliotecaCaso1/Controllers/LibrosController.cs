using Microsoft.AspNetCore.Mvc;

namespace BibliotecaCaso1.Controllers
{
    public class LibrosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
