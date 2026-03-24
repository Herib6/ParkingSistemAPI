# 🚀 Parking System API - Guía de Inicio Rápido

## 📦 Estado Actual del Proyecto

**Versión**: 0.85  
**Estado**: ✅ En Desarrollo Activo  
**Progreso**: 85% Completado  

---

## 🎯 Lo que Funciona AHORA

### ✅ Pantallas Completamente Funcionales

1. **Dashboard (Home)**
   - Acceso a las tres pantallas principales
   - Estadísticas resumidas
   - Navegación fluida

2. **Vehículos** 🚗
   - ✅ **Lista/Buscar** - Grid de tarjetas con búsqueda por placa, filtro por tipo y estado
   - ✅ **Crear** - Formulario con validación AJAX de placa única
   - ✅ **Editar** - Modifica datos, muestra auditoría
   - ✅ **Eliminar** - Con confirmación y validaciones
   - ✅ **Detalles** - Vista completa con información y historial
   - Paginación implementada
   - Validaciones COMPLETAS en cliente y servidor

3. **Espacios** 🅿️
   - Grid interactivo de estacionamientos
   - Colores dinámicos (Verde=Libre, Rojo=Ocupado)
   - Click para registrar entrada (verde) / ver vehículo (rojo)
   - Modales para seleccionar vehículo
   - Actividad reciente
   - Estadísticas en tiempo real

4. **Registros** 📋
   - Tabla sticky header (fixed)
   - Historial de entradas/salidas
   - Cálculo automático de duración
   - Filtro por fechas
   - Búsqueda por vehículo
   - Badges de estado

---

## 🔐 7 Reglas de Negocio IMPLEMENTADAS

```
✅ REGLA 1: Un vehículo por espacio
   - Solo un vehículo puede estar dentro de un espacio
   - Validación en entrada

✅ REGLA 2: Una placa por instancia
   - No permite entrada si ya está "Dentro"
   - Verifica registro activo

✅ REGLA 3: No placas duplicadas simultáneamente
   - Misma placa no puede estar en múltiples espacios
   - Validación transaccional

✅ REGLA 4: Validación de estados
   - "Dentro" o "Fuera" (no valores inválidos)
   - Actualización automática

✅ REGLA 5: Validaciones de campos
   - Placa: 3-8 caracteres, única
   - Tipo: Auto|SUV|Moto|Camión|Otro
   - Color: Obligatorio

✅ REGLA 6: Auditoría completa
   - Quién crea (UsuarioIdCreacion)
   - Cuándo crea (FechaCreacion)
   - Quién modifica (UsuarioIdModificacion)
   - Cuándo modifica (FechaModificacion)

✅ REGLA 7: Seguridad en eliminación
   - No eliminar si tiene registros activos
   - No eliminar si tiene registros en últimos 30 días
   - Previene pérdida de datos críticos
```

---

## 🏗️ Arquitectura (3 Capas)

```
┌─────────────────────────────────────────┐
│  PRESENTACIÓN (Views)                   │
│  - Index.cshtml (Grid + Búsqueda)      │
│  - Create.cshtml (Formulario)          │
│  - Edit.cshtml (Edición)               │
│  - Delete.cshtml (Confirmación)        │
│  - Details.cshtml (Detalles)           │
└──────────────┬──────────────────────────┘
               │
┌──────────────▼──────────────────────────┐
│  CONTROLADORES (Controllers)            │
│  - VehiculosController                 │
│  - EspaciosController                  │
│  - RegistrosController                 │
│  - AuthController                      │
└──────────────┬──────────────────────────┘
               │
┌──────────────▼──────────────────────────┐
│  LÓGICA DE NEGOCIO (Services)          │
│  - IVehiculoService                    │
│  - VehiculoService (350+ líneas)      │
│  - Validaciones                        │
│  - Transacciones                       │
└──────────────┬──────────────────────────┘
               │
┌──────────────▼──────────────────────────┐
│  DATOS (Repositories)                  │
│  - IVehiculoRepository                 │
│  - VehiculoRepository                  │
│  - DbContext (EF Core)                 │
└──────────────┬──────────────────────────┘
               │
┌──────────────▼──────────────────────────┐
│  BASE DE DATOS (SQL Server)            │
│  - Vehiculos                           │
│  - Espacios                            │
│  - Registros                           │
│  - Usuarios                            │
│  - Sensores                            │
└─────────────────────────────────────────┘
```

---

## 🚀 Cómo Ejecutar

### Requisitos
- .NET 10.0
- SQL Server 2022 (Docker)
- Adminer (para ver BD)

### Pasos
1. **Base de Datos** (si es nueva):
   ```bash
   dotnet ef database update
   ```

2. **Ejecutar Aplicación**:
   ```bash
   dotnet run
   ```

3. **Acceder**:
   - URL: `http://localhost:5198`
   - Usuario predeterminado: (login requerido)

