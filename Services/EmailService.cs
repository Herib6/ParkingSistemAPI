using System.Net;
using System.Net.Mail;

namespace ParkingSystemAPI.Services
{
    public interface IEmailService
    {
        Task<bool> EnviarCorreoAsync(string destinatario, string asunto, string mensaje);
        Task<bool> EnviarCodigoVerificacionAsync(string email, string codigo);
        Task<bool> EnviarBienvenidaAsync(string email, string nombre);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> EnviarCorreoAsync(string destinatario, string asunto, string mensaje)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "localhost";
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "1025");
                var emailOrigen = _configuration["EmailSettings:EmailOrigen"] ?? "parkingsystem@localhost";
                var contrasena = _configuration["EmailSettings:Contrasena"] ?? "";
                var useMailpit = bool.Parse(_configuration["EmailSettings:UseMailpit"] ?? "true");

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = false; // Mailpit no usa SSL localmente
                    
                    if (!useMailpit && !string.IsNullOrEmpty(contrasena))
                    {
                        client.Credentials = new NetworkCredential(emailOrigen, contrasena);
                    }

                    var mailMessage = new MailMessage(emailOrigen, destinatario, asunto, mensaje);
                    mailMessage.IsBodyHtml = true;

                    await client.SendMailAsync(mailMessage);
                    _logger.LogInformation($"Correo enviado a {destinatario}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al enviar correo: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EnviarCodigoVerificacionAsync(string email, string codigo)
        {
            var mensaje = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <div style='background-color: #f5f5f5; padding: 20px; border-radius: 10px;'>
                        <h2 style='color: #667eea;'>Verificar Email - Parking System</h2>
                        <p>Tu código de verificación es:</p>
                        <h1 style='color: #667eea; background-color: #f0f0f0; padding: 15px; border-radius: 5px; text-align: center;'>{codigo}</h1>
                        <p><strong>Este código expira en 24 horas.</strong></p>
                        <hr style='border: none; border-top: 1px solid #ddd; margin: 20px 0;'>
                        <p style='font-size: 12px; color: #999;'>Este es un mensaje automático, por favor no responder.</p>
                    </div>
                </body>
                </html>";

            return await EnviarCorreoAsync(email, "Código de Verificación - Parking System", mensaje);
        }

        public async Task<bool> EnviarBienvenidaAsync(string email, string nombre)
        {
            var mensaje = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <div style='background-color: #f5f5f5; padding: 20px; border-radius: 10px;'>
                        <h2 style='color: #667eea;'>Bienvenido a Parking System, {nombre}!</h2>
                        <p>Tu cuenta ha sido registrada exitosamente.</p>
                        <div style='background-color: white; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                            <p><strong>Email:</strong> {email}</p>
                        </div>
                        <p>Ahora puedes acceder al sistema con tus credenciales.</p>
                        <p><a href='http://localhost:5198' style='background-color: #667eea; color: white; padding: 10px 20px; border-radius: 5px; text-decoration: none; display: inline-block;'>Ir al Sistema</a></p>
                        <hr style='border: none; border-top: 1px solid #ddd; margin: 20px 0;'>
                        <p style='font-size: 12px; color: #999;'>Este es un mensaje automático, por favor no responder.</p>
                    </div>
                </body>
                </html>";

            return await EnviarCorreoAsync(email, "Bienvenido a Parking System", mensaje);
        }
    }
}
