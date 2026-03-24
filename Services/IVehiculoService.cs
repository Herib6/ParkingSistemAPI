using ParkingSystemAPI.Models;

namespace ParkingSystemAPI.Services
{
    /// <summary>
    /// Interfaz para el servicio de negocio de Vehículos
    /// Encapsula las reglas de negocio y validaciones
    /// </summary>
    public interface IVehiculoService
    {
        // Métodos de lectura
        Task<List<Vehiculo>> GetAllVehiculosAsync();
        Task<Vehiculo?> GetVehiculoByIdAsync(int id);
        Task<Vehiculo?> GetVehiculoByPlacaAsync(string placa);
        Task<List<Vehiculo>> GetVehiculosPorEstadoAsync(string estado);
        Task<List<Vehiculo>> BuscarVehiculosAsync(string? buscar, string? tipo, string? estado, int? page = null, int pageSize = 10);
        Task<int> GetTotalVehiculosAsync();

        // Métodos de escritura con validaciones
        Task<(bool success, string message, Vehiculo? vehiculo)> CrearVehiculoAsync(Vehiculo vehiculo, int usuarioId);
        Task<(bool success, string message, Vehiculo? vehiculo)> ActualizarVehiculoAsync(Vehiculo vehiculo, int usuarioId);
        Task<(bool success, string message)> EliminarVehiculoAsync(int vehiculoId);

        // Validaciones específicas
        Task<(bool success, string message)> RegistrarEntradaAsync(int vehiculoId, int espacioId);
        Task<(bool success, string message)> RegistrarSalidaAsync(int vehiculoId);

        // Métodos de consulta
        Task<List<string>> GetTiposVehiculoAsync();
        Task<bool> PuedeSalirAsync(int vehiculoId);
    }
}