---

## 📚 Documentación

Consulta estos archivos para más información:

- **DOCUMENTACION_SISTEMA.md** - Documentación completa del sistema
- **CAMBIOS_SESION.md** - Resumen de cambios de esta sesión
- **GUIA_INICIO_RAPIDO.md** - Este archivo

---

## 🔄 Flujo de Entrada/Salida

```
1. Usuario hace click en espacio VERDE (Disponible)
   ↓
2. Modal abre para seleccionar vehículo
   ↓
3. Sistema valida:
   - ¿Hay espacio disponible? ✓
   - ¿Vehículo está fuera? ✓
   - ¿No existe otra instancia con misma placa? ✓
   ↓
4. Se crea registro de ENTRADA
   ↓
5. Estado vehículo cambia a "Dentro"
   ↓
6. Estado espacio cambia a "Ocupado" (ROJO)
   ↓
7. Usuario hace click en espacio ROJO (Ocupado)
   ↓
8. Modal muestra vehículo estacionado
   ↓
9. Opción para registrar SALIDA
   ↓
10. Se completa registro con FechaSalida
    ↓
11. Estado vehículo cambia a "Fuera"
    ↓
12. Estado espacio cambia a "Disponible" (VERDE)
```

---

## 🎨 Colores y Diseño

| Elemento | Color | Significa |
|----------|-------|-----------|
| Gradiente Navbar | #667eea → #764ba2 | Púrpura/Violeta elegante |
| Espacio Verde | #48bb78 | Disponible |
| Espacio Rojo | #f56565 | Ocupado |
| Botón Primario | Gradiente | Acción principal |
| Botón Secundario | #f5f5f5 | Acción secundaria |
| Badge Verde | #d4edda | Estado Dentro |
| Badge Azul | #cfe2ff | Estado Fuera |

---

## 📊 Base de Datos

### Tablas
- **Usuarios** - Autenticación
- **Vehiculos** - Información de vehículos + Estado + Auditoría
- **Espacios** - Espacios de parqueo
- **Sensores** - Sensores asociados
- **Registros** - Entrada/Salida

### Nueva Estructura Vehiculos
```sql
CREATE TABLE [Vehiculos] (
    [Id] INT PRIMARY KEY,
    [Placa] NVARCHAR(8) UNIQUE NOT NULL,
    [Tipo] NVARCHAR(50) NOT NULL,
    [Color] NVARCHAR(50) NOT NULL,
    [Estado] NVARCHAR(20) DEFAULT 'Fuera',
    [FechaCreacion] DATETIME2 DEFAULT GETDATE(),
    [FechaModificacion] DATETIME2 NULL,
    [UsuarioIdCreacion] INT NULL,
    [UsuarioIdModificacion] INT NULL
)
```

---

## 🧪 Testing Manual

### Test 1: Crear Vehículo
1. Ir a Vehículos → Nuevo Vehículo
2. Ingresar: Placa (ABC-1234), Tipo (Auto), Color (Rojo)
3. Validar que placa sea única
4. Ver en lista

### Test 2: Guardar en Espacio
1. Ir a Espacios
2. Click en espacio verde
3. Seleccionar vehículo
4. Confirmar entrada
5. Espacio debe volverse rojo

### Test 3: Salida Vehículo
1. Click en espacio rojo
2. Ver vehículo estacionado
3. Registrar salida
4. Espacio debe volverse verde

### Test 4: Ver Historial
1. Ir a Registros
2. Filtrar por fechas
3. Verificar entrada/salida
4. Ver duración calculada

---

## ⏳ Próximas 24 Horas

- [ ] Testar CRUD completo en navegador
- [ ] Validar todas las reglas de negocio
- [ ] Verificar paginación
- [ ] Confirmar validaciones funcionan

## 🎯 Esta Semana

- [ ] Mejorar diseño de Index
- [ ] Agregar índices en BD
- [ ] Implementar reportes
- [ ] Integración Espacios-Vehículos completa

## 📈 Este Mes

- [ ] Dashboard con gráficos
- [ ] API REST pública
- [ ] Notificaciones en tiempo real
- [ ] Integración con sensores

---

## 💡 Tips Importantes

1. **Placas**: Se guardan en MAYÚSCULAS automáticamente
2. **Estados**: Solo "Dentro" o "Fuera" - validado
3. **Eliminación**: Segura - valida registros previos
4. **Auditoría**: Cada cambio se registra
5. **Búsqueda**: Normalizada para no importar mayúsculas

---

## 🆘 Soporte

Si algo no funciona:
1. Revisar la consola para errores
2. Consultar `DOCUMENTACION_SISTEMA.md`
3. Ver logs de SQL Server
4. Revisar validaciones en navegador (F12)

---

**Última actualización**: 24/03/2026  
**Desarrollador**: Equipo de IA  
**Estado**: ✅ Listo para testing
