# School API

REST API para gestión de estudiantes, profesores y calificaciones. Desarrollado con ASP.NET Core 9.0 y Entity Framework Core con SQLite.

## 📋 Requisitos Previos

- **.NET SDK 9.0** o superior
  - Descarga desde: https://dotnet.microsoft.com/download
  - Verifica la instalación: `dotnet --version`

- **Git** (para clonar el repositorio)
  - Descarga desde: https://git-scm.com/

## 🚀 Instalación

### 1. Clonar el Repositorio

```bash
git clone https://github.com/blaagroupsas/prueba_audisoft_backend.git
cd prueba_audisoft_backend
```

### 2. Restaurar Dependencias

```bash
dotnet restore
```

### 3. Ejecutar Migraciones de Base de Datos

Las migraciones se aplican automáticamente al iniciar la aplicación, lo que crea la base de datos SQLite si no existe.

```bash
dotnet ef database update
```

### 4. Ejecutar la Aplicación

```bash
dotnet run
```

La API estará disponible en:
- **URL Base**: `https://localhost:7125` o `http://localhost:5025`
- **Swagger UI**: `https://localhost:7125/swagger/index.html`

## 📚 API Endpoints

La API expone los siguientes endpoints:

### Estudiantes (Students)
- `GET /api/students` - Obtener todos los estudiantes
- `GET /api/students/{id}` - Obtener un estudiante por ID
- `POST /api/students` - Crear un nuevo estudiante
- `PUT /api/students/{id}` - Actualizar un estudiante
- `DELETE /api/students/{id}` - Eliminar un estudiante

### Profesores (Teachers)
- `GET /api/teachers` - Obtener todos los profesores
- `GET /api/teachers/{id}` - Obtener un profesor por ID
- `POST /api/teachers` - Crear un nuevo profesor
- `PUT /api/teachers/{id}` - Actualizar un profesor
- `DELETE /api/teachers/{id}` - Eliminar un profesor

### Calificaciones (Grades)
- `GET /api/grades` - Obtener todas las calificaciones
- `GET /api/grades/{id}` - Obtener una calificación por ID
- `POST /api/grades` - Crear una nueva calificación
- `PUT /api/grades/{id}` - Actualizar una calificación
- `DELETE /api/grades/{id}` - Eliminar una calificación

## 🔧 Configuración

### Cadena de Conexión

La base de datos se configura en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=schoolapi.db"
  }
}
```

Para cambiar la ubicación o nombre de la base de datos, modifica esta ruta en `appsettings.json`.

### CORS

La API está configurada para permitir solicitudes desde:
- `http://localhost:4200` (por defecto para aplicaciones Angular)

Para modificar los orígenes permitidos, edita la configuración en `Program.cs`:

```csharp
options.AddPolicy(AngularCorsPolicy, policy =>
{
    policy.WithOrigins("http://localhost:4200")
          .AllowAnyHeader()
          .AllowAnyMethod();
});
```

## 🛠️ Desarrollo

### Estructura del Proyecto

```
SchoolApi/
├── Controllers/        # Controladores API (StudentController, TeacherController, GradeController)
├── Models/            # Modelos de dominio (Student, Teacher, Grade)
├── Dtos/              # Data Transfer Objects
├── Data/              # DbContext y configuración de base de datos
├── Migrations/        # Migraciones de Entity Framework
├── Properties/        # Configuración del proyecto
├── Program.cs         # Configuración de la aplicación
├── appsettings.json   # Configuración por entorno
└── SchoolApi.csproj   # Archivo de proyecto
```

### Agregar una Nueva Migración

```bash
dotnet ef migrations add NombreMigracion
dotnet ef database update
```

### Ejecutar en Modo Desarrollo

```bash
dotnet run --configuration Development
```

## 🧪 Testing

Para ejecutar pruebas (si existen):

```bash
dotnet test
```

## 🐛 Troubleshooting

### Error: "Unable to connect to database"
- Verifica que `schoolapi.db` tenga permisos de lectura/escritura
- Asegúrate de estar en el directorio correcto

### Error: "Port already in use"
- La aplicación por defecto usa los puertos 7125 (HTTPS) y 5025 (HTTP)
- Verifica que estos puertos estén disponibles o cambia la configuración en `launchSettings.json`

### Error: "dotnet command not found"
- Verifica la instalación de .NET SDK: `dotnet --version`
- Reinicia la terminal después de instalar el SDK

## 📝 Licencia

Este proyecto es parte de Audisoft Group.

## ✉️ Soporte

Para reportar problemas o sugerencias, contacta al equipo de desarrollo.
