namespace ParkingSystemAPI.Models
{
    public class Espacio
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public string? Estado { get; set; }
        public int SensorId { get; set; }
    }
}
