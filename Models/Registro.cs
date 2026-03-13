namespace ParkingSystemAPI.Models
{
    public class Registro
    {
        public int Id { get; set; }
        public int VehiculoId { get; set; }
        public int EspacioId { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime? FechaSalida { get; set; }
    }
}
