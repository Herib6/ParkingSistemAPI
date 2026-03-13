using Microsoft.AspNetCore.Mvc;

namespace ParkingSystemAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
                return RedirectToAction("Login", "Auth");
            
            return View();
        }
    }
}
