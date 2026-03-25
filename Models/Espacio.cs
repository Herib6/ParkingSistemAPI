namespace ParkingSystemAPI.Models
{
    public class Espacio
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public string? Estado { get; set; } // "Disponible", "Transiendo", "Ocupado"
        public string? TipoEspacio { get; set; } = "Carro"; // "Carro" o "Moto"
        public int SensorId { get; set; }
        public bool SensorActivado { get; set; } = false; // True = sensor detecta ocupación
        public DateTime? FechaTransicion { get; set; } // Para controlar el tiempo de transición (15 seg)
    }
}
