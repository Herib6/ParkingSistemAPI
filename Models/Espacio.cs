namespace ParkingSystemAPI.Models
{
    public class Espacio
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public string? Estado { get; set; }
        public string? TipoEspacio { get; set; } = "Carro"; // "Carro" o "Moto"
        public int SensorId { get; set; }
        public bool SensorActivado { get; set; } = false; // True = sensor detecta ocupación
    }
}
