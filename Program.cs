// ============================================================
//  BibliotecaUDB | Program.cs
//  Punto de entrada principal del sistema.
//
//  Funciones:
//    ✔ Inicializar arreglos en memoria
//    ✔ Cargar archivos CSV automáticamente
//    ✔ Ejecutar MenuPrincipal
//    ✔ Guardar automáticamente al salir
//    ✔ Manejo global de errores
// ============================================================

using System;
using BibliotecaUDB.Modelos;
using BibliotecaUDB.BaseDeDatos;
using BibliotecaUDB.Menus;

namespace BibliotecaUDB
{
    class Program
    {
        // ────────────────────────────────────────────────────
        //  Límites del sistema (según requerimientos)
        // ────────────────────────────────────────────────────
        private const int MAX_LIBROS    = 10;
        private const int MAX_USUARIOS  = 5;
        private const int MAX_PRESTAMOS = 10;

        // ════════════════════════════════════════════════════
        //  MAIN → Punto de entrada del programa
        // ════════════════════════════════════════════════════
        static void Main(string[] args)
        {
            Console.Title = "Sistema Integral de Gestión de Biblioteca UDB";

            // ────────────────────────────────────────────────
            //  Arreglos en memoria (structs)
            // ────────────────────────────────────────────────
            Libro[] libros = new Libro[MAX_LIBROS];
            Usuario[] usuarios = new Usuario[MAX_USUARIOS];
            Prestamo[] prestamos = new Prestamo[MAX_PRESTAMOS];

            // Contadores
            int totalLibros = 0;
            int totalUsuarios = 0;
            int totalPrestamos = 0;

            try
            {
                MostrarBienvenida();

                // ════════════════════════════════════════════
                //  CARGA AUTOMÁTICA DE ARCHIVOS CSV
                // ════════════════════════════════════════════
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n  Cargando información del sistema...");
                Console.ResetColor();

                ArchivoLibros.Cargar(libros, ref totalLibros);
                ArchivoUsuarios.Cargar(usuarios, ref totalUsuarios);
                ArchivoPrestamos.Cargar(prestamos, ref totalPrestamos);

                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  [✔] Sistema inicializado correctamente.");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(
                    $"  Libros: {totalLibros}/{MAX_LIBROS} | " +
                    $"Usuarios: {totalUsuarios}/{MAX_USUARIOS} | " +
                    $"Préstamos: {totalPrestamos}/{MAX_PRESTAMOS}"
                );
                Console.ResetColor();

                Console.WriteLine();
                Pausar();

                // ════════════════════════════════════════════
                //  EJECUTAR MENÚ PRINCIPAL
                // ════════════════════════════════════════════
                MenuPrincipal.Mostrar(
                    libros, ref totalLibros,
                    usuarios, ref totalUsuarios,
                    prestamos, ref totalPrestamos
                );
            }
            catch (Exception ex)
            {
                // try-catch global
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n  [✖] Error crítico del sistema:");
                Console.WriteLine($"      {ex.Message}");
                Console.ResetColor();

                Pausar();
            }
            finally
            {
                // ════════════════════════════════════════════
                //  GUARDADO AUTOMÁTICO FINAL
                //  (seguridad extra por si hubo error)
                // ════════════════════════════════════════════
                try
                {
                    ArchivoLibros.Guardar(libros, totalLibros);
                    ArchivoUsuarios.Guardar(usuarios, totalUsuarios);
                    ArchivoPrestamos.Guardar(prestamos, totalPrestamos);
                }
                catch
                {
                    // Evitar que un error final rompa el cierre
                }
            }
        }

        // ════════════════════════════════════════════════════
        //  BIENVENIDA DEL SISTEMA
        // ════════════════════════════════════════════════════
        private static void MostrarBienvenida()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║     SISTEMA INTEGRAL DE GESTIÓN DE BIBLIOTECA        ║");
            Console.WriteLine("║              Universidad Don Bosco                  ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  Fecha del sistema: {DateTime.Now:dd/MM/yyyy}");
            Console.ResetColor();
        }

        // ════════════════════════════════════════════════════
        //  PAUSA DEL SISTEMA
        // ════════════════════════════════════════════════════
        private static void Pausar()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\n  Presione ENTER para continuar...");
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}
