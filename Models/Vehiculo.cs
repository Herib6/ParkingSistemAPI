namespace ParkingSystemAPI.Models
{
    using System.Text.Json.Serialization;

    public class Vehiculo
    {
        public int Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Estado { get; set; } = "Fuera"; // "Dentro" o "Fuera"
        
        // Campos de auditoría
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaModificacion { get; set; }
        public int? UsuarioIdCreacion { get; set; }
        public int? UsuarioIdModificacion { get; set; }
        
        // Relaciones
        [JsonIgnore]
        public virtual List<Registro> Registros { get; set; } = new();
    }
}