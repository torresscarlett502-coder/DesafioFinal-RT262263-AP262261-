// ============================================================
//  BibliotecaUDB | Operaciones/GestionLibros.cs
//  Módulo A – Gestión de Libros
//
//  Estructuras de control presentes:
//    ✔ if-else    → validaciones, búsquedas, condiciones
//    ✔ switch     → clasificación de stock, ficha de libro
//    ✔ for        → recorrer arreglos, listar, buscar
//    ✔ while      → reintentar entradas inválidas
//    ✔ do-while   → confirmar acciones, captura obligatoria
//    ✔ try-catch  → parseo de enteros desde consola
// ============================================================

using System;
using BibliotecaUDB.Modelos;

namespace BibliotecaUDB.Operaciones
{
    static class GestionLibros
    {
        // ────────────────────────────────────────────────────
        //  Constantes del módulo
        // ────────────────────────────────────────────────────
        private const int MAX_LIBROS  = 10;
        private const int ANIO_MIN    = 1900;
        private const int STOCK_BAJO  = 3;   // umbral para alerta de stock

        // ════════════════════════════════════════════════════
        //  REGISTRAR LIBRO
        // ════════════════════════════════════════════════════
        public static void RegistrarLibro(Libro[] libros, ref int totalLibros)
        {
            Console.Clear();
            Encabezado("REGISTRAR NUEVO LIBRO");

            // if-else: verificar capacidad del arreglo
            if (totalLibros >= MAX_LIBROS)
            {
                Error($"Límite alcanzado. El sistema admite un máximo de {MAX_LIBROS} libros.");
                Pausar();
                return;
            }

            Libro libro = new Libro();

            // ── Código ───────────────────────────────────────
            libro.Codigo = LeerCodigo(libros, totalLibros);

            // ── Título ───────────────────────────────────────
            libro.Titulo = LeerTexto("Título del libro");

            // ── Autor ────────────────────────────────────────
            libro.Autor = LeerTexto("Autor");

            // ── Editorial ────────────────────────────────────
            libro.Editorial = LeerTexto("Editorial");

            // ── Año de publicación ───────────────────────────
            libro.AnioPublicacion = LeerAnio();

            // ── Categoría ────────────────────────────────────
            libro.Categoria = LeerCategoria();

            // ── Ejemplares disponibles ───────────────────────
            libro.EjemplaresDisponibles = LeerEnteroPositivo("Cantidad de ejemplares disponibles");

            libros[totalLibros] = libro;
            totalLibros++;

            Console.WriteLine();
            Separador();
            Exito($"Libro \"{libro.Titulo}\" registrado con código [{libro.Codigo}].");

            // switch: mostrar estado de stock al registrar
            string estadoStock = ObtenerEstadoStock(libro.EjemplaresDisponibles);
            Console.ForegroundColor = ObtenerColorStock(libro.EjemplaresDisponibles);
            Console.WriteLine($"  Estado de inventario : {estadoStock}");
            Console.ResetColor();

            Console.WriteLine($"  Libros en sistema    : {totalLibros}/{MAX_LIBROS}");
            Pausar();
        }

        // ════════════════════════════════════════════════════
        //  BUSCAR LIBRO (por Código o por Título)
        // ════════════════════════════════════════════════════
        public static void BuscarLibro(Libro[] libros, int totalLibros)
        {
            Console.Clear();
            Encabezado("BUSCAR LIBRO");

            // if-else: verificar que existan libros
            if (totalLibros == 0)
            {
                Advertencia("No hay libros registrados en el sistema.");
                Pausar();
                return;
            }

            // switch-case: elegir criterio de búsqueda
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  Criterio de búsqueda:");
            Console.ResetColor();
            Console.WriteLine("    1. Por código");
            Console.WriteLine("    2. Por título (búsqueda parcial)");
            Console.Write("\n  Seleccione una opción: ");

            string opcion = Console.ReadLine().Trim();
            Console.WriteLine();

            switch (opcion)
            {
                case "1":
                    BuscarPorCodigo(libros, totalLibros);
                    break;

                case "2":
                    BuscarPorTitulo(libros, totalLibros);
                    break;

                default:
                    Advertencia("Opción no válida. Regresando al menú de libros.");
                    break;
            }

            Pausar();
        }

