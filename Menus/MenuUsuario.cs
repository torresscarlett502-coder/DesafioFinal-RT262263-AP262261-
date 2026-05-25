// ============================================================
//  BibliotecaUDB | Menus/MenuUsuarios.cs
//  Submenú del Módulo B – Gestión de Usuarios
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
    class MenuUsuarios
    {
        public static void Mostrar(Usuario[] usuarios, ref int totalUsuarios)
        {
            int opcion;

            // do-while: mantener el submenú hasta que el usuario elija Volver
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n  ╔══════════════════════════════════════╗");
                Console.WriteLine("  ║        GESTIÓN DE USUARIOS           ║");
                Console.WriteLine("  ╚══════════════════════════════════════╝\n");
                Console.ResetColor();
                Console.WriteLine("    1. Registrar nuevo usuario");
                Console.WriteLine("    2. Buscar usuario");
                Console.WriteLine("    3. Listar todos los usuarios");
                Console.WriteLine("    4. Cambiar estado de usuario");
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
                        GestionUsuarios.RegistrarUsuario(usuarios, ref totalUsuarios);
                        break;

                    case 2:
                        // BuscarUsuario incluye submenú: por carné o por nombre
                        GestionUsuarios.BuscarUsuario(usuarios, totalUsuarios);
                        break;

                    case 3:
                        GestionUsuarios.ListarUsuarios(usuarios, totalUsuarios);
                        break;

                    case 4:
                        GestionUsuarios.CambiarEstadoUsuario(usuarios, totalUsuarios);
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
