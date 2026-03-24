using Microsoft.AspNetCore.Mvc;
using ParkingSystemAPI.Data;

namespace ParkingSystemAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
                return RedirectToAction("Login", "Auth");
            
            // Obtener estadísticas de la base de datos
            var totalRegistros = _context.Registros.Count();
            var totalVehiculos = _context.Vehiculos.Count();
            var espaciosDisponibles = _context.Espacios.Count(e => e.Estado == "Disponible");
            
            ViewBag.TotalRegistros = totalRegistros;
            ViewBag.TotalVehiculos = totalVehiculos;
            ViewBag.EspaciosDisponibles = espaciosDisponibles;
            
            return View();
        }
    }
}