        // ────────────────────────────────────────────────────
        //  Búsqueda por código exacto
        // ────────────────────────────────────────────────────
        private static void BuscarPorCodigo(Libro[] libros, int totalLibros)
        {
            Console.Write("  Código a buscar (8 caracteres): ");
            string codigo = Console.ReadLine().Trim().ToUpper();

            // if-else: validar que no esté vacío
            if (string.IsNullOrWhiteSpace(codigo))
            {
                Advertencia("Debe ingresar un código para realizar la búsqueda.");
                return;
            }

            int pos = -1;

            // for: recorrer libros comparando código
            for (int i = 0; i < totalLibros; i++)
            {
                if (libros[i].Codigo == codigo)
                {
                    pos = i;
                    break;
                }
            }

            Separador();

            // if-else: mostrar resultado o mensaje de error
            if (pos != -1)
            {
                Exito($"Libro encontrado con el código \"{codigo}\":");
                Console.WriteLine();
                MostrarFichaLibro(libros[pos], pos + 1);
            }
            else
            {
                Error($"No se encontró ningún libro con el código \"{codigo}\".");
            }
        }

        // ────────────────────────────────────────────────────
        //  Búsqueda por título parcial
        // ────────────────────────────────────────────────────
        private static void BuscarPorTitulo(Libro[] libros, int totalLibros)
        {
            Console.Write("  Palabras del título a buscar: ");
            string fragmento = Console.ReadLine().Trim().ToLower();

            // if-else: validar entrada
            if (string.IsNullOrWhiteSpace(fragmento))
            {
                Advertencia("Debe ingresar al menos una palabra para buscar.");
                return;
            }

            int encontrados = 0;

            // for: recorrer todos los libros buscando coincidencias
            for (int i = 0; i < totalLibros; i++)
            {
                if (libros[i].Titulo.ToLower().Contains(fragmento))
                {
                    // if-else: primer resultado → mostrar encabezado
                    if (encontrados == 0)
                    {
                        Separador();
                        Exito($"Resultados para \"{fragmento}\":");
                        Console.WriteLine();
                    }

                    MostrarFichaLibro(libros[i], i + 1);
                    Console.WriteLine();
                    encontrados++;
                }
            }

            // if-else: verificar si se encontró algo
            if (encontrados == 0)
            {
                Separador();
                Error($"Ningún libro contiene \"{fragmento}\" en el título.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  Total de coincidencias: {encontrados}");
                Console.ResetColor();
            }
        }

        // ════════════════════════════════════════════════════
        //  LISTAR TODOS LOS LIBROS
        // ════════════════════════════════════════════════════
        public static void ListarLibros(Libro[] libros, int totalLibros)
        {
            Console.Clear();
            Encabezado("LISTADO COMPLETO DE LIBROS");

            // if-else: verificar que haya libros
            if (totalLibros == 0)
            {
                Advertencia("No hay libros registrados en el sistema.");
                Pausar();
                return;
            }

            // Contadores para el resumen estadístico
            int sinStock        = 0;
            int stockBajo       = 0;
            int disponibles     = 0;
            int totalEjemplares = 0;

            // for: primer recorrido — calcular estadísticas
            for (int i = 0; i < totalLibros; i++)
            {
                int ej = libros[i].EjemplaresDisponibles;
                totalEjemplares += ej;

                // switch: clasificar cada libro por nivel de stock
                switch (ObtenerNivelStock(ej))
                {
                    case 0: sinStock++;    break;
                    case 1: stockBajo++;   break;
                    default: disponibles++; break;
                }
            }

            // Resumen estadístico
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(
                $"\n  Registrados : {totalLibros}/{MAX_LIBROS}   " +
                $"Ejemplares totales: {totalEjemplares}   " +
                $"Sin stock: {sinStock}   Stock bajo: {stockBajo}   OK: {disponibles}"
            );
            Console.ResetColor();
            Console.WriteLine();

            // Encabezado de tabla
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(
                $"  {"N°",-4}" +
                $"{"CÓDIGO",-11}" +
                $"{"TÍTULO",-30}" +
                $"{"AUTOR",-24}" +
                $"{"EDITORIAL",-16}" +
                $"{"AÑO",-6}" +
                $"{"CATEGORÍA",-18}" +
                $"{"EJEMPL.",-9}" +
                $"{"ESTADO",-15}"
            );
            Separador();
            Console.ResetColor();

            // for: segundo recorrido — imprimir cada libro en tabla
            for (int i = 0; i < totalLibros; i++)
            {
                int nivel = ObtenerNivelStock(libros[i].EjemplaresDisponibles);

                Console.ForegroundColor = ObtenerColorStock(libros[i].EjemplaresDisponibles);

                Console.WriteLine(
                    $"  {(i + 1),-4}" +
                    $"{libros[i].Codigo,-11}" +
                    $"{Cortar(libros[i].Titulo,     29),-30}" +
                    $"{Cortar(libros[i].Autor,      23),-24}" +
                    $"{Cortar(libros[i].Editorial,  15),-16}" +
                    $"{libros[i].AnioPublicacion,-6}" +
                    $"{Cortar(libros[i].Categoria,  17),-18}" +
                    $"{libros[i].EjemplaresDisponibles,-9}" +
                    $"{ObtenerEstadoStock(libros[i].EjemplaresDisponibles),-15}"
                );

                Console.ResetColor();
            }

            Separador();

            // Leyenda de colores
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;  Console.Write("  ■ Disponible  ");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("■ Stock bajo  ");
            Console.ForegroundColor = ConsoleColor.Red;    Console.Write("■ Sin stock");
            Console.ResetColor();
            Console.WriteLine("\n");

            Pausar();
        }

