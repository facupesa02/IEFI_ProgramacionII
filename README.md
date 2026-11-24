# IEFI_ProgramacionII

# 🐾 Patitas — Sistema de gestión de estadías para guardería de mascotas

Trabajo integrador (IEFI) de **Programación II**. Aplicación web para guarderías de mascotas que permite registrar mascotas, administrar los espacios de la guardería y gestionar las estadías, controlando la capacidad de cada espacio.

## Funcionalidades

- **Mascotas:** alta, listado, edición y baja (nombre, especie, raza, edad y peso).
- **Espacios:** alta, listado, edición y baja (nombre, tipo, tamaño, capacidad y estado de disponibilidad).
- **Estadías:** registro de una estadía eligiendo una mascota y un espacio, con fecha de ingreso y de salida.
- **Reglas de negocio al crear una estadía:**
  - Una mascota no puede tener más de una estadía registrada a la vez.
  - El espacio debe existir y estar disponible.
  - El espacio no puede superar su capacidad máxima en las fechas indicadas.
- **Validaciones en el frontend:** campos obligatorios, números mayores a 0 y fecha de salida no anterior a la de ingreso. Los errores se muestran debajo de cada campo, sin `alert()`.
- Confirmación antes de eliminar y diseño responsive (se ve bien en el celular).

## Tecnologías

| Capa | Tecnología |
|------|------------|
| Backend | Web API en **.NET 8** (C#) |
| Acceso a datos | **Entity Framework Core 8** (Code First + Migrations) |
| Base de datos | **SQL Server** |
| Documentación de la API | Swagger / Swashbuckle |
| Frontend | **HTML, CSS y JavaScript vanilla** (sin frameworks ni librerías) |

## Arquitectura

El backend sigue una arquitectura de **4 capas**:

```
IEFI_ProgramacionII/
├── Backend/
│   ├── Api/           # Controllers, Program.cs y configuración (appsettings)
│   ├── Business/      # Lógica de negocio (Services)
│   ├── Data/          # DbContext, Repositories y Migrations (EF Core)
│   └── Entities/      # Modelos (Models) y DTOs
├── Frontend/
│   ├── index.html
│   ├── css/styles.css
│   └── js/            # api.js, form-utils.js, app.js, mascotas.js, espacios.js, estadias.js
└── IEFI_ProgramacionII.slnx
```

Flujo de una petición: `Controller (Api)` → `Service (Business)` → `Repository (Data)` → `SQL Server`.

## Requisitos previos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server o SQL Server Express
- Herramienta de EF Core (para aplicar las migraciones):
  ```bash
  dotnet tool install --global dotnet-ef --version 8.0.5
  ```

## Instalación y ejecución

### 1. Clonar el repositorio

```bash
git clone https://github.com/facupesa02/IEFI_ProgramacionII.git
cd IEFI_ProgramacionII
```

### 2. Configurar la cadena de conexión

Editá `Backend/Api/appsettings.json` y ajustá `DefaultConnection` a tu instancia de SQL Server:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=GuarderiaPatitas;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### 3. Crear la base de datos (migraciones)

Desde la raíz del repositorio:

```bash
dotnet ef database update --project Backend/Data --startup-project Backend/Api
```

### 4. Levantar la API

```bash
dotnet run --project Backend/Api
```

La API queda disponible en `http://localhost:5151` y Swagger en `http://localhost:5151/swagger`.

### 5. Abrir el frontend

Servilo con cualquier servidor estático local (por ejemplo la extensión *Live Server* de VS Code, o `python -m http.server` dentro de la carpeta `Frontend`) y abrí `index.html` desde `http://localhost:<puerto>`.

> **Importante:** no lo abras haciendo doble clic en el archivo (`file://`), porque la API solo permite peticiones CORS desde orígenes `localhost` en modo Development.

Si cambiás el puerto de la API, actualizá la constante `API_BASE_URL` al comienzo de `Frontend/js/api.js`.

## Endpoints de la API

| Recurso | Ruta base | Operaciones |
|---------|-----------|-------------|
| Mascotas | `/api/Mascotas` | `GET`, `GET /{id}`, `POST`, `PUT /{id}`, `DELETE /{id}` |
| Espacios | `/Espacio` | `GET`, `GET /{id}`, `POST`, `PUT /{id}`, `DELETE /{id}` |
| Estadías | `/api/Estadias` | `GET`, `GET /{id}`, `POST`, `PUT /{id}`, `DELETE /{id}` |

Podés probarlos de forma interactiva desde Swagger.

## Modelo de datos

- **Mascota:** `Id`, `Nombre`, `Especie`, `Raza`, `Edad`, `Peso`
- **Espacio:** `Id`, `Nombre`, `Tipo`, `Tamano`, `Capacidad`, `Estado`
- **Estadia:** `Id`, `MascotaId`, `EspacioId`, `FechaIngreso`, `FechaSalida`

Relaciones: una estadía pertenece a una mascota y a un espacio; un espacio puede alojar varias estadías mientras no supere su capacidad.

## Autor

[@facupesa02](https://github.com/facupesa02) — Trabajo integrador de Programación II
