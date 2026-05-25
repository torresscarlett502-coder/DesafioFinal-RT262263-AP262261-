// ============================================================
//  BibliotecaUDB | Menus/MenuPrincipal.cs
//  Menú raíz del sistema. Punto de entrada de la navegación.
//
//  Estructuras de control:
//    ✔ do-while → mantener el sistema activo hasta Salir
//    ✔ switch   → enrutar a cada submódulo
//    ✔ try-catch → captura de opción no numérica
// ============================================================

using System;
using BibliotecaUDB.Modelos;
using BibliotecaUDB.Operaciones;
using BibliotecaUDB.BaseDeDatos;   // ← ArchivoLibros, ArchivoUsuarios, ArchivoPrestamos

namespace BibliotecaUDB.Menus
{
    class MenuPrincipal
    {
        public static void Mostrar(
            Libro[]    libros,    ref int totalLibros,
            Usuario[]  usuarios,  ref int totalUsuarios,
            Prestamo[] prestamos, ref int totalPrestamos)
        {
            int opcion;

            // do-while: mantener el sistema activo hasta que el usuario elija Salir
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n  ╔══════════════════════════════════════════════════════╗");
                Console.WriteLine("  ║    SISTEMA INTEGRAL DE GESTIÓN DE BIBLIOTECA         ║");
                Console.WriteLine("  ║               Universidad Don Bosco                  ║");
                Console.WriteLine("  ╚══════════════════════════════════════════════════════╝\n");
                Console.ResetColor();

                // Mostrar capacidad actual del sistema
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  Libros: {totalLibros}/10   " +
                                  $"Usuarios: {totalUsuarios}/5   " +
                                  $"Préstamos: {totalPrestamos}/10\n");
                Console.ResetColor();

                Console.WriteLine("    1. Gestión de Libros");
                Console.WriteLine("    2. Gestión de Usuarios");
                Console.WriteLine("    3. Gestión de Préstamos");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("    4. Salir del Sistema");
                Console.ResetColor();
                Console.Write("\n  Seleccione una opción: ");

                // try-catch: capturar entrada no numérica sin romper el programa
                try { opcion = int.Parse(Console.ReadLine()); }
                catch { opcion = 0; }

                // switch: enrutar al submenú correspondiente
                switch (opcion)
                {
                    case 1:
                        MenuLibros.Mostrar(libros, ref totalLibros);
                        break;

                    case 2:
                        MenuUsuarios.Mostrar(usuarios, ref totalUsuarios);
                        break;

                    case 3:
                        MenuPrestamos.Mostrar(
                            prestamos, ref totalPrestamos,
                            libros,    totalLibros,
                            usuarios,  totalUsuarios);
                        break;

                    case 4:
                        // Guardar los 3 arreglos en sus respectivos archivos CSV
                        Console.WriteLine();
                        ArchivoLibros.Guardar(libros, totalLibros);
                        ArchivoUsuarios.Guardar(usuarios, totalUsuarios);
                        ArchivoPrestamos.Guardar(prestamos, totalPrestamos);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n  [✔] Datos guardados correctamente.");
                        Console.WriteLine("  Hasta pronto. ¡Que tenga un buen día!");
                        Console.ResetColor();
                        Console.WriteLine();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n  [!] Opción no válida. Presione Enter para continuar...");
                        Console.ResetColor();
                        Console.ReadLine();
                        break;
                }

            } while (opcion != 4);
        }
    }
}
