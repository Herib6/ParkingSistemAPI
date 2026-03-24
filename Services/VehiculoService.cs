using ParkingSystemAPI.Data;
using ParkingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ParkingSystemAPI.Services
{
    /// <summary>
    /// Implementación del servicio de negocio de Vehículos
    /// Aplica todas las reglas de negocio y validaciones
    /// </summary>
    public class VehiculoService : IVehiculoService
    {
        private readonly IVehiculoRepository _repository;
        private readonly AppDbContext _context;

        public VehiculoService(IVehiculoRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        // ==================== LECTURA ====================

        public async Task<List<Vehiculo>> GetAllVehiculosAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Vehiculo?> GetVehiculoByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Vehiculo?> GetVehiculoByPlacaAsync(string placa)
        {
            return await _repository.GetByPlacaAsync(placa);
        }

        public async Task<List<Vehiculo>> GetVehiculosPorEstadoAsync(string estado)
        {
            var estadosValidos = new[] { "Dentro", "Fuera" };
            if (!estadosValidos.Contains(estado))
                return new List<Vehiculo>();

            return await _repository.GetByEstadoAsync(estado);
        }

        public async Task<List<Vehiculo>> BuscarVehiculosAsync(string? buscar, string? tipo, string? estado, int? page = null, int pageSize = 10)
        {
            return await _repository.SearchAsync(buscar, tipo, estado, page, pageSize);
        }

        public async Task<int> GetTotalVehiculosAsync()
        {
            return await _repository.GetTotalCountAsync();
        }

        public async Task<List<string>> GetTiposVehiculoAsync()
        {
            return await _context.Vehiculos
                .Select(v => v.Tipo)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();
        }

        // ==================== CREACIÓN ====================

        public async Task<(bool success, string message, Vehiculo? vehiculo)> CrearVehiculoAsync(Vehiculo vehiculo, int usuarioId)
        {
            try
            {
                // Validaciones
                var validacion = ValidarVehiculo(vehiculo);
                if (!validacion.success)
                    return (false, validacion.message, null);

                // Validar que la placa sea única
                if (await _repository.ExistsByPlacaAsync(vehiculo.Placa))
                    return (false, "La placa ya existe en el sistema", null);

                // Asegurar valores
                vehiculo.Estado = "Fuera";
                vehiculo.FechaCreacion = DateTime.Now;
                vehiculo.UsuarioIdCreacion = usuarioId;

                var resultado = await _repository.CreateAsync(vehiculo);
                return (true, "Vehículo creado exitosamente", resultado);
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear vehículo: {ex.Message}", null);
            }
        }

        // ==================== ACTUALIZACIÓN ====================

        public async Task<(bool success, string message, Vehiculo? vehiculo)> ActualizarVehiculoAsync(Vehiculo vehiculo, int usuarioId)
        {
            try
            {
                var existente = await _repository.GetByIdAsync(vehiculo.Id);
                if (existente == null)
                    return (false, "Vehículo no encontrado", null);

                // Validaciones
                var validacion = ValidarVehiculo(vehiculo);
                if (!validacion.success)
                    return (false, validacion.message, null);

                // Validar que la placa sea única (excluyendo el vehículo actual)
                if (vehiculo.Placa != existente.Placa && 
                    await _repository.ExistsByPlacaAsync(vehiculo.Placa, vehiculo.Id))
                    return (false, "La placa ya existe en el sistema", null);

                // Mantener datos de auditoría
                vehiculo.FechaCreacion = existente.FechaCreacion;
                vehiculo.UsuarioIdCreacion = existente.UsuarioIdCreacion;
                vehiculo.FechaModificacion = DateTime.Now;
                vehiculo.UsuarioIdModificacion = usuarioId;

                // No permitir cambiar estado manualmente directo aquí
                vehiculo.Estado = existente.Estado;

                var resultado = await _repository.UpdateAsync(vehiculo);
                return (true, "Vehículo actualizado exitosamente", resultado);
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar vehículo: {ex.Message}", null);
            }
        }

        // ==================== ELIMINACIÓN ====================

        public async Task<(bool success, string message)> EliminarVehiculoAsync(int vehiculoId)
        {
            try
            {
                var vehiculo = await _repository.GetByIdAsync(vehiculoId);
                if (vehiculo == null)
                    return (false, "Vehículo no encontrado");

                // Validar que no tenga registros activos
                if (await _repository.ExisteRegistroActivoAsync(vehiculoId))
                    return (false, "No se puede eliminar un vehículo con registro activo en el parqueo");

                // Validar que no tenga registros en los últimos 30 días (opcional)
                var tieneRegistrosRecientes = await _context.Registros
                    .AnyAsync(r => r.VehiculoId == vehiculoId && 
                                   r.FechaEntrada >= DateTime.Now.AddDays(-30));

                if (tieneRegistrosRecientes)
                    return (false, "No se puede eliminar un vehículo con registros recientes");

                await _repository.DeleteAsync(vehiculoId);
                return (true, "Vehículo eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar vehículo: {ex.Message}");
            }
        }

        // ==================== ESTADO Y PARQUEO ====================

        public async Task<(bool success, string message)> RegistrarEntradaAsync(int vehiculoId, int espacioId)
        {
            try
            {
                var vehiculo = await _repository.GetByIdAsync(vehiculoId);
                if (vehiculo == null)
                    return (false, "Vehículo no encontrado");

                // 🔹 REGLA 1: No permitir entrada si ya está "Dentro"
                if (vehiculo.Estado == "Dentro")
                    return (false, "El vehículo ya se encuentra dentro del parqueo");

                // 🔹 REGLA 2: Validar que no hay otro vehículo con la misma placa "Dentro"
                var otraInstancia = await _context.Registros
                    .Where(r => r.VehiculoId != vehiculoId && 
                               !r.FechaSalida.HasValue)
                    .Include(r => r.Vehiculo)
                    .FirstOrDefaultAsync(r => r.Vehiculo.Placa == vehiculo.Placa);

                if (otraInstancia != null)
                    return (false, "Existe otro vehículo con la misma placa activo en el parqueo");

                // 🔹 REGLA 3: Validar que el espacio esté disponible
                var espacio = await _context.Espacios.FirstOrDefaultAsync(e => e.Id == espacioId);
                if (espacio == null)
                    return (false, "Espacio no encontrado");

                if (espacio.Estado == "Ocupado")
                    return (false, "El espacio seleccionado ya está ocupado");

                // Validar que no haya otro vehículo en ese espacio
                var vehiculoEnEspacio = await _context.Registros
                    .AnyAsync(r => r.EspacioId == espacioId && !r.FechaSalida.HasValue);

                if (vehiculoEnEspacio)
                    return (false, "El espacio está ocupado por otro vehículo");

                // Crear registro de entrada
                var registro = new Registro
                {
                    VehiculoId = vehiculoId,
                    EspacioId = espacioId,
                    FechaEntrada = DateTime.Now,
                    FechaSalida = null
                };

                _context.Registros.Add(registro);

                // Actualizar estado del vehículo
                vehiculo.Estado = "Dentro";
                vehiculo.FechaModificacion = DateTime.Now;
                _context.Vehiculos.Update(vehiculo);

                // Actualizar estado del espacio
                espacio.Estado = "Ocupado";
                _context.Espacios.Update(espacio);

                await _context.SaveChangesAsync();
                return (true, "Entrada registrada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al registrar entrada: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> RegistrarSalidaAsync(int vehiculoId)
        {
            try
            {
                var vehiculo = await _repository.GetByIdAsync(vehiculoId);
                if (vehiculo == null)
                    return (false, "Vehículo no encontrado");

                // 🔹 REGLA 1: Validar que esté "Dentro" para poder salir
                if (vehiculo.Estado != "Dentro")
                    return (false, "El vehículo no está dentro del parqueo");

                // Obtener el registro activo
                var registro = await _context.Registros
                    .FirstOrDefaultAsync(r => r.VehiculoId == vehiculoId && !r.FechaSalida.HasValue);

                if (registro == null)
                    return (false, "No hay registro de entrada activo para este vehículo");

                // Actualizar registro de salida
                registro.FechaSalida = DateTime.Now;
                _context.Registros.Update(registro);

                // Actualizar estado del vehículo
                vehiculo.Estado = "Fuera";
                vehiculo.FechaModificacion = DateTime.Now;
                _context.Vehiculos.Update(vehiculo);

                // Liberar espacio
                var espacio = await _context.Espacios.FirstOrDefaultAsync(e => e.Id == registro.EspacioId);
                if (espacio != null)
                {
                    espacio.Estado = "Disponible";
                    _context.Espacios.Update(espacio);
                }

                await _context.SaveChangesAsync();
                return (true, "Salida registrada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al registrar salida: {ex.Message}");
            }
        }

        public async Task<bool> PuedeSalirAsync(int vehiculoId)
        {
            var vehiculo = await _repository.GetByIdAsync(vehiculoId);
            return vehiculo?.Estado == "Dentro";
        }

        // ==================== VALIDACIONES ====================

        private (bool success, string message) ValidarVehiculo(Vehiculo vehiculo)
        {
            if (string.IsNullOrWhiteSpace(vehiculo.Placa))
                return (false, "La placa es obligatoria");

            if (vehiculo.Placa.Length > 8)
                return (false, "La placa no debe exceder 8 caracteres");

            if (string.IsNullOrWhiteSpace(vehiculo.Tipo))
                return (false, "El tipo de vehículo es obligatorio");

            var tiposValidos = new[] { "Auto", "SUV", "Moto", "Camión", "Otro" };
            if (!tiposValidos.Contains(vehiculo.Tipo))
                return (false, "Tipo de vehículo inválido");

            if (string.IsNullOrWhiteSpace(vehiculo.Color))
                return (false, "El color es obligatorio");

            return (true, "Validación exitosa");
        }
    }
}