        // ════════════════════════════════════════════════════
        //  ELIMINAR LIBRO
        // ════════════════════════════════════════════════════
        public static void EliminarLibro(Libro[] libros, ref int totalLibros)
        {
            Console.Clear();
            Encabezado("ELIMINAR LIBRO");

            // if-else: verificar que haya libros
            if (totalLibros == 0)
            {
                Advertencia("No hay libros registrados en el sistema.");
                Pausar();
                return;
            }

            // Mostrar lista compacta previa
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  {"N°",-4}{"CÓDIGO",-11}{"TÍTULO",-32}{"EJEMPLARES",-12}{"ESTADO",-15}");
            Separador();
            Console.ResetColor();

            // for: mostrar todos los libros disponibles para eliminar
            for (int i = 0; i < totalLibros; i++)
            {
                Console.ForegroundColor = ObtenerColorStock(libros[i].EjemplaresDisponibles);

                Console.WriteLine(
                    $"  {(i + 1),-4}" +
                    $"{libros[i].Codigo,-11}" +
                    $"{Cortar(libros[i].Titulo, 31),-32}" +
                    $"{libros[i].EjemplaresDisponibles,-12}" +
                    $"{ObtenerEstadoStock(libros[i].EjemplaresDisponibles),-15}"
                );

                Console.ResetColor();
            }

            Separador();

            Console.Write("\n  Ingrese el código del libro a eliminar: ");
            string codigo = Console.ReadLine().Trim().ToUpper();

            // if-else: validar que no esté vacío
            if (string.IsNullOrWhiteSpace(codigo))
            {
                Advertencia("Debe ingresar un código.");
                Pausar();
                return;
            }

            int indice = -1;

            // for: recorrer el arreglo para localizar el libro
            for (int i = 0; i < totalLibros; i++)
            {
                if (libros[i].Codigo == codigo)
                {
                    indice = i;
                    break;
                }
            }

            // if-else: verificar si se encontró el código
            if (indice == -1)
            {
                Error($"No se encontró el código \"{codigo}\" en el registro.");
                Pausar();
                return;
            }

            Console.WriteLine();
            MostrarFichaLibro(libros[indice], indice + 1);

            // if-else: advertir si el libro tiene préstamos activos (sin stock)
            if (libros[indice].EjemplaresDisponibles == 0)
            {
                Console.WriteLine();
                Advertencia("Este libro no tiene ejemplares disponibles. Verifique préstamos activos antes de eliminar.");
            }

            // do-while: confirmar eliminación — repite hasta obtener S o N
            string respuesta;
            do
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\n  ¿Confirma la eliminación de este libro? (S/N): ");
                Console.ResetColor();
                respuesta = Console.ReadLine().Trim().ToUpper();

                if (respuesta != "S" && respuesta != "N")
                    Advertencia("Respuesta no válida. Ingrese S para confirmar o N para cancelar.");

            } while (respuesta != "S" && respuesta != "N");

            // if-else: procesar la respuesta final
            if (respuesta == "N")
            {
                Advertencia("Operación cancelada. El libro no fue eliminado.");
                Pausar();
                return;
            }

            string tituloEliminado = libros[indice].Titulo;
            string codigoEliminado = libros[indice].Codigo;

            // for: desplazar elementos hacia arriba para llenar el hueco
            for (int i = indice; i < totalLibros - 1; i++)
                libros[i] = libros[i + 1];

