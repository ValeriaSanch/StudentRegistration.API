# StudentRegistration API

API REST para el registro de estudiantes y gestión de matrículas, desarrollada como prueba técnica para **Inter Rapidísimo**.

---

## Tabla de contenido

- [Descripción general](#descripción-general)
- [Arquitectura](#arquitectura)
- [Requisitos previos](#requisitos-previos)
- [Ejecución con Docker (recomendado)](#ejecución-con-docker-recomendado)
- [Ejecución local sin Docker](#ejecución-local-sin-docker)
- [Variables de entorno](#variables-de-entorno)
- [Endpoints de la API](#endpoints-de-la-api)
- [Reglas de negocio](#reglas-de-negocio)
- [Datos de prueba (seed)](#datos-de-prueba-seed)
- [Ejemplos de uso con curl](#ejemplos-de-uso-con-curl)
- [Ejecutar las pruebas](#ejecutar-las-pruebas)
- [Estructura del proyecto](#estructura-del-proyecto)
- [Stack tecnológico](#stack-tecnológico)

---

## Descripción general

Sistema que permite registrar estudiantes, consultarlos, actualizarlos y gestionar sus matrículas en materias. Aplica reglas de negocio estrictas a nivel de dominio (máximo 3 materias, sin repetir profesor, sin duplicar materia, etc.).

---

## Arquitectura

El proyecto implementa **Arquitectura Hexagonal (Ports & Adapters)** con principios de **Domain-Driven Design (DDD)**:

```
StudentRegistration.Domain          ← Entidades, Value Objects, reglas de negocio
StudentRegistration.Application     ← Casos de uso, DTOs, comandos y queries
StudentRegistration.Infrastructure  ← EF Core, repositorios, seed de datos
StudentRegistration.Presentation    ← Controladores, validadores, middleware
StudentRegistration.API             ← Host, DI, Swagger, configuración
StudentRegistration.Tests           ← Pruebas unitarias
```

El dominio no depende de ninguna capa externa. Las reglas de negocio se aplican dentro de los agregados.

---

## Requisitos previos

| Herramienta | Versión mínima | Uso |
|---|---|---|
| [Docker Desktop](https://www.docker.com/products/docker-desktop/) | 24.x | Ejecución con contenedores |
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/8.0) | 8.0 | Ejecución local / pruebas |
| MySQL | 8.0 | Solo para ejecución local sin Docker |

> Con Docker no se necesita instalar .NET SDK ni MySQL por separado.

---

## Ejecución con Docker (recomendado)

### 1. Clonar el repositorio

```bash
git clone <url-del-repositorio>
cd StudentRegistration.API
```

### 2. (Opcional) Crear archivo `.env`

Si no se crea el archivo, Docker Compose usa los valores por defecto indicados entre paréntesis.

```env
ASPNETCORE_ENVIRONMENT=Production
API_PORT=8080
DB_PORT=3306
DB_NAME=student_registration
DB_USER=root
DB_PASSWORD=root_password
```

### 3. Levantar los contenedores

```bash
docker compose up --build -d
```

Este comando:
- Compila la imagen de la API desde el `Dockerfile`
- Levanta un contenedor MySQL 8.0
- Espera a que MySQL esté saludable antes de iniciar la API
- Crea el esquema de la base de datos automáticamente
- Carga los datos de prueba (profesores y materias)

### 4. Verificar que todo esté corriendo

```bash
docker compose ps
```

Salida esperada:

```
NAME                       STATUS          PORTS
student_registration_api   Up              0.0.0.0:8080->8080/tcp
student_registration_db    Up (healthy)    0.0.0.0:3306->3306/tcp
```

### 5. Probar la API

```bash
curl http://localhost:8080/api/v1/subjects
```

### 6. Ver logs

```bash
# Logs de la API
docker compose logs api -f

# Logs de la base de datos
docker compose logs db -f
```

### 7. Detener los contenedores

```bash
# Detener sin borrar datos
docker compose down

# Detener y borrar la base de datos
docker compose down -v
```

---

## Ejecución local sin Docker

### 1. Instalar y configurar MySQL 8.0

Crear la base de datos:

```sql
CREATE DATABASE student_registration CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 2. Configurar la cadena de conexión

Editar `StudentRegistration.API/appsettings.Development.json` con los datos de tu instancia local:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=student_registration;User=root;Password=TU_PASSWORD;AllowPublicKeyRetrieval=true;SslMode=None;CharSet=utf8mb4;"
  }
}
```

### 3. Ejecutar la API

```bash
cd StudentRegistration.API
dotnet run --project StudentRegistration.API
```

La API queda disponible en `https://localhost:7XXX` y `http://localhost:5XXX` (los puertos exactos se muestran en la consola).

### 4. Swagger UI

Con `ASPNETCORE_ENVIRONMENT=Development` (valor por defecto al ejecutar `dotnet run`), Swagger está disponible en:

```
https://localhost:{puerto}/swagger
```

---

## Variables de entorno

| Variable | Valor por defecto | Descripción |
|---|---|---|
| `ASPNETCORE_ENVIRONMENT` | `Production` | Entorno de ejecución. Usar `Development` para habilitar Swagger |
| `API_PORT` | `8080` | Puerto del host que mapea al contenedor de la API |
| `DB_PORT` | `3306` | Puerto del host que mapea al contenedor de MySQL |
| `DB_NAME` | `student_registration` | Nombre de la base de datos |
| `DB_USER` | `root` | Usuario de MySQL |
| `DB_PASSWORD` | `root_password` | Contraseña de MySQL |
| `ConnectionStrings__DefaultConnection` | _(ver docker-compose.yml)_ | Cadena de conexión completa (sobreescribe appsettings.json) |

---

## Endpoints de la API

**URL base:** `http://localhost:8080/api/v1`

Todas las respuestas de error siguen el formato **RFC 7807 Problem Details**.

---

### Estudiantes

#### `POST /students` — Registrar estudiante

**Request body:**
```json
{
  "fullName": "Juan Pérez",
  "email": "juan.perez@correo.com",
  "documentNumber": "12345678"
}
```

**Respuestas:**
| Código | Descripción |
|---|---|
| `201 Created` | Estudiante registrado exitosamente |
| `400 Bad Request` | Datos inválidos (nombre vacío, email mal formado, etc.) |
| `409 Conflict` | Ya existe un estudiante con ese email o número de documento |

---

#### `GET /students` — Listar todos los estudiantes activos

**Respuesta `200 OK`:**
```json
[
  {
    "studentId": "aca902bc-d7bb-4f65-840b-3ddf5b5f9f99",
    "fullName": "Juan Pérez",
    "email": "juan.perez@correo.com",
    "documentNumber": "12345678",
    "totalCredits": 6
  }
]
```

---

#### `GET /students/{studentId}` — Obtener estudiante por ID

**Respuestas:**
| Código | Descripción |
|---|---|
| `200 OK` | Datos del estudiante con sus matrículas activas |
| `404 Not Found` | Estudiante no encontrado |

**Respuesta `200 OK`:**
```json
{
  "studentId": "aca902bc-d7bb-4f65-840b-3ddf5b5f9f99",
  "fullName": "Juan Pérez",
  "email": "juan.perez@correo.com",
  "documentNumber": "12345678",
  "createdAt": "2026-05-01T20:00:00Z",
  "updatedAt": null,
  "isActive": true,
  "totalCredits": 6,
  "enrollments": [
    {
      "enrollmentId": "9fb9d377-e7fa-4fbe-9e49-3e139ec3782d",
      "subjectId": "20000000-0000-0000-0000-000000000001",
      "subjectName": "Matemáticas",
      "credits": 3,
      "professorName": "Prof. Ana Martínez",
      "enrolledAt": "2026-05-01T20:05:00Z",
      "isActive": true
    }
  ]
}
```

---

#### `PUT /students/{studentId}` — Actualizar estudiante

**Request body:**
```json
{
  "fullName": "Juan Carlos Pérez",
  "documentNumber": "87654321"
}
```

**Respuestas:**
| Código | Descripción |
|---|---|
| `200 OK` | Estudiante actualizado |
| `400 Bad Request` | Datos inválidos |
| `404 Not Found` | Estudiante no encontrado |
| `409 Conflict` | El número de documento ya existe |

---

#### `DELETE /students/{studentId}` — Desactivar estudiante

Realiza una **baja lógica**: marca al estudiante como inactivo y cancela todas sus matrículas activas.

**Respuestas:**
| Código | Descripción |
|---|---|
| `204 No Content` | Estudiante desactivado exitosamente |
| `404 Not Found` | Estudiante no encontrado |

---

### Matrículas

#### `POST /students/{studentId}/enrollments` — Matricular en una materia

**Request body:**
```json
{
  "subjectId": "20000000-0000-0000-0000-000000000001"
}
```

**Respuestas:**
| Código | Descripción |
|---|---|
| `201 Created` | Matrícula registrada exitosamente |
| `400 Bad Request` | SubjectId vacío o inválido |
| `404 Not Found` | Estudiante o materia no encontrados |
| `422 Unprocessable Entity` | Violación de regla de negocio (BR-01, BR-02, BR-04, BR-09) |

**Respuesta `201 Created`:**
```json
{
  "enrollmentId": "9fb9d377-e7fa-4fbe-9e49-3e139ec3782d",
  "subjectId": "20000000-0000-0000-0000-000000000001",
  "subjectName": "Matemáticas",
  "credits": 3,
  "professorName": "Prof. Ana Martínez",
  "enrolledAt": "2026-05-01T20:05:00Z",
  "isActive": true
}
```

---

#### `GET /students/{studentId}/enrollments` — Listar matrículas activas

**Respuestas:**
| Código | Descripción |
|---|---|
| `200 OK` | Lista de matrículas activas del estudiante |
| `404 Not Found` | Estudiante no encontrado |

---

#### `DELETE /students/{studentId}/enrollments/{enrollmentId}` — Cancelar matrícula

**Respuestas:**
| Código | Descripción |
|---|---|
| `204 No Content` | Matrícula cancelada exitosamente |
| `404 Not Found` | Estudiante o matrícula no encontrados |

---

### Materias

#### `GET /subjects` — Listar todas las materias

**Respuesta `200 OK`:**
```json
[
  {
    "subjectId": "20000000-0000-0000-0000-000000000001",
    "name": "Matemáticas",
    "credits": 3,
    "professorId": "10000000-0000-0000-0000-000000000001",
    "professorName": "Prof. Ana Martínez"
  }
]
```

---

### Compañeros de clase

#### `GET /students/{studentId}/subjects/{subjectId}/classmates` — Ver compañeros de una materia

Devuelve únicamente el nombre de los compañeros (privacidad de datos — BR-11).

**Respuestas:**
| Código | Descripción |
|---|---|
| `200 OK` | Lista de compañeros |
| `404 Not Found` | Estudiante no matriculado en esa materia |

**Respuesta `200 OK`:**
```json
[
  { "fullName": "María García" },
  { "fullName": "Carlos Rodríguez" }
]
```

---

## Reglas de negocio

Todas las reglas se aplican en la capa de dominio y devuelven `422 Unprocessable Entity` cuando se violan.

| Regla | Descripción | Error |
|---|---|---|
| **BR-01** | Un estudiante puede estar matriculado en un máximo de **3 materias activas** | `El estudiante ya tiene 3 materias inscritas.` |
| **BR-02** | Un estudiante **no puede tener dos materias del mismo profesor** simultáneamente | `El estudiante ya tiene una materia con este profesor.` |
| **BR-04** | Un estudiante **inactivo no puede matricularse** | `El estudiante no está activo.` |
| **BR-05** | Todas las materias tienen exactamente **3 créditos** (inmutable) | `Los créditos deben ser 3.` |
| **BR-08** | Un profesor puede dictar máximo **2 materias** | `El profesor ya tiene 2 materias asignadas.` |
| **BR-09** | Un estudiante **no puede matricularse dos veces** en la misma materia | `El estudiante ya está inscrito en esta materia.` |
| **BR-10** | Cancelar una matrícula **libera el cupo** para que el estudiante pueda inscribir otra | (comportamiento implícito) |
| **BR-11** | La consulta de compañeros devuelve **solo el nombre** por privacidad | (aplicado en el DTO) |

---

## Datos de prueba (seed)

Al iniciar la aplicación se cargan automáticamente los siguientes datos si la base de datos está vacía:

### Profesores

| ID | Nombre |
|---|---|
| `10000000-0000-0000-0000-000000000001` | Prof. Ana Martínez |
| `10000000-0000-0000-0000-000000000002` | Prof. Carlos López |
| `10000000-0000-0000-0000-000000000003` | Prof. María Torres |
| `10000000-0000-0000-0000-000000000004` | Prof. Juan Rodríguez |
| `10000000-0000-0000-0000-000000000005` | Prof. Sofía Ramírez |

### Materias

| ID | Materia | Profesor | Créditos |
|---|---|---|---|
| `20000000-0000-0000-0000-000000000001` | Matemáticas | Prof. Ana Martínez | 3 |
| `20000000-0000-0000-0000-000000000002` | Física | Prof. Ana Martínez | 3 |
| `20000000-0000-0000-0000-000000000003` | Historia | Prof. Carlos López | 3 |
| `20000000-0000-0000-0000-000000000004` | Geografía | Prof. Carlos López | 3 |
| `20000000-0000-0000-0000-000000000005` | Química | Prof. María Torres | 3 |
| `20000000-0000-0000-0000-000000000006` | Biología | Prof. María Torres | 3 |
| `20000000-0000-0000-0000-000000000007` | Programación | Prof. Juan Rodríguez | 3 |
| `20000000-0000-0000-0000-000000000008` | Bases de Datos | Prof. Juan Rodríguez | 3 |
| `20000000-0000-0000-0000-000000000009` | Inglés | Prof. Sofía Ramírez | 3 |
| `20000000-0000-0000-0000-000000000010` | Comunicación | Prof. Sofía Ramírez | 3 |

---

## Ejemplos de uso con curl

### Registrar un estudiante

```bash
curl -X POST http://localhost:8080/api/v1/students \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Juan Pérez",
    "email": "juan.perez@correo.com",
    "documentNumber": "12345678"
  }'
```

### Matricular en Matemáticas

```bash
curl -X POST http://localhost:8080/api/v1/students/{studentId}/enrollments \
  -H "Content-Type: application/json" \
  -d '{
    "subjectId": "20000000-0000-0000-0000-000000000001"
  }'
```

### Consultar matrículas del estudiante

```bash
curl http://localhost:8080/api/v1/students/{studentId}/enrollments
```

### Cancelar una matrícula

```bash
curl -X DELETE http://localhost:8080/api/v1/students/{studentId}/enrollments/{enrollmentId}
```

### Ver compañeros en Matemáticas

```bash
curl http://localhost:8080/api/v1/students/{studentId}/subjects/20000000-0000-0000-0000-000000000001/classmates
```

### Ejemplo de error de dominio (BR-01)

```bash
# Respuesta cuando el estudiante ya tiene 3 materias activas:
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Error de dominio",
  "status": 422,
  "detail": "El estudiante ya tiene 3 materias inscritas.",
  "traceId": "00-abc123..."
}
```

---

## Ejecutar las pruebas

```bash
# Desde la raíz del repositorio
dotnet test

# Con reporte de cobertura
dotnet test --collect:"XPlat Code Coverage"

# Solo el proyecto de pruebas
dotnet test StudentRegistration.Tests
```

### Cobertura de pruebas

| Suite | Descripción |
|---|---|
| `Domain/StudentTests` | Reglas de negocio del agregado Student (BR-01, BR-02, BR-04, BR-09, BR-10) |
| `Domain/EmailValueObjectTests` | Validación del Value Object Email |
| `Application/EnrollStudentUseCaseTests` | Caso de uso de matrícula con mocks de repositorios |
| `Presentation/RegisterStudentCommandValidatorTests` | Validación FluentValidation del comando de registro |

---

## Estructura del proyecto

```
StudentRegistration.API/
├── StudentRegistration.Domain/
│   ├── Entities/
│   │   ├── Student.cs              ← Aggregate Root
│   │   ├── Enrollment.cs
│   │   ├── Subject.cs
│   │   └── Professor.cs
│   ├── ValueObjects/
│   │   ├── Email.cs
│   │   ├── StudentId.cs
│   │   ├── SubjectId.cs
│   │   ├── ProfessorId.cs
│   │   └── EnrollmentId.cs
│   ├── Exceptions/                 ← Excepciones de dominio (BR-01..BR-11)
│   └── Interfaces/                 ← Puertos (IStudentRepository, IUnitOfWork, etc.)
│
├── StudentRegistration.Application/
│   ├── UseCases/                   ← Casos de uso (RegisterStudent, EnrollStudent, etc.)
│   ├── Commands/                   ← Comandos de escritura
│   ├── Queries/                    ← Queries de lectura
│   ├── DTOs/                       ← Objetos de transferencia de datos
│   ├── Mappings/                   ← Perfiles AutoMapper
│   └── Abstractions/               ← IUseCase<TCommand, TResult>
│
├── StudentRegistration.Infrastructure/
│   ├── Persistence/
│   │   ├── ApplicationDbContext.cs
│   │   ├── Configurations/         ← EF Core Fluent API (por entidad)
│   │   └── Seed/DataSeeder.cs      ← Datos iniciales
│   └── Repositories/               ← Implementaciones de los repositorios
│
├── StudentRegistration.Presentation/
│   ├── Controllers/                ← StudentsController, EnrollmentsController, etc.
│   ├── Validators/                 ← Validadores FluentValidation
│   └── Middleware/
│       └── ExceptionHandlingMiddleware.cs
│
├── StudentRegistration.API/
│   ├── Program.cs                  ← Composición raíz (DI + pipeline)
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── Dockerfile
│
├── StudentRegistration.Tests/
│   ├── Domain/
│   ├── Application/
│   └── Presentation/
│
├── docker-compose.yml
├── Directory.Build.props           ← StyleCop compartido entre proyectos
└── stylecop.json                   ← Configuración de análisis estático
```

---

## Stack tecnológico

| Componente | Tecnología | Versión |
|---|---|---|
| Framework | ASP.NET Core | 8.0 |
| ORM | Entity Framework Core + Pomelo MySQL | 8.0.10 / 8.0.2 |
| Base de datos | MySQL | 8.0 |
| Mapper | AutoMapper | 12.0.1 |
| Validación | FluentValidation | 11.3.0 |
| Logging | Serilog (consola + archivo) | 8.0.3 |
| Documentación | Swashbuckle / Swagger | 6.9.0 |
| Testing | xUnit + Moq + FluentAssertions | 2.9.2 / 4.20.72 / 6.12.1 |
| Análisis estático | StyleCop.Analyzers | 1.2.0-beta.556 |
| Contenedores | Docker + Docker Compose | v2 |
