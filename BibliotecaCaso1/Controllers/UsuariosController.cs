using Microsoft.AspNetCore.Mvc;

namespace BibliotecaCaso1.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
