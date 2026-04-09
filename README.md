# 🅿️ Parking System API

## Sistema Integral de Gestión de Estacionamientos

**Desarrollado con**: ASP.NET Core MVC 10.0 + SQL Server 2022 + Entity Framework Core

---

## 📋 Descripción

Sistema web profesional para gestión completa de estacionamientos con arquitectura en 3 capas, implementación de reglas de negocio, auditoría completa y operaciones CRUD funcionales.

---

## ✨ Características

### ✅ Gestión de Vehículos
- Crear vehículos con validación de placa única  
- Consultar con búsqueda avanzada, filtros y paginación
- Editar información con auditoría visible
- Eliminar con validaciones de seguridad
- Detalles: vista completa con información
- Estadísticas de vehículos

### ✅ Control de Espacios
- Grid interactivo de estacionamientos
- Visualización de estado (Verde=Libre, Rojo=Ocupado)
- Registrar entrada/salida por click
- Modales para seleccionar vehículo
- Actividad reciente
- Estadísticas en tiempo real

### ✅ Registro de Movimientos
- Historial completo de entrada/salida
- Cálculo automático de duración
- Filtro por fechas y vehículo
- Sticky header funcional
- Badges de estado

### ✅ Autenticación
- Login/Logout funcional
- Session management
- Protección de rutas

---

## 🔐 Reglas de Negocio

| # | Regla | Descripción |
|---|-------|-------------|
| 1 | Un vehículo por espacio | Solo uno puede estar dentro |
| 2 | Una placa por instancia | No permite entrada si está "Dentro" |
| 3 | No placas duplicadas | No puede estar en múltiples espacios |
| 4 | Validación de estados | Solo "Dentro" o "Fuera" |
| 5 | Validaciones de campos | Placa 3-8, Tipo válido, Color requerido |
| 6 | Auditoría completa | Quién crea, cuándo, quién modifica |
| 7 | Seguridad en eliminación | No eliminar con registros activos |

---

## 🏗️ Arquitectura

```
PRESENTACIÓN (Vistas Razor)
        ↓
CONTROLADORES (MVC)
        ↓
SERVICIOS (Lógica de Negocio)
        ↓
DATOS (Repositorio + DbContext)
```

---

## 📦 Estructura

```
ParkingSystemAPI/
├── Controllers/      # Controladores HTTP
├── Models/          # Modelos de datos
├── Services/        # Lógica de negocio
├── Views/           # Vistas Razor
├── Data/            # Acceso a datos
└── Migrations/      # Migraciones BD
```

---

## 🚀 Ejecución

### Requisitos
- .NET SDK 10.0
- SQL Server 2022
- Visual Studio Code o Visual Studio

### Pasos

1. Restaurar dependencias
   ```bash
   dotnet restore
   ```

2. Actualizar base de datos
   ```bash
   dotnet ef database update
   ```

3. Ejecutar
   ```bash
   dotnet run
   ```

4. Acceder a `http://localhost:5198`

---

## 📊 Estadísticas

| Métrica | Valor |
|---------|-------|
| Líneas de código | 1500+ |
| Métodos en servicios | 20+ |
| Validaciones | 15+ |
| Vistas | 4+ |
| Reglas de negocio | 7 |

---

## ✅ Implementado

### Backend
- [x] Repositorio CRUD
- [x] Servicio con validaciones
- [x] Controlador MVC
- [x] Transacciones atómicas
- [x] Manejo de excepciones
- [x] Inyección de dependencias

### Frontend
- [x] Formularios validados
- [x] Interfaz responsiva
- [x] Validación HTML5 + JavaScript
- [x] Estilos profesionales

### Base de Datos
- [x] Migración completa
- [x] Auditoría
- [x] Relaciones normalizadas
- [x] Datos de prueba

---

## 🎨 Diseño

| Elemento | Color |
|----------|-------|
| Navbar | #667eea → #764ba2 |
| Disponible | #48bb78 |
| Ocupado | #f56565 |

---

## 🔄 Patrones

- Repository Pattern
- Service Layer Pattern
- Dependency Injection
- MVC Pattern
- SOLID Principles

---

## 🧪 Testing

### Crear Vehículo
1. Vehículos → Crear
2. Ingresar datos
3. Validar placa única
4. Verificar en lista

### Editar Vehículo
1. Click en "Editar"
2. Cambiar información
3. Guardar

### Eliminar Vehículo
1. Click en "Eliminar"
2. Confirmar
3. Verificar eliminación

### Registrar Entrada
1. Espacios → Espacio libre
2. Seleccionar vehículo
3. Espacio se marca ocupado

### Registrar Salida
1. Click espacio ocupado
2. Confirmar salida
3. Espacio disponible

---

## 🔍 Futuras Mejoras

- Índices en base de datos
- Tests unitarios
- Dashboard analítico
- API REST pública
- Notificaciones en tiempo real
- Integración IoT
- Reportes
- Autorización por roles

---

## 📝 Notas

- Placas: Normalizadas a MAYÚSCULAS
- Estados: "Dentro" o "Fuera"
- Eliminación: Segura con validaciones
- Auditoría: Completa en todos los cambios
- Búsqueda: Case-insensitive

---

**Arquitectura profesional lista para producción.**
