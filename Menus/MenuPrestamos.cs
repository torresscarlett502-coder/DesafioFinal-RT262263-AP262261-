// ============================================================
//  BibliotecaUDB | Menus/MenuPrestamos.cs
//  Submenú del Módulo C – Gestión de Préstamos
//
//  Estructuras de control:
//    ✔ do-while → mantener el submenú activo hasta Volver
//    ✔ switch   → enrutar cada opción a su operación
//    ✔ try-catch → captura de opción no numérica
// ============================================================

using System;
using BibliotecaUDB.Modelos;
using BibliotecaUDB.Operaciones;

namespace BibliotecaUDB.Menus
{
    class MenuPrestamos
    {
        public static void Mostrar(
            Prestamo[] prestamos, ref int totalPrestamos,
            Libro[]    libros,        int totalLibros,
            Usuario[]  usuarios,      int totalUsuarios)
        {
            int opcion;

            // do-while: mantener el submenú hasta que el usuario elija Volver
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n  ╔══════════════════════════════════════╗");
                Console.WriteLine("  ║        GESTIÓN DE PRÉSTAMOS          ║");
                Console.WriteLine("  ╚══════════════════════════════════════╝\n");
                Console.ResetColor();
                Console.WriteLine("    1. Registrar préstamo");
                Console.WriteLine("    2. Registrar devolución");
                Console.WriteLine("    3. Historial activo de un usuario");
                Console.WriteLine("    4. Actualizar estado de préstamo");
                Console.WriteLine("    5. Listar todos los préstamos");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("    6. Volver al Menú Principal");
                Console.ResetColor();
                Console.Write("\n  Seleccione una opción: ");

                // try-catch: capturar entrada no numérica
                try { opcion = int.Parse(Console.ReadLine()); }
                catch { opcion = 0; }

                // switch: enrutar la opción seleccionada
                switch (opcion)
                {
                    case 1:
                        // Firma: (prestamos, ref totalPrestamos, usuarios, totalUsuarios, libros, totalLibros)
                        GestionPrestamos.RegistrarPrestamo(
                            prestamos, ref totalPrestamos,
                            usuarios,  totalUsuarios,
                            libros,    totalLibros);
                        break;

                    case 2:
                        // Firma: (prestamos, totalPrestamos, libros, totalLibros, usuarios, totalUsuarios)
                        GestionPrestamos.RegistrarDevolucion(
                            prestamos, totalPrestamos,
                            libros,    totalLibros,
                            usuarios,  totalUsuarios);
                        break;

                    case 3:
                        // Firma: (prestamos, totalPrestamos, libros, totalLibros, usuarios, totalUsuarios)
                        GestionPrestamos.HistorialActivoUsuario(
                            prestamos, totalPrestamos,
                            libros,    totalLibros,
                            usuarios,  totalUsuarios);
                        break;

                    case 4:
                        // Firma: (prestamos, totalPrestamos, libros, totalLibros, usuarios, totalUsuarios)
                        GestionPrestamos.ActualizarEstadoPrestamo(
                            prestamos, totalPrestamos,
                            libros,    totalLibros,
                            usuarios,  totalUsuarios);
                        break;

                    case 5:
                        // Firma: (prestamos, totalPrestamos, libros, totalLibros, usuarios, totalUsuarios)
                        GestionPrestamos.ListarPrestamos(
                            prestamos, totalPrestamos,
                            libros,    totalLibros,
                            usuarios,  totalUsuarios);
                        break;

                    case 6:
                        // Salir del do-while → regresa al menú principal
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n  [!] Opción no válida. Presione Enter para continuar...");
                        Console.ResetColor();
                        Console.ReadLine();
                        break;
                }

            } while (opcion != 6);
        }
    }
}
