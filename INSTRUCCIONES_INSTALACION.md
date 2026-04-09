# 🅿️ Parking System API - Guía de Instalación y Ejecución

## 📋 Requisitos Previos

Antes de comenzar, asegúrate de tener instalados los siguientes programas:

### 1. **.NET SDK 10.0** (o superior)
- Descargar desde: https://dotnet.microsoft.com/download
- Verificar instalación: `dotnet --version`

### 2. **Docker Desktop**
- Descargar desde: https://www.docker.com/products/docker-desktop
- Tener habilitada la virtualización en el BIOS

### 3. **Visual Studio Code** (opcional pero recomendado)
- Descargar desde: https://code.visualstudio.com
- Extensión recomendada: "C# DevKit"

### 4. **Git** (para clonar el repositorio)
- Descargar desde: https://git-scm.com/download

---

## 🚀 Pasos de Instalación y Ejecución

### **Paso 1: Clonar o descargar el proyecto**

```bash
# Opción A: Clonar con Git
git clone <URL_DEL_REPOSITORIO>
cd ParkingSystemAPI

# Opción B: Si tienes el archivo ZIP
# Descomprime el archivo y abre la carpeta
cd ParkingSystemAPI
```

### **Paso 2: Restaurar dependencias**

```bash
dotnet restore
```

Este comando descargará todas las librerías necesarias (puede tomar 1-2 minutos).

---

### **Paso 3: Iniciar los servicios con Docker**

En una terminal (PowerShell o CMD), navega a la carpeta del proyecto y ejecuta:

```bash
docker-compose up -d
```

**Esto levantará:**
- 🗄️ SQL Server (Base de datos)
- 🌐 Adminer (Gestor BD en http://localhost:8080)
- 📧 Mailpit (Captor de correos en http://localhost:8025)

⏳ Espera 2-3 segundos a que los contenedores se inicialicen.

**Verificar que todo esté corriendo:**
```bash
docker ps
```

Deberías ver 3 contenedores: `sqlserver`, `adminer`, `mailpit`

---

### **Paso 4: Actualizar la Base de Datos**

```bash
dotnet ef database update
```

Esto ejecutará las migraciones y creará la estructura de la BD.

---

### **Paso 5: Ejecutar la aplicación**

```bash
dotnet run
```

La aplicación se iniciará en `http://localhost:5198`

---

## 🎯 Acceso a la Aplicación

### **Login de Prueba:**
- **Email:** `admin@parking.com`
- **Contraseña:** `admin123`

### **Nota importante:**
- El email del admin ya está verificado, así que puedes entrar directamente

---

## 📧 Recibir Correos

Para ver los correos de verificación al registrarte:

1. Abre: `http://localhost:8025`
2. Aquí recibirás todos los correos en tiempo real
3. Copia el código de 6 dígitos
4. Úsalo en el login para verificarte

---

## 🗄️ Gestionar la Base de Datos

Acceso a Adminer (phpAdmin para SQL Server):

1. Abre: `http://localhost:8080`
2. **Servidor:** `sqlserver`
3. **Usuario:** `sa`
4. **Contraseña:** `ParkingSystem123!`
5. **Base de datos:** `ParkingSystemDB`

---

## ⚠️ Solución de Problemas

### **Error: "Puerto 1433 ya está en uso"**
```bash
# Detener todos los contenedores
docker-compose down
# Luego volver a iniciar
docker-compose up -d
```

### **Error: "No puede conectar a SQL Server"**
```bash
# Espera 5 segundos más y reinicia la aplicación
# SQL Server tarda un poco en inicializar
```

### **Error: "dotnet no es reconocido"**
- Instala .NET SDK desde https://dotnet.microsoft.com/download
- Reinicia tu terminal

### **Mailpit no levanta**
```bash
# Reiniciar docker-compose
docker-compose down
docker-compose up -d
```

---

## 🧪 Flujo de Prueba Recomendado

### **1. Crear cuenta nueva:**
- Ve a http://localhost:5198
- Click en "Regístrate aquí"
- Ingresa email, nombre, contraseña
- Verás que se envía correo

### **2. Recibir código:**
- Abre http://localhost:8025
- Verás el correo recibido
- Copia el código de 6 dígitos

### **3. Verificar email:**
- Vuelve al login
- Ingresa email y contraseña
- Copia el código en el campo de verificación
- Click "Verificar y Continuar"

### **4. Ya dentro del sistema:**
- Ve a "Vehículos" y crea uno nuevo
- Ve a "Espacios" y registra entrada/salida
- Ve a "Registros" para ver el historial

---

## 🛑 Detener la Aplicación

### **Sigues estas teclas:**
```bash
Ctrl + C
```

### **Apagar los contenedores Docker:**
```bash
docker-compose down
```

---

## 📱 URLs Importantes

| Servicio | URL | Usuario | Contraseña |
|----------|-----|---------|-----------|
| App Principal | http://localhost:5198 | - | - |
| Mailpit (Correos) | http://localhost:8025 | - | - |
| Adminer (BD) | http://localhost:8080 | sa | ParkingSystem123! |
| SQL Server | localhost:1433 | sa | ParkingSystem123! |

---

## 💾 Configuración Producción

Si quieres usar credenciales SMTP reales (Gmail, etc), edita:
```
appsettings.json
```

Sección `EmailSettings` con tus credenciales.

---

## ✅ Checklist Final

Antes de entregar al otro usuario, verifica:

- [ ] Docker Desktop está instalado y corriendo
- [ ] .NET SDK 10.0 está instalado
- [ ] Git está instalado (opcional)
- [ ] El archivo ZIP/repositorio está disponible
- [ ] Este documento está incluido

---

## 🆘 Soporte

Si hay problemas:

1. Verifica que **Docker está ejecutándose**
2. Verifica que **los puertos no estén en uso** (1433, 8025, 8080, 5198)
3. Intenta `docker-compose down` y `docker-compose up -d`
4. Reinicia de cero si es necesario

---

**¡El sistema está listo para usar! 🎉**
