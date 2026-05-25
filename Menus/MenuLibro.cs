// ============================================================
//  BibliotecaUDB | Menus/MenuLibros.cs
//  Submenú del Módulo A – Gestión de Libros
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
    class MenuLibros
    {
        public static void Mostrar(Libro[] libros, ref int totalLibros)
        {
            int opcion;

            // do-while: mantener el submenú hasta que el usuario elija Volver
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n  ╔══════════════════════════════════════╗");
                Console.WriteLine("  ║        GESTIÓN DE LIBROS             ║");
                Console.WriteLine("  ╚══════════════════════════════════════╝\n");
                Console.ResetColor();
                Console.WriteLine("    1. Registrar nuevo libro");
                Console.WriteLine("    2. Buscar libro");
                Console.WriteLine("    3. Listar todos los libros");
                Console.WriteLine("    4. Eliminar libro");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("    5. Volver al Menú Principal");
                Console.ResetColor();
                Console.Write("\n  Seleccione una opción: ");

                // try-catch: capturar entrada no numérica
                try { opcion = int.Parse(Console.ReadLine()); }
                catch { opcion = 0; }

                // switch: enrutar la opción seleccionada
                switch (opcion)
                {
                    case 1:
                        GestionLibros.RegistrarLibro(libros, ref totalLibros);
                        break;

                    case 2:
                        // BuscarLibro incluye submenú: por código o por título
                        GestionLibros.BuscarLibro(libros, totalLibros);
                        break;

                    case 3:
                        GestionLibros.ListarLibros(libros, totalLibros);
                        break;

                    case 4:
                        GestionLibros.EliminarLibro(libros, ref totalLibros);
                        break;

                    case 5:
                        // Salir del do-while → regresa al menú principal
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n  [!] Opción no válida. Presione Enter para continuar...");
                        Console.ResetColor();
                        Console.ReadLine();
                        break;
                }

            } while (opcion != 5);
        }
    }
}
