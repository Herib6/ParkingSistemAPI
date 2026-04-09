namespace ParkingSystemAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContraseniaHash { get; set; } = string.Empty;
        public string Rol { get; set; } = "Usuario";
        
        // Auditoría
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public DateTime? FechaUltimoLogin { get; set; }
        public bool Activo { get; set; } = true;
        
        // Verificación de email
        public bool EmailVerificado { get; set; } = false;
        public string? CodigoVerificacion { get; set; }
        public DateTime? FechaVerificacion { get; set; }
    }
}
