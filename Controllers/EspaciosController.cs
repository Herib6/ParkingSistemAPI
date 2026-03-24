using Microsoft.AspNetCore.Mvc;
using ParkingSystemAPI.Data;
using ParkingSystemAPI.Models;
using ParkingSystemAPI.Services;

namespace ParkingSystemAPI.Controllers
{
    public class EspaciosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IVehiculoService _vehiculoService;

        public EspaciosController(AppDbContext context, IVehiculoService vehiculoService)
        {
            _context = context;
            _vehiculoService = vehiculoService;
        }

        public IActionResult Index()
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
                return RedirectToAction("Login", "Auth");

            var espacios = _context.Espacios.ToList();
            var registros = _context.Registros.OrderByDescending(r => r.FechaEntrada).Take(10).ToList();
            
            ViewBag.Registros = registros;
            ViewBag.EspaciosDisponibles = espacios.Count(e => e.Estado == "Disponible");
            ViewBag.EspaciosOcupados = espacios.Count(e => e.Estado == "Ocupado");
            
            return View(espacios);
        }

        [HttpGet]
        public IActionResult GetVehiculosPorEspacio(int espacioId)
        {
            // Obtener el vehículo que está actualmente en el espacio (sin salida)
            var registro = _context.Registros
                .Where(r => r.EspacioId == espacioId && !r.FechaSalida.HasValue)
                .OrderByDescending(r => r.FechaEntrada)
                .FirstOrDefault();

            if (registro != null)
            {
                var vehiculo = _context.Vehiculos.FirstOrDefault(v => v.Id == registro.VehiculoId);
                if (vehiculo != null)
                {
                    return Json(new { success = true, vehiculo = vehiculo });
                }
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public IActionResult GetVehiculosDisponibles()
        {
            // Obtener vehículos que NO están dentro (sin registro activo)
            var vehiculosAdentro = _context.Registros
                .Where(r => !r.FechaSalida.HasValue)
                .Select(r => r.VehiculoId)
                .Distinct()
                .ToList();

            var vehiculosDisponibles = _context.Vehiculos
                .Where(v => !vehiculosAdentro.Contains(v.Id))
                .ToList();

            return Json(new { success = true, vehiculos = vehiculosDisponibles });
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarEntrada(int vehiculoId, int espacioId)
        {
            try
            {
                var (success, message) = await _vehiculoService.RegistrarEntradaAsync(vehiculoId, espacioId);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarSalida(int vehiculoId)
        {
            try
            {
                // Obtener el registro activo
                var registro = _context.Registros
                    .Where(r => r.VehiculoId == vehiculoId && !r.FechaSalida.HasValue)
                    .FirstOrDefault();

                if (registro != null)
                {
                    // Calcular duración real
                    var ahora = DateTime.Now;
                    var duracion = ahora - registro.FechaEntrada;
                    var horasCompletas = (int)duracion.TotalHours;
                    var minutos = duracion.Minutes;
                    
                    // Calcular costo: libre si < 30 min, sino (100 × horas) + (50 si minutos > 30)
                    decimal costoCalculado = 0;
                    if (!(horasCompletas == 0 && minutos < 30))
                    {
                        costoCalculado = (100 * horasCompletas);
                        if (minutos > 30)
                        {
                            costoCalculado += 50;
                        }
                    }
                    
                    // Guardar el costo calculado
                    registro.Costo = costoCalculado;
                }

                var (success, message) = await _vehiculoService.RegistrarSalidaAsync(vehiculoId);
                
                return Json(new 
                { 
                    success, 
                    message
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpGet]
        public IActionResult GetRegistroInfo(int vehiculoId)
        {
            var registro = _context.Registros
                .Where(r => r.VehiculoId == vehiculoId && !r.FechaSalida.HasValue)
                .FirstOrDefault();

            if (registro != null)
            {
                var duracion = DateTime.Now - registro.FechaEntrada;
                var horasCompletas = (int)duracion.TotalHours;
                var minutos = duracion.Minutes;
                
                return Json(new 
                { 
                    success = true,
                    horasCompletas = horasCompletas,
                    minutos = minutos,
                    fechaEntrada = registro.FechaEntrada.ToString("HH:mm"),
                    tiempoFormato = $"{horasCompletas}h {minutos}m"
                });
            }

            return Json(new { success = false });
        }
    }
}
