# Parking System API - Documentación

## 📋 Descripción General

**Parking System API** es una aplicación web desarrollada en **ASP.NET Core 10.0** que implementa un sistema de gestión de estacionamiento. La aplicación incluye un módulo de autenticación con login de usuarios, dashboard interactivo y gestión de espacios, vehículos y registros de entrada/salida.

**Nota Importante:** Este proyecto contiene modelos preparados para integración futura con MQTT para sensores de espacios, pero actualmente solo ejecuta el módulo de login sin conexión a base de datos.

---

## 🛠️ Extensiones y Dependencias Utilizadas

### Dependencias NuGet (Paquetes Instalados)

| Paquete | Versión | Propósito |
|---------|---------|----------|
| `Microsoft.AspNetCore.OpenApi` | 10.0.0 | Soporte para OpenAPI/Swagger |
| `Microsoft.EntityFrameworkCore` | 10.0.5 | ORM para mapeo de base de datos |
| `Microsoft.EntityFrameworkCore.SqlServer` | 10.0.5 | Proveedor SQL Server para EF Core |
| `Microsoft.EntityFrameworkCore.Tools` | 10.0.5 | Herramientas para migraciones de BD |
| `BCrypt.Net-Next` | 4.1.0 | Hashing seguro de contraseñas |
| `MQTTnet` | 5.1.0.1559 | Protocolo MQTT para sensores (preparado, no activo) |

### Framework y Tecnologías

- **Framework:** ASP.NET Core 10.0
- **Lenguaje:** C# 12.0
- **Patrón:** MVC (Model-View-Controller)
- **Sesiones:** Sesiones HTTP para mantener estado de usuario
- **Base de Datos:** SQL Server (configurado pero no requerido actualmente)
- **Frontend:** HTML5, CSS3, Razor Views

---

## 📦 Requisitos Previos

### Sistema Operativo
- **Windows 10/11** (probado en Windows 11)
- **Linux/macOS** (compatible pero no probado)

### Software Requerido

1. **.NET 10.0 SDK**
   - Descarga: https://dotnet.microsoft.com/download/dotnet/10.0
   - Verificación: `dotnet --version`

2. **Git** 
   - Descarga: https://git-scm.com/

3. **Visual Studio Code** (recomendado)
   - Descarga: https://code.visualstudio.com/
   - Extensiones recomendadas:
     - **C# Dev Kit** (Microsoft) - Soporte completo para C#
     - **REST Client** (Huachao Mao) - Pruebas de API
     - **Thunder Client** - Cliente HTTP en VS Code

4. **SQL Server** 
   - Para desarrollo: SQL Server Express 2022
   

---

## 🚀 Instalación y Configuración

### Paso 1: Clonar o Descargar el Proyecto

```bash
# Opción 1: Si tienes Git
git clone [URL-del-repositorio]
cd ParkingSystemAPI

# Opción 2: Si descargaste como ZIP
cd c:\ruta\hacia\ParkingSystemAPI
```

### Paso 2: Restaurar Dependencias

```bash
cd c:\Users\user\OneDrive\Escritorio\ProyectoFinalProgramación3\ParkingSystemAPI
dotnet restore
```

Este comando descarga e instala todos los paquetes NuGet especificados en `ParkingSystemAPI.csproj`.

### Paso 3: Compilar el Proyecto

```bash
dotnet build
```

**Esperado:** Compilación exitosa con máximo 3 advertencias (sobre propiedades nullable de Vehiculo).

### Paso 4: Ejecutar la Aplicación

```bash
dotnet run
```

**Salida esperada:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started.
```

### Paso 5: Acceder a la Aplicación

Abre tu navegador y ve a:
```
http://localhost:5000
```

---

## 🔐 Credenciales de Prueba

**Email:** `admin@parking.com`  
**Contraseña:** `admin123`

---

## 📋 Comandos Útiles

### Compilación y Ejecución

```bash
# Compilar el proyecto
dotnet build

# Ejecutar la aplicación
dotnet run

# Compilar modo Release (optimizado)
dotnet build -c Release

# Ejecutar modo Release
dotnet run -c Release
```

### Gestión de Migraciones (Entity Framework)

```bash
# Crear nueva migración
dotnet ef migrations add [NombreMigracion]

# Ejemplo: Agregar tabla de usuarios
dotnet ef migrations add AddUsuariosTable

