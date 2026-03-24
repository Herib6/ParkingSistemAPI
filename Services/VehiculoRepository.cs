using Microsoft.EntityFrameworkCore;
using ParkingSystemAPI.Data;
using ParkingSystemAPI.Models;

namespace ParkingSystemAPI.Services
{
    /// <summary>
    /// Implementación del repositorio de Vehículos
    /// Maneja toda la lógica de acceso a datos
    /// </summary>
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly AppDbContext _context;

        public VehiculoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vehiculo>> GetAllAsync()
        {
            return await _context.Vehiculos
                .OrderByDescending(v => v.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Vehiculo?> GetByIdAsync(int id)
        {
            return await _context.Vehiculos
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vehiculo?> GetByPlacaAsync(string placa)
        {
            return await _context.Vehiculos
                .FirstOrDefaultAsync(v => v.Placa.ToUpper() == placa.ToUpper());
        }

        public async Task<List<Vehiculo>> GetByEstadoAsync(string estado)
        {
            return await _context.Vehiculos
                .Where(v => v.Estado == estado)
                .OrderByDescending(v => v.FechaCreacion)
                .ToListAsync();
        }

        public async Task<List<Vehiculo>> GetByTipoAsync(string tipo)
        {
            return await _context.Vehiculos
                .Where(v => v.Tipo == tipo)
                .OrderByDescending(v => v.FechaCreacion)
                .ToListAsync();
        }

        public async Task<List<Vehiculo>> SearchAsync(string? buscar, string? tipo, string? estado, int? page = null, int pageSize = 10)
        {
            var query = _context.Vehiculos.AsQueryable();

            // Filtro por búsqueda (placa)
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.ToUpper().Trim();
                query = query.Where(v => v.Placa.ToUpper().Contains(buscar));
            }

            // Filtro por tipo
            if (!string.IsNullOrWhiteSpace(tipo))
            {
                query = query.Where(v => v.Tipo == tipo);
            }

            // Filtro por estado
            if (!string.IsNullOrWhiteSpace(estado))
            {
                query = query.Where(v => v.Estado == estado);
            }

            // Ordenar
            query = query.OrderByDescending(v => v.FechaCreacion);

            // Paginación
            if (page.HasValue && page.Value > 0)
            {
                int skip = (page.Value - 1) * pageSize;
                query = query.Skip(skip).Take(pageSize);
            }

            return await query.ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Vehiculos.CountAsync();
        }

        public async Task<Vehiculo> CreateAsync(Vehiculo vehiculo)
        {
            vehiculo.Placa = vehiculo.Placa?.ToUpper().Trim() ?? string.Empty;
            vehiculo.FechaCreacion = DateTime.Now;
            vehiculo.Estado = "Fuera";

            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();
            return vehiculo;
        }

        public async Task<Vehiculo> UpdateAsync(Vehiculo vehiculo)
        {
            vehiculo.Placa = vehiculo.Placa?.ToUpper().Trim() ?? string.Empty;
            vehiculo.FechaModificacion = DateTime.Now;

            _context.Vehiculos.Update(vehiculo);
            await _context.SaveChangesAsync();
            return vehiculo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var vehiculo = await GetByIdAsync(id);
            if (vehiculo == null)
                return false;

            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByPlacaAsync(string placa, int? excludeId = null)
        {
            placa = placa.ToUpper().Trim();
            var query = _context.Vehiculos
                .Where(v => v.Placa.ToUpper() == placa);

            if (excludeId.HasValue)
                query = query.Where(v => v.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> ExisteRegistroActivoAsync(int vehiculoId)
        {
            return await _context.Registros
                .AnyAsync(r => r.VehiculoId == vehiculoId && !r.FechaSalida.HasValue);
        }

        public async Task<bool> UpdateEstadoAsync(int vehiculoId, string nuevoEstado)
        {
            var vehiculo = await GetByIdAsync(vehiculoId);
            if (vehiculo == null)
                return false;

            vehiculo.Estado = nuevoEstado;
            vehiculo.FechaModificacion = DateTime.Now;
            await UpdateAsync(vehiculo);
            return true;
        }
    }
}