            // Limpiar el último slot duplicado y decrementar el contador
            libros[totalLibros - 1] = new Libro();
            totalLibros--;

            Console.WriteLine();
            Separador();
            Exito($"El libro \"{tituloEliminado}\" [{codigoEliminado}] fue eliminado del sistema.");
            Console.WriteLine($"  Libros en sistema: {totalLibros}/{MAX_LIBROS}");
            Pausar();
        }

        // ════════════════════════════════════════════════════
        //  MÉTODOS DE CAPTURA Y VALIDACIÓN
        // ════════════════════════════════════════════════════

        // do-while: capturar código válido y no duplicado
        private static string LeerCodigo(Libro[] libros, int totalLibros)
        {
            string codigo;

            do
            {
                Console.Write("\n  Código (8 caracteres alfanuméricos, ej. LIB00001): ");
                codigo = Console.ReadLine().Trim().ToUpper();

                // if-else: validar longitud
                if (codigo.Length != 8)
                {
                    Advertencia("El código debe tener exactamente 8 caracteres.");
                    codigo = "";
                    continue;
                }

                // if-else: validar que sea alfanumérico
                if (!EsAlfanumerico(codigo))
                {
                    Advertencia("El código solo puede contener letras y números (sin espacios ni símbolos).");
                    codigo = "";
                    continue;
                }

                // for: verificar que no esté duplicado
                bool duplicado = false;
                for (int i = 0; i < totalLibros; i++)
                {
                    if (libros[i].Codigo == codigo)
                    {
                        duplicado = true;
                        break;
                    }
                }

                // if-else: informar duplicado
                if (duplicado)
                {
                    Advertencia($"El código \"{codigo}\" ya está registrado. Ingrese uno diferente.");
                    codigo = "";
                }

            } while (string.IsNullOrWhiteSpace(codigo));

            return codigo;
        }

        // while: insistir hasta obtener texto no vacío
        private static string LeerTexto(string campo)
        {
            string valor = "";

            while (string.IsNullOrWhiteSpace(valor))
            {
                Console.Write($"  {campo}: ");
                valor = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(valor))
                    Advertencia($"El campo \"{campo}\" no puede estar vacío.");
            }

