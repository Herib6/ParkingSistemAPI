using ParkingSystemAPI.Models;

namespace ParkingSystemAPI.Services
{
    /// <summary>
    /// Interfaz para el repositorio de Vehículos
    /// Encapsula la lógica de acceso a datos
    /// </summary>
    public interface IVehiculoRepository
    {
        Task<List<Vehiculo>> GetAllAsync();
        Task<Vehiculo?> GetByIdAsync(int id);
        Task<Vehiculo?> GetByPlacaAsync(string placa);
        Task<List<Vehiculo>> GetByEstadoAsync(string estado);
        Task<List<Vehiculo>> GetByTipoAsync(string tipo);
        Task<List<Vehiculo>> SearchAsync(string? buscar, string? tipo, string? estado, int? page = null, int pageSize = 10);
        Task<int> GetTotalCountAsync();
        
        Task<Vehiculo> CreateAsync(Vehiculo vehiculo);
        Task<Vehiculo> UpdateAsync(Vehiculo vehiculo);
        Task<bool> DeleteAsync(int id);
        
        Task<bool> ExistsByPlacaAsync(string placa, int? excludeId = null);
        Task<bool> ExisteRegistroActivoAsync(int vehiculoId);
        Task<bool> UpdateEstadoAsync(int vehiculoId, string nuevoEstado);
    }
}
