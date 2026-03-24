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

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UsuarioId") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Register(string nombre, string email, string contrasena, string confirmarContrasena)
        {
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contrasena))
            {
                ViewBag.Error = "Todos los campos son requeridos";
                return View();
            }

            if (contrasena != confirmarContrasena)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            // Por ahora, registramos el usuario y lo logueamos automáticamente
            HttpContext.Session.SetString("UsuarioId", email);
            HttpContext.Session.SetString("NombreUsuario", nombre);
            HttpContext.Session.SetString("Rol", "Usuario");
            
            return RedirectToAction("Index", "Home");
        }
    }
}
