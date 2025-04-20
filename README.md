Ejercicio 1 - IntegraComex

# 📋 Proyecto CRUD de Clientes - Ejercicio 1  - IntegraComex

## Introducción

Aplicación web desarrollada en ASP.NET MVC con Entity Framework que permite realizar operaciones básicas de **alta, baja, modificación y visualización de clientes** (CRUD).

## Cómo ejecutar

1. Clonar el proyecto.
2. Abrir con Visual Studio.
3. Seleccionar el archivo sln.
4. Ejecutar con el perfil **IIS Express**

## ¿Qué tiene?

- Aplicación  con arquitectura en capas (Controladores, Servicios, Modelos, Contexto).
- Validaciones del lado del servidor.
- Logs informativos y de errores utilizando Serilog.
- Manejo de errores amigables para el usuario en la vista.
- Control de duplicados por Cuit al crear un nuevo cliente.

## Base de datos

-Implemente la base de datos con **SQL Server LocalDB**, que viene con Visual Studio. No hay que instalar nada ni ejecutar scripts: al ejecutar el proyecto con IIS Express, **la base de datos se crea automáticamente** con Entity Framework.

## Cómo verla?

1. En Visual Studio, abrís el **Explorador de SQL Server** (`Ver > Explorador de objetos de SQL Server`).
2. Buscás `(localdb)\MSSQLLocalDB` y ahí vas a ver la base con las tablas generadas.

##  ¿Qué me costó?

- Implementar Store Procedures con la base de datos que estaba manejando.
- Implementar otro tipo de conexion a la base de datos.
- Integrar Serilog fue nuevo para mí, pero no me resulto muy complicado, fue el framework para manejo de logs mas sencillo 

###  Logging 

El sistema de logging está implementado usando la librería **Serilog**, para registrar tanto informacion como errores.

-  Los logs se almacenan en un archivo `.txt` en la ruta:  
  `{/Ejercicio1-CRUD_Clientes/App_Data}`  


###  La configuración se realiza desde el archivo `Global.asax.cs` y se encarga de registrar:
  - **Información**: por ejemplo, cuando se crea un cliente exitosamente.
  - **Advertencias**: si el modelo no es válido.
  - **Errores**: excepciones inesperadas durante la ejecución.
