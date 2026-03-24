using ParkingSystemAPI.Models;

namespace ParkingSystemAPI.Data
{
    public static class DbSeeder
    {
        public static void SeedDatabase(AppDbContext context)
        {
            // Verificar si ya existen datos
            if (context.Usuarios.Any() || context.Vehiculos.Any() || context.Espacios.Any())
            {
                return; // Los datos ya existen, no hacer nada
            }

            // Crear usuario de prueba
            var usuario = new Usuario
            {
                Nombre = "Admin",
                Email = "admin@parking.com",
                Contrasena = "admin123", // En producción, esto debe ser hasheado
                Rol = "Admin"
            };
            context.Usuarios.Add(usuario);
            context.SaveChanges();

            // Crear espacios de prueba (10 espacios)
            var espacios = new List<Espacio>();
            for (int i = 1; i <= 10; i++)
            {
                espacios.Add(new Espacio
                {
                    Numero = i,
                    Estado = "Disponible"
                });
            }
            context.Espacios.AddRange(espacios);
            context.SaveChanges();

            // Crear vehículos de prueba
            var vehiculos = new List<Vehiculo>
            {
                new Vehiculo
                {
                    Placa = "ABC123",
                    Tipo = "Auto",
                    Color = "Rojo",
                    Estado = "Fuera",
                    FechaCreacion = DateTime.Now,
                    UsuarioIdCreacion = usuario.Id
                },
                new Vehiculo
                {
                    Placa = "XYZ789",
                    Tipo = "Auto",
                    Color = "Azul",
                    Estado = "Fuera",
                    FechaCreacion = DateTime.Now,
                    UsuarioIdCreacion = usuario.Id
                },
                new Vehiculo
                {
                    Placa = "MNO456",
                    Tipo = "Camioneta",
                    Color = "Negro",
                    Estado = "Fuera",
                    FechaCreacion = DateTime.Now,
                    UsuarioIdCreacion = usuario.Id
                },
                new Vehiculo
                {
                    Placa = "PQR321",
                    Tipo = "Moto",
                    Color = "Plateado",
                    Estado = "Fuera",
                    FechaCreacion = DateTime.Now,
                    UsuarioIdCreacion = usuario.Id
                },
                new Vehiculo
                {
                    Placa = "STU654",
                    Tipo = "Auto",
                    Color = "Blanco",
                    Estado = "Fuera",
                    FechaCreacion = DateTime.Now,
                    UsuarioIdCreacion = usuario.Id
                },
                new Vehiculo
                {
                    Placa = "VWX987",
                    Tipo = "Camioneta",
                    Color = "Gris",
                    Estado = "Fuera",
                    FechaCreacion = DateTime.Now,
                    UsuarioIdCreacion = usuario.Id
                },
                new Vehiculo
                {
                    Placa = "DEF111",
                    Tipo = "Auto",
                    Color = "Verde",
                    Estado = "Fuera",
                    FechaCreacion = DateTime.Now,
                    UsuarioIdCreacion = usuario.Id
                },
                new Vehiculo
                {
                    Placa = "GHI222",
                    Tipo = "Moto",
                    Color = "Naranja",
                    Estado = "Fuera",
                    FechaCreacion = DateTime.Now,
                    UsuarioIdCreacion = usuario.Id
                }
            };

            context.Vehiculos.AddRange(vehiculos);
            context.SaveChanges();
        }
    }
}
