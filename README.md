

Sistema Integral de Gestión de Biblioteca Universitaria
Universidad Don Bosco — Programación de Algoritmos | Desafío Final Ciclo I – 2026



Datos del Estudiante

Nombre completo: [Genesis Scarlett Ramirez Torres]
Carné: RT262263

Nombre completo: [Wilfredo Vladimir Alfaro Perez]
Carné: AP262261

Materia: Programación de Algoritmos
Ciclo: Ciclo I – 2026
Docente: [Ing. Edwin Alfredo Bonilla]


Descripción del Proyecto

Sistema de consola desarrollado en C# que permite administrar de forma completa los recursos y operaciones de una biblioteca universitaria.

El sistema opera mediante un menú interactivo de texto y administra tres módulos principales:

Módulo A — Gestión de Libros
Módulo B — Gestión de Usuarios
Módulo C — Gestión de Préstamos

Los datos son almacenados mediante archivos CSV y el sistema realiza carga automática al iniciar y guardado automático al finalizar.


Instrucciones para Clonar y Ejecutar

Requisitos previos

.NET SDK 6.0 o superior instalado.
Git instalado.
Terminal, CMD o PowerShell.

1. Clonar el repositorio

cd DesafioFinal-RT262263-AP262261

2. Compilar el proyecto

dotnet build

3. Ejecutar el sistema

dotnet run

Al ejecutar el proyecto por primera vez, la carpeta data se crea automáticamente junto con los archivos:

libros.csv
usuarios.csv
prestamos.csv

Estructura del Proyecto

BibliotecaUDB

Modelos

Libro.cs — Struct que representa un libro.
Usuario.cs — Struct que representa un usuario.
Prestamo.cs — Struct que representa un préstamo.

Menus

MenuPrincipal.cs — Menú principal del sistema.
MenuLibro.cs — Menú de gestión de libros.
MenuUsuario.cs — Menú de gestión de usuarios.
MenuPrestamos.cs — Menú de gestión de préstamos.

Operaciones

GestionLibros.cs — Lógica del módulo de libros.
GestionUsuarios.cs — Lógica del módulo de usuarios.
GestionPrestamos.cs — Lógica del módulo de préstamos.

Validaciones

Validador.cs — Validaciones centralizadas del sistema.

Base de Datos

ArchivoLibros.cs — Persistencia CSV de libros.
ArchivoUsuarios.cs — Persistencia CSV de usuarios.
ArchivoPrestamos.cs — Persistencia CSV de préstamos.

data

libros.csv
usuarios.csv
prestamos.csv

Program.cs — Punto de entrada del sistema.
README.md

Funcionalidades del Sistema

Módulo A — Gestión de Libros

Registrar libro.
Buscar libro por código o título.
Listar libros registrados.
Eliminar libros con confirmación.
Control de inventario y ejemplares disponibles.

Módulo B — Gestión de Usuarios

Registrar usuarios.
Buscar usuarios por carné o nombre.
Listar usuarios registrados.
Cambiar estado activo o inactivo.

Módulo C — Gestión de Préstamos

Registrar préstamos.
Validar disponibilidad del libro.
Registrar devoluciones.
Actualizar inventario automáticamente.
Consultar historial de préstamos activos.
Actualizar estado del préstamo.

Validaciones Implementadas

Código de libro

Exactamente 8 caracteres alfanuméricos.
Ejemplo: LIB00001.

Carné de usuario

Exactamente 8 dígitos numéricos.
Ejemplo: 20230001.

Correo electrónico

Debe contener el carácter @ y un punto posterior.

Año de publicación

Entre 1900 y el año actual.

Cantidad de ejemplares

Número entero mayor o igual a cero.

Fechas

Formato dd/mm/yyyy validado.

Validaciones de negocio

Usuarios inactivos no pueden solicitar préstamos.
Libros sin ejemplares disponibles no pueden prestarse.
No se permiten códigos o carnés duplicados.
Capacidad máxima:

10 libros
5 usuarios
10 préstamos

Manejo de errores

Uso de try-catch para entradas inválidas.
Protección contra FormatException y OverflowException.
Manejo seguro de líneas corruptas en archivos CSV.



Persistencia de Datos

Los registros se almacenan en archivos CSV ubicados dentro de la carpeta data.

Ejemplo de libros.csv

LIB00001,Introducción a C#,Charles Petzold,Microsoft Press,2022,Tecnología e Informática,5

Ejemplo de usuarios.csv

20230001,Ana García López,Ingeniería en Sistemas,[ana@gmail.com](mailto:ana@gmail.com),7890-1234,activo

Ejemplo de prestamos.csv

20230001,LIB00001,15/05/2026,29/05/2026,activo

La información se carga automáticamente al iniciar el sistema y se guarda automáticamente al salir.


Tecnologías Utilizadas

Lenguaje de programación: C#
Framework: .NET
Tipo de aplicación: Consola
Persistencia: Archivos CSV
Control de versiones: Git y GitHub


Video Demostrativo

Enlace del video:

[PEGAR ENLACE DEL VIDEO]



Universidad Don Bosco
Programación de Algoritmos — Ciclo I 2026
