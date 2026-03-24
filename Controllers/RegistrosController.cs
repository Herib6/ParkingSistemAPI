using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingSystemAPI.Data;

namespace ParkingSystemAPI.Controllers
{
    public class RegistrosController : Controller
    {
        private readonly AppDbContext _context;

        public RegistrosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string buscar = "", string fechaInicio = "", string fechaFin = "")
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
                return RedirectToAction("Login", "Auth");

            var registros = _context.Registros.Include(r => r.Vehiculo).AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
            {
                buscar = buscar.ToUpper().Trim();
                registros = registros.Where(r => 
                    r.VehiculoId.ToString().Contains(buscar) || 
                    r.Vehiculo.Placa.ToUpper().Contains(buscar));
            }

            if (!string.IsNullOrEmpty(fechaInicio) && DateTime.TryParse(fechaInicio, out var inicio))
                registros = registros.Where(r => r.FechaEntrada >= inicio);

            if (!string.IsNullOrEmpty(fechaFin) && DateTime.TryParse(fechaFin, out var fin))
                registros = registros.Where(r => r.FechaEntrada <= fin.AddDays(1));

            var registrosList = registros.OrderByDescending(r => r.FechaEntrada).ToList();

            ViewBag.Buscar = buscar;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;
            ViewBag.Total = registrosList.Count();
            ViewBag.Activos = registrosList.Count(r => !r.FechaSalida.HasValue);
            ViewBag.TotalCobrado = registrosList.Where(r => r.Costo.HasValue).Sum(r => r.Costo) ?? 0;
            
            return View(registrosList);
        }
    }
}
