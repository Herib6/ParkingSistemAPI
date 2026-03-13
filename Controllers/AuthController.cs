using Microsoft.AspNetCore.Mvc;

namespace ParkingSystemAPI.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UsuarioId") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string contrasena)
        {
            // Credenciales de prueba
            if (email == "admin@parking.com" && contrasena == "admin123")
            {
                HttpContext.Session.SetString("UsuarioId", "1");
                HttpContext.Session.SetString("NombreUsuario", "Admin");
                HttpContext.Session.SetString("Rol", "Administrador");
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Email o contraseña incorrectos";
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