            return valor;
        }

        // switch-case: selección de categoría predefinida o libre
        private static string LeerCategoria()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  Categorías disponibles:");
            Console.ResetColor();
            Console.WriteLine("    1. Ciencias e Ingeniería");
            Console.WriteLine("    2. Literatura y Narrativa");
            Console.WriteLine("    3. Historia y Ciencias Sociales");
            Console.WriteLine("    4. Arte y Humanidades");
            Console.WriteLine("    5. Tecnología e Informática");
            Console.WriteLine("    6. Derecho y Economía");
            Console.WriteLine("    7. Otra (ingresar manualmente)");
            Console.Write("\n  Seleccione una opción: ");

            string opcion = Console.ReadLine().Trim();
            string categoria;

            // switch: asignar categoría según opción seleccionada
            switch (opcion)
            {
                case "1":
                    categoria = "Ciencias e Ingeniería";
                    break;
                case "2":
                    categoria = "Literatura y Narrativa";
                    break;
                case "3":
                    categoria = "Historia y Ciencias Sociales";
                    break;
                case "4":
                    categoria = "Arte y Humanidades";
                    break;
                case "5":
                    categoria = "Tecnología e Informática";
                    break;
                case "6":
                    categoria = "Derecho y Economía";
                    break;
                case "7":
                    categoria = LeerTexto("Ingrese la categoría");
                    break;
                default:
                    Advertencia("Opción no válida. Se asignará categoría manualmente.");
                    categoria = LeerTexto("Ingrese la categoría");
                    break;
            }

            return categoria;
        }

        // while + try-catch: leer año con validación de rango
        private static int LeerAnio()
        {
            int  anio       = 0;
            int  anioActual = DateTime.Now.Year;
            bool valido     = false;

            while (!valido)
            {
                Console.Write($"  Año de publicación ({ANIO_MIN} – {anioActual}): ");

                try
                {
                    anio = int.Parse(Console.ReadLine());

                    // if-else: verificar rango permitido
                    if (anio < ANIO_MIN || anio > anioActual)
                        Advertencia($"El año debe estar entre {ANIO_MIN} y {anioActual}.");
                    else
                        valido = true;
                }
                catch (FormatException)
                {
                    Advertencia("Ingrese un año válido (solo dígitos, sin espacios ni letras).");
                }
                catch (OverflowException)
                {
                    Advertencia("El valor ingresado está fuera del rango numérico permitido.");
                }
            }

            return anio;
        }

        // while + try-catch: leer entero mayor o igual a cero
        private static int LeerEnteroPositivo(string campo)
        {
            int  valor  = 0;
            bool valido = false;

            while (!valido)
            {
                Console.Write($"  {campo}: ");

                try
                {
                    valor = int.Parse(Console.ReadLine());

                    // if-else: rechazar negativos
                    if (valor < 0)
                        Advertencia("El valor no puede ser negativo (mínimo: 0).");
                    else
                        valido = true;
                }
                catch (FormatException)
                {
                    Advertencia("Ingrese un número entero válido.");
                }
                catch (OverflowException)
                {
                    Advertencia("El número ingresado es demasiado grande.");
                }
            }

            return valor;
        }

        // ════════════════════════════════════════════════════
        //  CLASIFICACIÓN DE STOCK (switch auxiliar)
        // ════════════════════════════════════════════════════

        // switch: devuelve nivel de stock numérico (0=sin stock, 1=bajo, 2=OK)
        private static int ObtenerNivelStock(int ejemplares)
        {
            if (ejemplares == 0)       return 0;
            if (ejemplares <= STOCK_BAJO) return 1;
            return 2;
        }

        // switch: devuelve etiqueta de estado según nivel de stock
        private static string ObtenerEstadoStock(int ejemplares)
        {
            switch (ObtenerNivelStock(ejemplares))
            {
                case 0:  return "Sin stock";
                case 1:  return "Stock bajo";
                default: return "Disponible";
            }
        }

        // switch: devuelve color de consola según nivel de stock
        private static ConsoleColor ObtenerColorStock(int ejemplares)
        {
            switch (ObtenerNivelStock(ejemplares))
            {
                case 0:  return ConsoleColor.Red;
                case 1:  return ConsoleColor.Yellow;
                default: return ConsoleColor.Green;
            }
        }

        // ════════════════════════════════════════════════════
        //  PRESENTACIÓN DE FICHA DE LIBRO
        // ════════════════════════════════════════════════════
        private static void MostrarFichaLibro(Libro l, int numero)
        {
            ConsoleColor colorStock = ObtenerColorStock(l.EjemplaresDisponibles);
            string       estadoStock = ObtenerEstadoStock(l.EjemplaresDisponibles);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"  ┌─── Libro N° {numero} " + new string('─', 34) + "┐");
            Console.ResetColor();

            FilaFicha("Código",      l.Codigo);
            FilaFicha("Título",      l.Titulo);
            FilaFicha("Autor",       l.Autor);
            FilaFicha("Editorial",   l.Editorial);
            FilaFicha("Año",         l.AnioPublicacion.ToString());
            FilaFicha("Categoría",   l.Categoria);

            // Ejemplares con color según nivel de stock
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("  │  Ejemplares  : ");
            Console.ForegroundColor = colorStock;
            Console.WriteLine($"{$"{l.EjemplaresDisponibles} ({estadoStock})",-31}│");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  └" + new string('─', 49) + "┘");
            Console.ResetColor();
        }

        private static void FilaFicha(string etiqueta, string valor)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("  │  ");
            Console.ResetColor();
            Console.Write($"{etiqueta,-12}: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{Cortar(valor, 31),-31}│");
            Console.ResetColor();
        }

        // ════════════════════════════════════════════════════
        //  UTILIDADES
        // ════════════════════════════════════════════════════

        // for: verificar que cada carácter sea letra o dígito
        private static bool EsAlfanumerico(string texto)
        {
            for (int i = 0; i < texto.Length; i++)
            {
                if (!char.IsLetterOrDigit(texto[i]))
                    return false;
            }
            return true;
        }

        private static string Cortar(string texto, int max)
        {
            if (string.IsNullOrEmpty(texto)) return "";
            return texto.Length <= max ? texto : texto.Substring(0, max - 1) + "…";
        }

        private static void Encabezado(string titulo)
        {
            int ancho = 52;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n  ╔" + new string('═', ancho) + "╗");
            Console.WriteLine($"  ║  {titulo.PadRight(ancho - 2)}║");
            Console.WriteLine("  ╚" + new string('═', ancho) + "╝\n");
            Console.ResetColor();
        }

        private static void Separador()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  " + new string('─', 115));
            Console.ResetColor();
        }

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

        private static void Pausar()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  Presione cualquier tecla para continuar...");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
