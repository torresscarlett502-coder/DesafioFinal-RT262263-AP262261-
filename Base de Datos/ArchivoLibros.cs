/// ============================================================
//  BibliotecaUDB | BaseDeDatos/ArchivoLibros.cs
//  Persistencia – Módulo A (Libros)
//
//  CORRECCIÓN respecto a la versión original:
//    ✔ Métodos renombrados a Cargar() y Guardar() para que
//      coincidan con las llamadas desde MenuPrincipal.cs:
//        ArchivoLibros.Cargar(libros, ref totalLibros)
//        ArchivoLibros.Guardar(libros, totalLibros)
//    ✔ Validación de cada línea CSV delegada a Validador
//      (ValidarCamposCSVLibro) en lugar de lógica inline.
//
//  Estructuras de control:
//    ✔ if-else    → existencia de archivo/carpeta, límite, campos
//    ✔ for        → recorrer el arreglo al guardar
//    ✔ while      → leer línea a línea hasta EOF
//    ✔ try-catch  → IOException, FormatException, OverflowException
//
//  Formato CSV (7 campos, sin cabecera):
//    Codigo,Titulo,Autor,Editorial,AnioPublicacion,Categoria,EjemplaresDisponibles
// ============================================================

using System;
using System.IO;
using BibliotecaUDB.Modelos;
using BibliotecaUDB.Validaciones;

namespace BibliotecaUDB.BaseDeDatos
{
    static class ArchivoLibros
    {
        // ────────────────────────────────────────────────────
        //  Constantes del módulo
        // ────────────────────────────────────────────────────
        private const int MAX_LIBROS = 10;
        private const int CAMPOS_CSV = 7;

        private const string CARPETA  = "data";
        private static readonly string ARCHIVO = Path.Combine(CARPETA, "libros.csv");

        // ════════════════════════════════════════════════════
        //  CARGAR  →  archivo CSV  ──►  arreglo en memoria
        // ════════════════════════════════════════════════════
        /// <summary>
        /// Lee libros.csv y llena el arreglo libros[].
        /// Si el archivo no existe, arranca con inventario vacío.
        /// Valida cada línea con Validador.ValidarCamposCSVLibro().
        /// </summary>
        public static void Cargar(Libro[] libros, ref int totalLibros)
        {
            totalLibros = 0;

            // if-else: primer arranque — archivo aún no existe
            if (!File.Exists(ARCHIVO))
            {
                Informacion($"Archivo \"{ARCHIVO}\" no encontrado. Se iniciará con inventario vacío.");
                return;
            }

            try
            {
                StreamReader sr        = new StreamReader(ARCHIVO);
                string       linea;
                int          nLinea    = 0;
                int          ignoradas = 0;

                // while: leer línea a línea hasta fin de archivo (EOF)
                while ((linea = sr.ReadLine()) != null)
                {
                    nLinea++;

                    // if-else: ignorar líneas completamente vacías
                    if (string.IsNullOrWhiteSpace(linea))
                        continue;

                    // if-else: arreglo lleno → dejar de leer
                    if (totalLibros >= MAX_LIBROS)
                    {
                        Advertencia(
                            $"Límite de {MAX_LIBROS} libros alcanzado. " +
                            $"Las líneas restantes del archivo serán ignoradas."
                        );
                        break;
                    }

                    string[] campos = linea.Split(',');

                    // ── VALIDACIÓN CENTRALIZADA ───────────────────────
                    // Delegar toda la lógica de validación a Validador.cs
                    ResultadoValidacion rv = Validador.ValidarCamposCSVLibro(campos);

                    // if-else: línea inválida → advertir y continuar
                    if (!rv.Valido)
                    {
                        Advertencia($"Línea {nLinea} ignorada — {rv.Mensaje}");
                        ignoradas++;
                        continue;
                    }
                    // ── FIN VALIDACIÓN ────────────────────────────────

                    // Parseo seguro (ya validado arriba)
                    try
                    {
                        Libro libro = new Libro();
                        libro.Codigo                = campos[0].Trim();
                        libro.Titulo                = campos[1].Trim();
                        libro.Autor                 = campos[2].Trim();
                        libro.Editorial             = campos[3].Trim();
                        libro.AnioPublicacion       = int.Parse(campos[4].Trim());
                        libro.Categoria             = campos[5].Trim();
                        libro.EjemplaresDisponibles = int.Parse(campos[6].Trim());

                        libros[totalLibros] = libro;
                        totalLibros++;
                    }
                    catch (FormatException)
                    {
                        // if-else: caso residual — validador no detectó el error
                        Advertencia($"Línea {nLinea}: error de formato inesperado. Línea ignorada.");
                        ignoradas++;
                    }
                    catch (OverflowException)
                    {
                        Advertencia($"Línea {nLinea}: valor numérico fuera de rango. Línea ignorada.");
                        ignoradas++;
                    }
                }
                // ── fin del while ─────────────────────────────────────

                sr.Close();

                // if-else: informe final diferenciado según resultado
                if (totalLibros > 0)
                    Exito($"Libros cargados: {totalLibros} registro(s) desde \"{ARCHIVO}\"." +
                          (ignoradas > 0 ? $" ({ignoradas} línea(s) ignorada(s))" : ""));
                else
                    Informacion($"\"{ARCHIVO}\" existe pero no contiene registros válidos.");
            }
            catch (IOException ex)
            {
                Error($"Error de acceso al archivo \"{ARCHIVO}\": {ex.Message}");
                Error("El sistema continuará sin datos de libros.");
            }
            catch (Exception ex)
            {
                Error($"Error inesperado al cargar libros: {ex.Message}");
            }
        }

