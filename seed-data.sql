-- Insertar Usuarios
INSERT INTO [ParkingSystemDB].[dbo].[Usuarios] (Nombre, Email, Contrasena, Rol) VALUES
('Admin User', 'admin@parking.com', 'admin123', 'Admin'),
('Juan Pérez', 'juan@parking.com', 'password123', 'Usuario'),
('María García', 'maria@parking.com', 'password123', 'Usuario'),
('Carlos López', 'carlos@parking.com', 'password123', 'Usuario');

-- Insertar Sensores
INSERT INTO [ParkingSystemDB].[dbo].[Sensores] (Ubicacion, Estado, TopicMqtt, UltimaActualizacion) VALUES
('Planta 1 - Sector A', 'Activo', 'parking/sensores/p1_a', GETDATE()),
('Planta 1 - Sector B', 'Activo', 'parking/sensores/p1_b', GETDATE()),
('Planta 2 - Sector A', 'Activo', 'parking/sensores/p2_a', GETDATE()),
('Planta 2 - Sector B', 'Activo', 'parking/sensores/p2_b', GETDATE());

-- Insertar Espacios
INSERT INTO [ParkingSystemDB].[dbo].[Espacios] (Numero, Estado, SensorId) VALUES
(101, 'Disponible', 1),
(102, 'Ocupado', 1),
(103, 'Disponible', 1),
(104, 'Disponible', 1),
(105, 'Ocupado', 2),
(106, 'Disponible', 2),
(201, 'Disponible', 3),
(202, 'Ocupado', 3),
(203, 'Disponible', 3),
(204, 'Ocupado', 4);

-- Insertar Vehículos
INSERT INTO [ParkingSystemDB].[dbo].[Vehiculos] (Placa, Tipo, Color) VALUES
('ABC-1234', 'Auto', 'Rojo'),
('XYZ-5678', 'Auto', 'Azul'),
('MNP-9012', 'SUV', 'Gris'),
('QRS-3456', 'Sedan', 'Negro'),
('TUV-7890', 'Auto', 'Blanco');

-- Insertar Registros (Entradas y Salidas)
INSERT INTO [ParkingSystemDB].[dbo].[Registros] (VehiculoId, EspacioId, FechaEntrada, FechaSalida) VALUES
(1, 1, '2026-03-24 08:00:00', '2026-03-24 12:30:00'),
(2, 5, '2026-03-24 09:15:00', NULL),
(3, 2, '2026-03-24 10:00:00', '2026-03-24 14:00:00'),
(4, 10, '2026-03-24 11:30:00', NULL),
(5, 1, '2026-03-24 07:00:00', '2026-03-24 15:00:00');

SELECT 'Datos de prueba insertados correctamente' AS Resultado;
