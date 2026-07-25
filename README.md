
## TaskFlow

TaskFlow es una API REST desarrollada con ASP.NET Core 8, Entity Framework Core y SQL Server para la gestión de tareas.

Este proyecto fue desarrollado como parte de una prueba técnica e implementa una arquitectura por capas, operaciones CRUD, filtros, paginacion, validaciones de negocio, manejo global de errores y un reporte generado mediante un procedimiento almacenado de SQL Server.

## Tecnologias

* ASP.NET Core 8
* Entity Framework Core 8 (Database Fisrt)
* SQL Server
* Swagger / OpenAPI

## Funcionalidades

* CRUD de tareas
* Filtros por prioridad, estado, usaurio, rango de fechas
* Paginación
* Validaciones de negocio
* Borrado lógico (Soft Delete)
* Manejo global de excepciones
* Reporte de tareas pendeintes mediante un stored procedure
* Auditoria de cambios de estado mediante un trigger de SQL Server

## Base de datos

Los scripts de creación de la base de datos, restricciones, indices, trigger, procedure y datos de prueba se encuentran en la carpeta 'Database/Scripts'.

## Configuración y ejecución

1. Ejecutar los scripts ubicados en 'Database/Scripts' respetando el orden numérico
2. Configurar la cadena de conexión en 'appsettings.json'
3. Ejecutar la aplicación
4. Acceder a la documentación de la API mediante Swagger

https://localhost:<puerto>/swagger