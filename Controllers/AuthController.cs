using Microsoft.AspNetCore.Mvc;
using ParkingSystemAPI.Services;

namespace ParkingSystemAPI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, IEmailService emailService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UsuarioId") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string contrasena, string? codigoVerificacion = null)
        {
            // Si se envía código, intenta verificar
            if (!string.IsNullOrWhiteSpace(codigoVerificacion))
            {
                var verificado = await _authService.VerificarEmailAsync(email, codigoVerificacion);
                if (verificado)
                {
                    TempData["Success"] = "Email verificado! Ahora ya puedes hacer login.";
                    ViewBag.Email = email;
                    return View();
                }
                else
                {
                    ViewBag.Error = "Código de verificación inválido";
                    ViewBag.Email = email;
                    ViewBag.MostrarCodigoVerificacion = true;
                    return View();
                }
            }

            var (success, message, usuario) = await _authService.LoginAsync(email, contrasena);

            if (success && usuario != null)
            {
                HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
                HttpContext.Session.SetString("Email", usuario.Email);
                HttpContext.Session.SetString("NombreUsuario", usuario.Nombre);
                HttpContext.Session.SetString("Rol", usuario.Rol);
                
                TempData["Success"] = "Login exitoso";
                return RedirectToAction("Index", "Home");
            }

            // Si el error menciona verificación, mostrar campo de código
            if (message.Contains("verifica tu email"))
            {
                ViewBag.Email = email;
                ViewBag.MostrarCodigoVerificacion = true;
                ViewBag.Error = message;
                return View();
            }

            ViewBag.Error = message;
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UsuarioId") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string nombre, string email, string contrasena, string confirmarContrasena)
        {
            if (contrasena != confirmarContrasena)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            var (success, message, usuario) = await _authService.RegistrarAsync(nombre, email, contrasena);

            if (success)
            {
                TempData["Success"] = message + " Redirigiendo a verificación...";
                return RedirectToAction("VerificarEmail", new { email });
            }

            ViewBag.Error = message;
            return View();
        }

        [HttpGet]
        public IActionResult VerificarEmail(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerificarEmail(string email, string codigo)
        {
            var verificado = await _authService.VerificarEmailAsync(email, codigo);

            if (verificado)
            {
                TempData["Success"] = "Email verificado correctamente. Ahora puedes hacer login.";
                return RedirectToAction("Login");
            }

            ViewBag.Error = "Código de verificación inválido";
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReenviarCodigo(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Json(new { success = false, message = "Email es requerido" });

            var usuario = await _authService.ObtenerUsuarioPorEmailAsync(email);

            if (usuario == null)
                return Json(new { success = false, message = "Email no encontrado" });

            if (usuario.EmailVerificado)
                return Json(new { success = false, message = "Este email ya está verificado" });

            // Reenviar correo con código existente
            if (!string.IsNullOrEmpty(usuario.CodigoVerificacion))
            {
                await _emailService.EnviarCodigoVerificacionAsync(email, usuario.CodigoVerificacion);
                return Json(new { success = true, message = "Código reenviado a tu email" });
            }

            return Json(new { success = false, message = "No hay código pendiente" });
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Sesión cerrada correctamente";
            return RedirectToAction("Login");
        }
    }
}
