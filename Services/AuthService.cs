using ParkingSystemAPI.Data;
using ParkingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ParkingSystemAPI.Services
{
    public interface IAuthService
    {
        Task<(bool success, string message, Usuario? usuario)> RegistrarAsync(string nombre, string email, string contrasena);
        Task<(bool success, string message, Usuario? usuario)> LoginAsync(string email, string contrasena);
        Task<bool> VerificarEmailAsync(string email, string codigo);
        Task<Usuario?> ObtenerUsuarioAsync(int id);
        Task<Usuario?> ObtenerUsuarioPorEmailAsync(string email);
        Task<bool> ActualizarUltimoLoginAsync(int usuarioId);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AppDbContext context, IEmailService emailService, ILogger<AuthService> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<(bool success, string message, Usuario? usuario)> RegistrarAsync(string nombre, string email, string contrasena)
        {
            try
            {
                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contrasena))
                    return (false, "Todos los campos son requeridos", null);

                if (!ValidarEmail(email))
                    return (false, "Email inválido", null);

                if (contrasena.Length < 6)
                    return (false, "La contraseña debe tener al menos 6 caracteres", null);

                // Verificar si el email ya existe
                if (await _context.Usuarios.AnyAsync(u => u.Email == email))
                    return (false, "El email ya está registrado", null);

                // Crear usuario con contraseña hasheada
                var usuario = new Usuario
                {
                    Nombre = nombre,
                    Email = email,
                    ContraseniaHash = HashearContrasena(contrasena),
                    Rol = "Usuario",
                    FechaRegistro = DateTime.Now,
                    Activo = true,
                    EmailVerificado = false,
                    CodigoVerificacion = GenerarCodigoVerificacion()
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Enviar correo de verificación
                await _emailService.EnviarCodigoVerificacionAsync(email, usuario.CodigoVerificacion);
                await _emailService.EnviarBienvenidaAsync(email, nombre);

                _logger.LogInformation($"Usuario registrado: {email}");
                return (true, "Registro exitoso. Revisa tu correo para verificar la cuenta.", usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al registrar usuario: {ex.Message}");
                return (false, "Error al registrar usuario", null);
            }
        }

        public async Task<(bool success, string message, Usuario? usuario)> LoginAsync(string email, string contrasena)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contrasena))
                    return (false, "Email y contraseña requeridos", null);

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
                
                if (usuario == null)
                    return (false, "Email o contraseña incorrectos", null);

                if (!usuario.Activo)
                    return (false, "Cuenta desactivada", null);

                if (!usuario.EmailVerificado)
                    return (false, "Por favor verifica tu email antes de continuar", null);

                if (!VerificarContrasen(contrasena, usuario.ContraseniaHash))
                    return (false, "Email o contraseña incorrectos", null);

                // Actualizar último login
                await ActualizarUltimoLoginAsync(usuario.Id);

                _logger.LogInformation($"Login exitoso: {email}");
                return (true, "Login exitoso", usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al hacer login: {ex.Message}");
                return (false, "Error al hacer login", null);
            }
        }

        public async Task<bool> VerificarEmailAsync(string email, string codigo)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
                
                if (usuario == null)
                    return false;

                if (usuario.CodigoVerificacion != codigo)
                    return false;

                usuario.EmailVerificado = true;
                usuario.FechaVerificacion = DateTime.Now;
                usuario.CodigoVerificacion = null;

                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Email verificado: {email}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al verificar email: {ex.Message}");
                return false;
            }
        }

        public async Task<Usuario?> ObtenerUsuarioAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario?> ObtenerUsuarioPorEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ActualizarUltimoLoginAsync(int usuarioId)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                if (usuario == null)
                    return false;

                usuario.FechaUltimoLogin = DateTime.Now;
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Métodos auxiliares
        private bool ValidarEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private string HashearContrasena(string contrasena)
        {
            return BCrypt.Net.BCrypt.HashPassword(contrasena);
        }

        private bool VerificarContrasen(string contrasena, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(contrasena, hash);
        }

        private string GenerarCodigoVerificacion()
        {
            return new Random().Next(100000, 999999).ToString();
        }
    }
}