        // ════════════════════════════════════════════════════
        //  GUARDAR  →  arreglo en memoria  ──►  archivo CSV
        // ════════════════════════════════════════════════════
        /// <summary>
        /// Sobreescribe libros.csv con el contenido actual del arreglo.
        /// Crea la carpeta data/ si aún no existe.
        /// </summary>
        public static void Guardar(Libro[] libros, int totalLibros)
        {
            try
            {
                // if-else: crear carpeta data/ si no existe
                if (!Directory.Exists(CARPETA))
                {
                    Directory.CreateDirectory(CARPETA);
                    Informacion($"Carpeta \"{CARPETA}\" creada automáticamente.");
                }

                StreamWriter sw = new StreamWriter(ARCHIVO, append: false);

                // for: un ciclo = un struct = una línea CSV
                for (int i = 0; i < totalLibros; i++)
                {
                    // El orden de campos DEBE coincidir con Cargar()
                    sw.WriteLine(
                        libros[i].Codigo                + "," +
                        libros[i].Titulo                + "," +
                        libros[i].Autor                 + "," +
                        libros[i].Editorial             + "," +
                        libros[i].AnioPublicacion       + "," +
                        libros[i].Categoria             + "," +
                        libros[i].EjemplaresDisponibles
                    );
                }
                // ── fin del for ──────────────────────────────────────

                sw.Close();

                // if-else: mensaje diferenciado si no hay registros
                if (totalLibros > 0)
                    Exito($"Libros guardados: {totalLibros} registro(s) en \"{ARCHIVO}\".");
                else
                    Informacion($"No hay libros que guardar. \"{ARCHIVO}\" quedará vacío.");
            }
            catch (IOException ex)
            {
                Error($"Error de escritura en \"{ARCHIVO}\": {ex.Message}");
                Error("Los datos NO fueron guardados. Verifique permisos de escritura.");
            }
            catch (Exception ex)
            {
                Error($"Error inesperado al guardar libros: {ex.Message}");
            }
        }

        // ════════════════════════════════════════════════════
        //  MENSAJES DE CONSOLA (estilo unificado del sistema)
        // ════════════════════════════════════════════════════
        private static void Exito(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  [✔] {msg}");
            Console.ResetColor();
        }

        private static void Advertencia(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  [!] {msg}");
            Console.ResetColor();
        }

        private static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  [✖] {msg}");
            Console.ResetColor();
        }

        private static void Informacion(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  [i] {msg}");
            Console.ResetColor();
        }
    }
}