# Aplicar migraciones a la base de datos
dotnet ef database update

# Ver migraciones aplicadas
dotnet ef migrations list

# Deshacer última migración
dotnet ef migrations remove
```

### Limpieza y Mantenimiento

```bash
# Limpiar archivos compilados
dotnet clean

# Eliminar dependencias cacheadas
dotnet clean
dotnet restore

# Eliminar carpeta bin y obj (limpieza profunda)
rm -Recurse bin, obj -Force  # PowerShell
rm -rf bin obj               # Bash
```

### Instalación de Nuevos Paquetes

```bash
# Agregar paquete NuGet
dotnet add package [NombrePaquete]

# Ejemplo: Agregar Serilog para logging
dotnet add package Serilog.AspNetCore

# Remover paquete
dotnet remove package [NombrePaquete]
```

---

## 📁 Estructura del Proyecto

```
ParkingSystemAPI/
├── Controllers/
│   ├── AuthController.cs          # Controlador de autenticación (Login/Logout)
│   └── HomeController.cs          # Controlador de dashboard
├── Views/
│   ├── Auth/
│   │   ├── Login.cshtml           # Vista de página de login
│   │   └── Register.cshtml        # Vista de registro (preparada, no activa)
│   └── Home/
│       └── Index.cshtml           # Vista de dashboard
├── Models/
│   ├── Usuario.cs                 # Modelo de usuario
│   ├── Vehiculo.cs                # Modelo de vehículo
│   ├── Espacio.cs                 # Modelo de espacio de estacionamiento
│   ├── Sensor.cs                  # Modelo de sensor MQTT
│   └── Registro.cs                # Modelo de registro entrada/salida
├── Data/
│   ├── AppDbContext.cs            # Contexto de Entity Framework
│   └── Migrations/                # Migraciones de base de datos
├── Properties/
│   └── launchSettings.json        # Configuración de ejecución
├── appsettings.json               # Configuración general
├── appsettings.Development.json   # Configuración de desarrollo
├── Program.cs                     # Punto de entrada de la aplicación
└── ParkingSystemAPI.csproj        # Archivo de proyecto
```

---

## 🔄 Cambios Realizados Durante el Desarrollo

### 1. **Inicialización del Proyecto**
- ✅ Creación de estructura MVC básica
- ✅ Configuración de sesiones HTTP
- ✅ Instalación de dependencias principales

### 2. **Implementación de Autenticación (Sin BD)**
- ✅ Controlador `AuthController` con validación de credenciales en memoria
- ✅ Login de prueba: `admin@parking.com` / `admin123`
- ✅ Sesiones HTTP para mantener estado de usuario autenticado
- ✅ Botón de logout para limpiar sesión
- ⏸️ Registro de usuarios: Desactivado (pendiente de implementación con BD)

### 3. **Vistas y Frontend**

#### Vista de Login (`Views/Auth/Login.cshtml`)
- Diseño moderno con gradiente purple-pink
- Campos: Email y Contraseña
- Mensajes de error personalizados
- Responsiva para dispositivos móviles
- CSS incrustado (sin dependencias externas)

#### Vista de Dashboard (`Views/Home/Index.cshtml`)
- Barra de navegación con nombre de usuario actual
- **3 tarjetas de funcionalidad:**
  1. 🚗 **Vehículos** - Gestión de vehículos registrados
  2. 🅿️ **Espacios** - Consulta disponibilidad de espacios
  3. 📊 **Registros** - Historial de entrada/salida
- ~~📡 **Sensores** - Removido a solicitud del usuario~~
- Botón de logout en esquina superior derecha
- Diseño responsiva y profesional

### 4. **Modelos de Datos**
```csharp
// Usuario.cs - Estructura para autenticación
public class Usuario
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Email { get; set; }
    public string? Contrasena { get; set; }
    public string? Rol { get; set; }
}

// Vehiculo.cs - Registro de vehículos
public class Vehiculo
{
    public int Id { get; set; }
    public string? Placa { get; set; }
    public string? Tipo { get; set; }
    public string? Color { get; set; }
}

// Espacio.cs - Espacios de estacionamiento
public class Espacio
{
    public int Id { get; set; }
    public int Numero { get; set; }
    public string? Estado { get; set; }
    public int SensorId { get; set; }
}

