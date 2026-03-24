using Microsoft.AspNetCore.Mvc;
using ParkingSystemAPI.Models;
using ParkingSystemAPI.Services;
using System.ComponentModel.DataAnnotations;

namespace ParkingSystemAPI.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly IVehiculoService _vehiculoService;
        private readonly IVehiculoRepository _repository;

        public VehiculosController(IVehiculoService vehiculoService, IVehiculoRepository repository)
        {
            _vehiculoService = vehiculoService;
            _repository = repository;
        }

        // ==================== LISTAR Y BUSCAR ====================

        public async Task<IActionResult> Index(string? buscar = "", string? tipo = "", string? estado = "", int page = 1)
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
                return RedirectToAction("Login", "Auth");

            const int pageSize = 12;

            // Buscar vehículos
            var vehiculos = await _vehiculoService.BuscarVehiculosAsync(buscar, tipo, estado, page, pageSize);
            var total = await _vehiculoService.GetTotalVehiculosAsync();
            var tipos = await _vehiculoService.GetTiposVehiculoAsync();

            ViewBag.Buscar = buscar ?? "";
            ViewBag.Tipo = tipo ?? "";
            ViewBag.Estado = estado ?? "";
            ViewBag.Tipos = tipos;
            ViewBag.Total = total;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)total / pageSize);

            return View(vehiculos);
        }

        // ==================== CREAR ====================

        public IActionResult Create()
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
                return RedirectToAction("Login", "Auth");

            ViewBag.Tipos = new[] { "Auto", "SUV", "Moto", "Camión", "Otro" };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehiculo model)
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                var (success, message, vehiculo) = await _vehiculoService.CrearVehiculoAsync(model, int.Parse(usuarioId));
                
                if (success)
                {
                    TempData["Success"] = message;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", message);
                }
            }

            ViewBag.Tipos = new[] { "Auto", "SUV", "Moto", "Camión", "Otro" };
            return View(model);
        }

        // ==================== EDITAR ====================

        public async Task<IActionResult> Edit(int id)
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
                return RedirectToAction("Login", "Auth");

            var vehiculo = await _vehiculoService.GetVehiculoByIdAsync(id);
            if (vehiculo == null)
                return NotFound();

            ViewBag.Tipos = new[] { "Auto", "SUV", "Moto", "Camión", "Otro" };
            ViewBag.Estados = new[] { "Dentro", "Fuera" };
            return View(vehiculo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehiculo model)
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
                return RedirectToAction("Login", "Auth");

            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var (success, message, vehiculo) = await _vehiculoService.ActualizarVehiculoAsync(model, int.Parse(usuarioId));
                
                if (success)
                {
                    TempData["Success"] = message;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", message);
                }
            }

            ViewBag.Tipos = new[] { "Auto", "SUV", "Moto", "Camión", "Otro" };
            ViewBag.Estados = new[] { "Dentro", "Fuera" };
            return View(model);
        }

        // ==================== ELIMINAR ====================

        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
                return RedirectToAction("Login", "Auth");

            var vehiculo = await _vehiculoService.GetVehiculoByIdAsync(id);
            if (vehiculo == null)
                return NotFound();

            return View(vehiculo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
                return RedirectToAction("Login", "Auth");

            var (success, message) = await _vehiculoService.EliminarVehiculoAsync(id);
            
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }

        // ==================== DETALLES ====================

        public async Task<IActionResult> Details(int id)
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
                return RedirectToAction("Login", "Auth");

            var vehiculo = await _vehiculoService.GetVehiculoByIdAsync(id);
            if (vehiculo == null)
                return NotFound();

            return View(vehiculo);
        }

        // ==================== API ENDPOINTS ====================

        [HttpGet]
        public async Task<JsonResult> GetVehiculosPorEspacio(int espacioId)
        {
            var vehiculoAdentro = await _repository.GetByEstadoAsync("Dentro");
            return Json(vehiculoAdentro);
        }

        [HttpGet]
        public async Task<JsonResult> GetVehiculosDisponibles()
        {
            var vehiculosDisponibles = await _repository.GetByEstadoAsync("Fuera");
            return Json(new { success = true, vehiculos = vehiculosDisponibles });
        }

        [HttpPost]
        public async Task<JsonResult> ValidarPlaca(string placa, int? excludeId = null)
        {
            var existe = await _repository.ExistsByPlacaAsync(placa, excludeId);
            return Json(new { disponible = !existe, existe = existe });
        }

        [HttpPost]
        public async Task<JsonResult> RegistrarEntrada(int vehiculoId, int espacioId)
        {
            var (success, message) = await _vehiculoService.RegistrarEntradaAsync(vehiculoId, espacioId);
            return Json(new { success, message });
        }

        [HttpPost]
        public async Task<JsonResult> RegistrarSalida(int vehiculoId)
        {
            var (success, message) = await _vehiculoService.RegistrarSalidaAsync(vehiculoId);
            return Json(new { success, message });
        }

        [HttpPost]
        public async Task<JsonResult> CrearVehiculoApi(string placa, string tipo, string color)
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId))
                return Json(new { success = false, message = "No autenticado" });

            // Limpiar y validar datos
            placa = placa?.Trim().ToUpper() ?? "";
            tipo = tipo?.Trim() ?? "";
            color = color?.Trim() ?? "";

            if (string.IsNullOrEmpty(placa) || placa.Length < 3 || placa.Length > 8)
                return Json(new { success = false, message = "Placa inválida (3-8 caracteres)" });

            if (string.IsNullOrEmpty(tipo))
                return Json(new { success = false, message = "Tipo requerido" });

            if (string.IsNullOrEmpty(color))
                return Json(new { success = false, message = "Color requerido" });

            // Validar que la placa sea única
            var existe = await _repository.ExistsByPlacaAsync(placa);
            if (existe)
                return Json(new { success = false, message = "Esta placa ya está registrada" });

            // Crear vehículo
            var vehiculo = new Vehiculo
            {
                Placa = placa,
                Tipo = tipo,
                Color = color,
                Estado = "Fuera",
                FechaCreacion = DateTime.Now,
                UsuarioIdCreacion = int.Parse(usuarioId)
            };

            var (crearSuccess, crearMessage, vehiculoCreado) = await _vehiculoService.CrearVehiculoAsync(vehiculo, int.Parse(usuarioId));
            
            if (crearSuccess)
            {
                return Json(new { success = true, message = "Vehículo creado exitosamente", vehiculo = vehiculoCreado });
            }
            else
            {
                return Json(new { success = false, message = crearMessage });
            }
        }
    }
}
