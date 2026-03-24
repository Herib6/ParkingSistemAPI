namespace ParkingSystemAPI.Models
{
    using System.Text.Json.Serialization;

    public class Registro
    {
        public int Id { get; set; }
        public int VehiculoId { get; set; }
        public int EspacioId { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime? FechaSalida { get; set; }
        public decimal? Costo { get; set; } // Costo cobrado al salir

        // Relaciones de navegación
        [JsonIgnore]
        public virtual Vehiculo? Vehiculo { get; set; }
        
        [JsonIgnore]
        public virtual Espacio? Espacio { get; set; }
    }
}

