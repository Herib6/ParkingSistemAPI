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
        [HttpGet]
        public IActionResult GetVehiculosDisponibles(string? tipoEspacio = "Carro")
        {
            // Obtener vehículos que NO están dentro (sin registro activo)
            var vehiculosAdentro = _context.Registros
                .Where(r => !r.FechaSalida.HasValue)
                .Select(r => r.VehiculoId)
                .Distinct()
                .ToList();

            var vehiculosDisponibles = _context.Vehiculos
                .Where(v => !vehiculosAdentro.Contains(v.Id) && v.Tipo == tipoEspacio)
                .ToList();

            return Json(new { success = true, vehiculos = vehiculosDisponibles });
        }

        [HttpGet]
        public IActionResult GetTipoEspacio(int espacioId)
        {
            var espacio = _context.Espacios.FirstOrDefault(e => e.Id == espacioId);
            if (espacio != null)
            {
                return Json(new { success = true, tipoEspacio = espacio.TipoEspacio ?? "Carro" });
            }
            return Json(new { success = false, message = "Espacio no encontrado" });
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

        [HttpPost]
        public IActionResult SincronizarDatos()
        {
            try
            {
                // Actualizar vehículos Camioneta a Carro
                var vehiculosCamioneta = _context.Vehiculos.Where(v => v.Tipo == "Camioneta").ToList();
                foreach (var v in vehiculosCamioneta)
                {
                    v.Tipo = "Carro";
                }

                // Agregar espacios para motos si no existen
                var espaciosMoto = _context.Espacios.Where(e => e.TipoEspacio == "Moto").Count();
                if (espaciosMoto == 0)
                {
                    // Crear 10 espacios para motos
                    for (int i = 1; i <= 10; i++)
                    {
                        _context.Espacios.Add(new Espacio
                        {
                            Numero = 500 + i,
                            Estado = "Disponible",
                            TipoEspacio = "Moto",
                            SensorId = i
                        });
                    }
                }

                _context.SaveChanges();
                return Json(new { success = true, message = "Datos sincronizados correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost]
        public IActionResult GenerarEspaciosDePrueba()
        {
            try
            {
                // Obtener el número máximo actual de espacios
                var numeroMaximo = _context.Espacios.Max(e => (int?)e.Numero) ?? 0;
                var sensorMaximo = _context.Espacios.Max(e => (int?)e.SensorId) ?? 0;

                // Generar 20 espacios para carros
                for (int i = 1; i <= 20; i++)
                {
                    var numero = numeroMaximo + i;
                    var sensor = sensorMaximo + i;
                    
                    if (!_context.Espacios.Any(e => e.Numero == numero))
                    {
                        _context.Espacios.Add(new Espacio
                        {
                            Numero = numero,
                            Estado = "Disponible",
                            TipoEspacio = "Carro",
                            SensorId = sensor
                        });
                    }
                }

                // Generar 20 espacios para motos
                for (int i = 1; i <= 20; i++)
                {
                    var numero = numeroMaximo + 20 + i;
                    var sensor = sensorMaximo + 20 + i;
                    
                    if (!_context.Espacios.Any(e => e.Numero == numero))
                    {
                        _context.Espacios.Add(new Espacio
                        {
                            Numero = numero,
                            Estado = "Disponible",
                            TipoEspacio = "Moto",
                            SensorId = sensor
                        });
                    }
                }

                _context.SaveChanges();
                var totalCarros = _context.Espacios.Count(e => e.TipoEspacio == "Carro");
                var totalMotos = _context.Espacios.Count(e => e.TipoEspacio == "Moto");
                
                return Json(new 
                { 
                    success = true, 
                    message = $"Espacios de prueba generados correctamente. Total Carros: {totalCarros}, Total Motos: {totalMotos}" 
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
    }
}
