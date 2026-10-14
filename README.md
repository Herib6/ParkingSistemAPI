# Parking System API
## Sistema Integral de Gestión de Estacionamientos

Desarrollado con: ASP.NET Core MVC 10.0 + SQL Server 2022 + Entity Framework Core

---

## Descripción
Sistema web para la gestión completa de estacionamientos, con arquitectura en 3 capas, reglas de negocio implementadas, auditoría de cambios y operaciones CRUD funcionales.

## Objetivo
Controlar y registrar en tiempo real la entrada y salida de vehículos, la ocupación de los espacios y el historial de movimientos de un estacionamiento, aplicando reglas de negocio que garanticen la integridad de los datos.

## Problema que resuelve
La gestión manual de un estacionamiento (cuadernos u hojas de cálculo) genera registros duplicados, errores en las placas, falta de control sobre qué espacios están ocupados y ausencia de trazabilidad de quién hizo cada cambio. Este sistema centraliza la información, valida las operaciones y deja un historial auditable.

---

## Funcionalidades principales

### Gestión de vehículos
- Crear vehículos con validación de placa única.
- Consultar con búsqueda avanzada, filtros y paginación.
- Editar información con auditoría visible.
- Eliminar con validaciones de seguridad.
- Vista de detalles y estadísticas de vehículos.

### Control de espacios
- Grid interactivo de estacionamientos.
- Visualización de estado (verde = libre, rojo = ocupado).
- Registro de entrada y salida por clic.
- Modales para seleccionar vehículo.
- Actividad reciente y estadísticas en tiempo real.

### Registro de movimientos
- Historial completo de entrada y salida.
- Cálculo automático de duración.
- Filtro por fechas y vehículo.
- Encabezado fijo (sticky header) y etiquetas de estado.

### Autenticación
- Inicio y cierre de sesión.
- Manejo de sesiones.
- Protección de rutas.

---

## Reglas de negocio

| # | Regla | Descripción |
|---|-------|-------------|
| 1 | Un vehículo por espacio | Solo uno puede estar dentro |
| 2 | Una placa por instancia | No permite entrada si ya está "Dentro" |
| 3 | No placas duplicadas | Un vehículo no puede estar en múltiples espacios |
| 4 | Validación de estados | Solo "Dentro" o "Fuera" |
| 5 | Validaciones de campos | Placa de 3 a 8 caracteres, tipo válido, color requerido |
| 6 | Auditoría completa | Quién crea, cuándo, quién modifica |
| 7 | Seguridad en eliminación | No se elimina con registros activos |

---

## Arquitectura seleccionada
Se seleccionó una **arquitectura en capas (3 capas) con patrón MVC**, por su separación de responsabilidades, facilidad de mantenimiento y buen soporte en ASP.NET Core.

**Patrones aplicados:** Repository, Service Layer, Inyección de Dependencias, MVC y principios SOLID.

PRESENTACIÓN (Vistas Razor)
↓
CONTROLADORES (MVC)
↓
SERVICIOS (Lógica de negocio)
↓
DATOS (Repositorio + DbContext)
↓
SQL Server 2022

---

## Organización Frontend / Backend

### Frontend (capa de presentación)
**Responsabilidad:** interfaz con la que interactúa el usuario. Incluye las vistas Razor (`Views/`), formularios con validación HTML5 y JavaScript, el grid de espacios, los modales de selección de vehículo y los estilos responsivos. No contiene lógica de negocio ni accede directamente a la base de datos.

### Backend (controladores, servicios y datos)
- **Controllers:** reciben las peticiones HTTP, validan el modelo y delegan al servicio correspondiente.
- **Services:** contienen la lógica de negocio y las 7 reglas del sistema, y manejan transacciones atómicas y excepciones.
- **Data (Repositorio + DbContext):** acceso a SQL Server mediante Entity Framework Core y migraciones.
- **Models:** entidades y modelos de vista.

### Comunicación entre frontend y backend
1. El usuario realiza una acción en la vista (por ejemplo, clic en un espacio libre y selección de un vehículo).
2. El navegador envía una petición HTTP (GET/POST) al controlador, con protección de sesión.
3. El controlador invoca al servicio, que valida las reglas de negocio.
4. El servicio usa el repositorio para leer o escribir en la base de datos.
5. El resultado regresa al controlador, que devuelve la vista Razor actualizada (o un mensaje de error de validación).

---

## Herramientas utilizadas

| Herramienta | Uso |
|-------------|-----|
| ASP.NET Core MVC 10.0 | Framework web (backend y vistas) |
| Entity Framework Core | ORM y migraciones |
| SQL Server 2022 | Base de datos |
| Visual Studio Code / Visual Studio | Desarrollo |
| Visual Paradigm Online | Modelos UML |
| GitHub, Issues y GitHub Projects | Control de versiones y gestión del avance |

---

## Estructura del proyecto

ParkingSystemAPI/
├── Controllers/ # Controladores HTTP
├── Models/ # Modelos de datos
├── Services/ # Lógica de negocio
├── Views/ # Vistas Razor
├── Data/ # Acceso a datos
├── Migrations/ # Migraciones de BD
└── docs/ # Requisitos, UML, arquitectura y nota de versión
---

## Ejecución

### Requisitos
- .NET SDK 10.0
- SQL Server 2022
- Visual Studio Code o Visual Studio

### Pasos
1. Restaurar dependencias: `dotnet restore`
2. Actualizar la base de datos: `dotnet ef database update`
3. Ejecutar: `dotnet run`
4. Acceder a `http://localhost:5198`

---

## Estado actual del proyecto
**Versión actual: v1.0**

| Área | Estado |
|------|--------|
| Backend (repositorio CRUD, servicios, controladores, transacciones, excepciones, DI) | Implementado |
| Frontend (formularios validados, interfaz responsiva, estilos) | Implementado |
| Base de datos (migración, auditoría, relaciones, datos de prueba) | Implementado |
| Pruebas manuales de los flujos principales | Realizadas |
| Pruebas unitarias | Pendiente |
| Autorización por roles | Pendiente |

## Pruebas manuales realizadas
- **Crear vehículo:** Vehículos > Crear, ingresar datos, validar placa única, verificar en la lista.
- **Editar vehículo:** clic en "Editar", cambiar información, guardar.
- **Eliminar vehículo:** clic en "Eliminar", confirmar, verificar eliminación.
- **Registrar entrada:** Espacios > espacio libre > seleccionar vehículo > el espacio se marca ocupado.
- **Registrar salida:** clic en espacio ocupado > confirmar > el espacio queda disponible.

## Documentación técnica
- Requisitos y modelos UML: carpeta `/docs`
- Nota técnica de versión: `/docs/nota-version.md`
- Seguimiento: pestañas Issues y Projects del repositorio

---

## Mejoras futuras
- Índices en la base de datos
- Pruebas unitarias
- Dashboard analítico
- API REST pública
- Notificaciones en tiempo real
- Integración IoT (sensores)
- Reportes
- Autorización por roles

## Notas
- Placas: normalizadas a MAYÚSCULAS.
- Estados: "Dentro" o "Fuera".
- Eliminación: segura con validaciones.
- Auditoría: completa en todos los cambios.
- Búsqueda: no distingue mayúsculas de minúsculas.
