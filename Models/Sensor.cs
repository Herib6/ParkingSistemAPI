namespace ParkingSystemAPI.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public string? Ubicacion { get; set; }
        public string? Estado { get; set; }
        public string? TopicMqtt { get; set; }
        public DateTime UltimaActualizacion { get; set; }
    }
}