// Sensor.cs - Sensores MQTT
public class Sensor
{
    public int Id { get; set; }
    public string? Ubicacion { get; set; }
    public string? Estado { get; set; }
    public string? TopicMqtt { get; set; }  // Para MQTT
    public DateTime UltimaActualizacion { get; set; }
}

// Registro.cs - Entrada/Salida de vehículos
public class Registro
{
    public int Id { get; set; }
    public int VehiculoId { get; set; }
    public int EspacioId { get; set; }
    public DateTime FechaEntrada { get; set; }
    public DateTime? FechaSalida { get; set; }
}
```

### 5. **Configuración**

#### Program.cs
```csharp
// Cambios principales:
- ✅ AddControllersWithViews() - Soporte MVC
- ✅ AddSession() - Sesiones de usuario
- ✅ Middleware de enrutamiento
- ❌ Removida integración con SQL Server (a solicitud)
- ❌ Removida configuración MQTT (no activa)
```

#### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ParkingSystemDB;..."
  },
  "Mqtt": {
    "BrokerAddress": "mqtt://localhost:1883",
    "ClientId": "ParkingSystem",
    "Topics": {
      "Sensores": "parking/sensores/#"
    }
  }
}
```

### 6. **Decisiones y Removals**

| Cambio | Razón | Estado |
|--------|-------|--------|
| Remover SQL Server | Simplificar setup inicial | ✅ Completado |
| Removido servicio MQTT | Esperar configuración | ⏸️ Preparado para futuro |
| Login sin BD | Pruebas rápidas | ✅ Funcionando |
| Botón Sensores removido | A solicitud del usuario | ✅ Completado |
| Credenciales hardcodeadas | Desarrollo/Demo | ✅ Vigente |

---

## 🧪 Pruebas Manuales

### Caso 1: Login Exitoso
```
1. Ir a http://localhost:5000
2. Ingresar email: admin@parking.com
3. Ingresar contraseña: admin123
4. Click en "Iniciar Sesión"
✅ Esperado: Redirección al dashboard
```

### Caso 2: Login Fallido
```
1. Ir a http://localhost:5000
2. Ingresar credenciales incorrectas
3. Click en "Iniciar Sesión"
✅ Esperado: Mensaje de error "Email o contraseña incorrectos"
```

### Caso 3: Logout
```
1. En el dashboard, click en "Cerrar Sesión"
✅ Esperado: Redirección a pantalla de login, sesión limpiada
```

### Caso 4: Acceso sin Autenticación
```
1. Ir a http://localhost:5000/home/index sin autenticar
✅ Esperado: Redirección automática a login
```

---

## 🔮 Próximas Implementaciones Planeadas

### Corto Plazo
- [ ] Conectar con SQL Server para persistencia de datos
- [ ] Implementar registro de nuevos usuarios
- [ ] CRUD completo para vehículos
- [ ] CRUD para espacios de estacionamiento

### Mediano Plazo
- [ ] Integración con sensores MQTT
- [ ] Dashboard con estadísticas en tiempo real
- [ ] API REST para consumo móvil
- [ ] Autenticación con JWT

### Largo Plazo
- [ ] App móvil iOS/Android
- [ ] Panel administrativo avanzado
- [ ] Reportes y analíticos
- [ ] Sistema de pagos

---

## 📝 Notas de Desarrollo

### Debugging
```bash
# Ejecutar con logs detallados
dotnet run --verbose

# Ejecutar con debugging
dotnet run --environment Development
```

### Problemas Comunes

**❌ Error: "dotnet no reconocido"**
- Solución: Instalar .NET 10.0 SDK desde https://dotnet.microsoft.com

**❌ Error: "Puerto 5000 en uso"**
```bash
# Hallar proceso en puerto 5000
netstat -ano | findstr :5000

# Matar proceso
taskkill /PID [PID] /F

# O ejecutar en puerto diferente
dotnet run --urls http://localhost:5001
```

**❌ Error de migraciones: "Modelo cambia cada que se compila"**
- Solución: No usar `DateTime.Now` en HasData, usar valores hardcodeados


## 📄 Licencia

Este proyecto es de uso educativo/interno.

**Última actualización:** 13 de Marzo de 2026  
**Versión:** 1.0.0  
**Estado:** En desarrollo - Módulo de Login funcional ✅
