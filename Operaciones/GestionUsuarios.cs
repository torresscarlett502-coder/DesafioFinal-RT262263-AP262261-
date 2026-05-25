// ============================================================
//  BibliotecaUDB | Operaciones/GestionUsuarios.cs
//  Módulo B – Gestión de Usuarios
//
//  Estructuras de control presentes:
//    ✔ if-else    → validaciones, condiciones, flujo de datos
//    ✔ switch     → menú de búsqueda, selección de estado/carrera
//    ✔ for        → recorrer arreglos, buscar, listar
//    ✔ while      → reintentar entradas inválidas
//    ✔ do-while   → captura obligatoria de carné y confirmaciones
//    ✔ try-catch  → manejo de excepciones en parseo numérico
// ============================================================

using System;
using BibliotecaUDB.Modelos;

namespace BibliotecaUDB.Operaciones
{
    static class GestionUsuarios
    {
        // ────────────────────────────────────────────────────
        //  Constantes del módulo
        // ────────────────────────────────────────────────────
        private const int MAX_USUARIOS   = 5;
        private const int LONGITUD_CARNE = 8;

        // ════════════════════════════════════════════════════
        //  REGISTRAR USUARIO
        // ════════════════════════════════════════════════════
        public static void RegistrarUsuario(Usuario[] usuarios, ref int totalUsuarios)
        {
            Console.Clear();
            Encabezado("REGISTRAR NUEVO USUARIO");

            // if-else: verificar que no se supere el límite del arreglo
            if (totalUsuarios >= MAX_USUARIOS)
            {
                Error($"Límite alcanzado. El sistema admite un máximo de {MAX_USUARIOS} usuarios.");
                Pausar();
                return;
            }

            Usuario usuario = new Usuario();

            // ── Carné ────────────────────────────────────────
            usuario.Carne = LeerCarne(usuarios, totalUsuarios);

            // ── Nombre completo ──────────────────────────────
            usuario.NombreCompleto = LeerTexto("Nombre completo");

            // ── Carrera ──────────────────────────────────────
            usuario.Carrera = LeerCarrera();

            // ── Correo electrónico ───────────────────────────
            usuario.CorreoElectronico = LeerCorreo();

            // ── Teléfono ─────────────────────────────────────
            usuario.Telefono = LeerTelefono();

            // ── Estado ───────────────────────────────────────
            usuario.Estado = LeerEstado();

            usuarios[totalUsuarios] = usuario;
            totalUsuarios++;

            Console.WriteLine();
            Separador();
            Exito($"Usuario \"{usuario.NombreCompleto}\" registrado con carné [{usuario.Carne}].");

            // switch: mostrar mensaje adicional según el estado asignado
            switch (usuario.Estado.ToLower())
            {
                case "activo":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("  El usuario puede solicitar préstamos de inmediato.");
                    break;

                case "inactivo":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("  El usuario está registrado pero no puede solicitar préstamos.");
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("  Estado del usuario registrado.");
                    break;
            }

            Console.ResetColor();
            Console.WriteLine($"  Usuarios en sistema: {totalUsuarios}/{MAX_USUARIOS}");
            Pausar();
        }

        // ════════════════════════════════════════════════════
        //  BUSCAR USUARIO (por carné o por nombre)
        // ════════════════════════════════════════════════════
        public static void BuscarUsuario(Usuario[] usuarios, int totalUsuarios)
        {
            Console.Clear();
            Encabezado("BUSCAR USUARIO");

            // if-else: verificar que haya usuarios registrados
            if (totalUsuarios == 0)
            {
                Advertencia("No hay usuarios registrados en el sistema.");
                Pausar();
                return;
            }

            // switch-case: seleccionar criterio de búsqueda
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  Criterio de búsqueda:");
            Console.ResetColor();
            Console.WriteLine("    1. Por carné (coincidencia exacta)");
            Console.WriteLine("    2. Por nombre (búsqueda parcial)");
            Console.Write("\n  Seleccione una opción: ");

            string opcion = Console.ReadLine().Trim();
            Console.WriteLine();

            switch (opcion)
            {
                case "1":
                    BuscarPorCarne(usuarios, totalUsuarios);
                    break;

                case "2":
                    BuscarPorNombre(usuarios, totalUsuarios);
                    break;

                default:
                    Advertencia("Opción no válida. Regresando al menú de usuarios.");
                    break;
            }

            Pausar();
        }

        // ════════════════════════════════════════════════════
        //  BUSCAR POR CARNÉ — coincidencia exacta
        // ════════════════════════════════════════════════════
        public static void BuscarPorCarne(Usuario[] usuarios, int totalUsuarios)
        {
            Console.Write("  Carné a buscar (8 dígitos): ");
            string inputCarne = Console.ReadLine().Trim();

            // if-else: validar que no esté vacío
            if (string.IsNullOrWhiteSpace(inputCarne))
            {
                Advertencia("Debe ingresar un carné para realizar la búsqueda.");
                return;
            }

            int pos = -1;

            // for: recorrer el arreglo comparando carné con igualdad exacta
            for (int i = 0; i < totalUsuarios; i++)
            {
                if (usuarios[i].Carne == inputCarne)
                {
                    pos = i;
                    break;
                }
            }

            Console.WriteLine();
            Separador();

            // if-else: mostrar resultado o mensaje de no encontrado
            if (pos != -1)
            {
                Exito($"Usuario encontrado con carné [{inputCarne}]:");
                Console.WriteLine();
                MostrarFichaUsuario(usuarios[pos], pos + 1);
            }
            else
            {
                Error($"No se encontró ningún usuario con el carné \"{inputCarne}\".");
            }
        }

        // ════════════════════════════════════════════════════
        //  BUSCAR POR NOMBRE — búsqueda parcial
        // ════════════════════════════════════════════════════
        public static void BuscarPorNombre(Usuario[] usuarios, int totalUsuarios)
        {
            Console.Write("  Nombre o fragmento a buscar: ");
            string fragmento = Console.ReadLine().Trim();

            // if-else: validar entrada
            if (string.IsNullOrWhiteSpace(fragmento))
            {
                Advertencia("Debe ingresar al menos una palabra para la búsqueda.");
                return;
            }

            int encontrados = 0;

            // for: recorrer usuarios buscando coincidencia parcial en nombre
            for (int i = 0; i < totalUsuarios; i++)
            {
                // if-else: comparación parcial insensible a mayúsculas
                if (usuarios[i].NombreCompleto.ToLower().Contains(fragmento.ToLower()))
                {
                    // if-else: primer resultado → imprimir encabezado
                    if (encontrados == 0)
                    {
                        Separador();
                        Exito($"Resultados que contienen \"{fragmento}\":");
                        Console.WriteLine();
                    }

                    MostrarFichaUsuario(usuarios[i], i + 1);
                    Console.WriteLine();
                    encontrados++;
                }
            }

            // if-else: verificar si hubo resultados
            if (encontrados == 0)
            {
                Separador();
                Error($"Ningún usuario tiene \"{fragmento}\" en su nombre.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  Total de coincidencias: {encontrados}");
                Console.ResetColor();
            }
        }

        // ════════════════════════════════════════════════════
        //  LISTAR TODOS LOS USUARIOS
        // ════════════════════════════════════════════════════
        public static void ListarUsuarios(Usuario[] usuarios, int totalUsuarios)
        {
            Console.Clear();
            Encabezado("LISTADO COMPLETO DE USUARIOS");

            // if-else: verificar que haya usuarios registrados
            if (totalUsuarios == 0)
            {
                Advertencia("No hay usuarios registrados en el sistema.");
                Pausar();
                return;
            }

            // Contadores de resumen estadístico
            int activos   = 0;
            int inactivos = 0;

            // for: primer recorrido — calcular estadísticas de estado
            for (int i = 0; i < totalUsuarios; i++)
            {
                // switch: clasificar usuario por estado
                switch (usuarios[i].Estado.ToLower())
                {
                    case "activo":
                        activos++;
                        break;
                    case "inactivo":
                        inactivos++;
                        break;
                }
            }

            // Mostrar resumen antes de la tabla
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(
                $"\n  Registrados: {totalUsuarios}/{MAX_USUARIOS}   " +
                $"Activos: {activos}   " +
                $"Inactivos: {inactivos}"
            );
            Console.ResetColor();
            Console.WriteLine();

            // Encabezado de la tabla
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(
                $"  {"N°",-4}" +
                $"{"CARNÉ",-11}" +
                $"{"NOMBRE COMPLETO",-28}" +
                $"{"CARRERA",-26}" +
                $"{"CORREO",-28}" +
                $"{"TELÉFONO",-13}" +
                $"{"ESTADO",-10}"
            );
            Separador();
            Console.ResetColor();

            // for: segundo recorrido — imprimir cada usuario en tabla
            for (int i = 0; i < totalUsuarios; i++)
            {
                // if-else: seleccionar color según estado
                bool esActivo = usuarios[i].Estado.ToLower() == "activo";

                Console.ForegroundColor = esActivo
                    ? ConsoleColor.White
                    : ConsoleColor.DarkGray;

                Console.WriteLine(
                    $"  {(i + 1),-4}" +
                    $"{usuarios[i].Carne,-11}" +
                    $"{Cortar(usuarios[i].NombreCompleto, 27),-28}" +
                    $"{Cortar(usuarios[i].Carrera,        25),-26}" +
                    $"{Cortar(usuarios[i].CorreoElectronico, 27),-28}" +
                    $"{usuarios[i].Telefono,-13}" +
                    $"{usuarios[i].Estado,-10}"
                );

                Console.ResetColor();
            }

            Separador();

            // Leyenda de estado
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;    Console.Write("  ■ Activo   ");
            Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("■ Inactivo");
            Console.ResetColor();
            Console.WriteLine("\n");

            Pausar();
        }

        // ════════════════════════════════════════════════════
        //  CAMBIAR ESTADO DE USUARIO
        // ════════════════════════════════════════════════════
        public static void CambiarEstadoUsuario(Usuario[] usuarios, int totalUsuarios)
        {
            Console.Clear();
            Encabezado("CAMBIAR ESTADO DE USUARIO");

            // if-else: verificar que haya usuarios
            if (totalUsuarios == 0)
            {
                Advertencia("No hay usuarios registrados en el sistema.");
                Pausar();
                return;
            }

            Console.Write("  Ingrese el carné del usuario: ");
            string inputCarne = Console.ReadLine().Trim();

            // if-else: validar entrada
            if (string.IsNullOrWhiteSpace(inputCarne))
            {
                Advertencia("Debe ingresar un carné.");
                Pausar();
                return;
            }

            int indice = -1;

            // for: buscar el usuario en el arreglo
            for (int i = 0; i < totalUsuarios; i++)
            {
                if (usuarios[i].Carne == inputCarne)
                {
                    indice = i;
                    break;
                }
            }

            // if-else: verificar si se encontró
            if (indice == -1)
            {
                Error($"No se encontró ningún usuario con el carné \"{inputCarne}\".");
                Pausar();
                return;
            }

            Console.WriteLine();
            MostrarFichaUsuario(usuarios[indice], indice + 1);

            // Estado actual y nuevo estado propuesto
            string estadoActual = usuarios[indice].Estado;
            string nuevoEstado  = estadoActual.ToLower() == "activo" ? "inactivo" : "activo";

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  Estado actual : {estadoActual}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  Nuevo estado  : {nuevoEstado}");
            Console.ResetColor();

            // do-while: confirmar cambio de estado hasta obtener S o N
            string respuesta;
            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n  ¿Confirma el cambio de estado? (S/N): ");
                Console.ResetColor();
                respuesta = Console.ReadLine().Trim().ToUpper();

                if (respuesta != "S" && respuesta != "N")
                    Advertencia("Respuesta no válida. Ingrese S para confirmar o N para cancelar.");

            } while (respuesta != "S" && respuesta != "N");

            // if-else: aplicar o cancelar el cambio
            if (respuesta == "N")
            {
                Advertencia("Operación cancelada. El estado no fue modificado.");
                Pausar();
                return;
            }

            usuarios[indice].Estado = nuevoEstado;

            Console.WriteLine();
            Separador();
            Exito($"Estado de \"{usuarios[indice].NombreCompleto}\" actualizado a [{nuevoEstado.ToUpper()}].");
            Pausar();
        }

        // ════════════════════════════════════════════════════
        //  MÉTODOS DE CAPTURA Y VALIDACIÓN
        // ════════════════════════════════════════════════════

        // do-while: capturar carné válido (8 dígitos) y no duplicado
        private static string LeerCarne(Usuario[] usuarios, int totalUsuarios)
        {
            string carne;

            do
            {
                Console.Write($"\n  Carné ({LONGITUD_CARNE} dígitos numéricos): ");
                carne = Console.ReadLine().Trim();

                // if-else: verificar longitud exacta
                if (carne.Length != LONGITUD_CARNE)
                {
                    Advertencia($"El carné debe tener exactamente {LONGITUD_CARNE} dígitos.");
                    carne = "";
                    continue;
                }

                // if-else: verificar que sean solo dígitos
                if (!SoloDigitos(carne))
                {
                    Advertencia("El carné solo puede contener dígitos numéricos (0-9), sin letras ni espacios.");
                    carne = "";
                    continue;
                }

                // for: verificar que no esté duplicado
                bool duplicado = false;
                for (int i = 0; i < totalUsuarios; i++)
                {
                    if (usuarios[i].Carne == carne)
                    {
                        duplicado = true;
                        break;
                    }
                }

                // if-else: informar duplicado
                if (duplicado)
                {
                    Advertencia($"El carné \"{carne}\" ya está registrado. Ingrese uno diferente.");
                    carne = "";
                }

            } while (string.IsNullOrWhiteSpace(carne));

            return carne;
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

        // switch-case: selección de carrera predefinida o manual
        private static string LeerCarrera()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  Carreras disponibles:");
            Console.ResetColor();
            Console.WriteLine("    1. Ingeniería en Sistemas Informáticos");
            Console.WriteLine("    2. Ingeniería Industrial");
            Console.WriteLine("    3. Ingeniería Civil");
            Console.WriteLine("    4. Administración de Empresas");
            Console.WriteLine("    5. Contaduría Pública");
            Console.WriteLine("    6. Derecho");
            Console.WriteLine("    7. Medicina");
            Console.WriteLine("    8. Arquitectura");
            Console.WriteLine("    9. Otra (ingresar manualmente)");
            Console.Write("\n  Seleccione una opción: ");

            string opcion  = Console.ReadLine().Trim();
            string carrera;

            // switch: asignar carrera según opción elegida
            switch (opcion)
            {
                case "1": carrera = "Ing. Sistemas Informáticos"; break;
                case "2": carrera = "Ing. Industrial";            break;
                case "3": carrera = "Ing. Civil";                 break;
                case "4": carrera = "Administración de Empresas"; break;
                case "5": carrera = "Contaduría Pública";         break;
                case "6": carrera = "Derecho";                    break;
                case "7": carrera = "Medicina";                   break;
                case "8": carrera = "Arquitectura";               break;
                case "9":
                    carrera = LeerTexto("Ingrese la carrera");
                    break;
                default:
                    Advertencia("Opción no válida. Se ingresará la carrera manualmente.");
                    carrera = LeerTexto("Ingrese la carrera");
                    break;
            }

            return carrera;
        }

        // while: validar formato de correo (debe tener @ y punto después del @)
        private static string LeerCorreo()
        {
            string correo = "";
            bool   valido = false;

            while (!valido)
            {
                Console.Write("  Correo electrónico: ");
                correo = Console.ReadLine().Trim();

                // if-else: verificar que no esté vacío
                if (string.IsNullOrWhiteSpace(correo))
                {
                    Advertencia("El correo no puede estar vacío.");
                    continue;
                }

                int posArroba = correo.IndexOf('@');

                // if-else: verificar que tenga @
                if (posArroba < 1)
                {
                    Advertencia("El correo debe contener el símbolo '@'.");
                    continue;
                }

                // if-else: verificar que haya texto antes del @
                if (posArroba == 0)
                {
                    Advertencia("El correo debe tener caracteres antes del '@'.");
                    continue;
                }

                string dominio = correo.Substring(posArroba + 1);

                // if-else: verificar que el dominio tenga un punto
                if (!dominio.Contains("."))
                {
                    Advertencia("El dominio del correo debe contener un punto (ejemplo: @gmail.com).");
                    continue;
                }

                // if-else: verificar que el punto no sea el último carácter
                if (dominio.LastIndexOf('.') >= dominio.Length - 2)
                {
                    Advertencia("El correo no tiene un dominio válido después del punto.");
                    continue;
                }

                valido = true;
            }

            return correo;
        }

        // while: validar teléfono no vacío con verificación de formato básico
        private static string LeerTelefono()
        {
            string telefono = "";
            bool   valido   = false;

            while (!valido)
            {
                Console.Write("  Teléfono (ej. 7890-1234): ");
                telefono = Console.ReadLine().Trim();

                // if-else: verificar que no esté vacío
                if (string.IsNullOrWhiteSpace(telefono))
                {
                    Advertencia("El teléfono no puede estar vacío.");
                    continue;
                }

                // if-else: verificar longitud mínima razonable
                if (telefono.Length < 7)
                {
                    Advertencia("El teléfono parece demasiado corto. Verifique el número ingresado.");
                    continue;
                }

                // for: verificar que los caracteres sean dígitos o guión
                bool formatoValido = true;
                for (int i = 0; i < telefono.Length; i++)
                {
                    // if-else: permitir solo dígitos, guiones y espacios
                    if (!char.IsDigit(telefono[i]) && telefono[i] != '-' && telefono[i] != ' ')
                    {
                        formatoValido = false;
                        break;
                    }
                }

                // if-else: informar error de formato
                if (!formatoValido)
                {
                    Advertencia("El teléfono solo puede contener dígitos, guiones y espacios.");
                    continue;
                }

                valido = true;
            }

            return telefono;
        }

        // switch-case: seleccionar estado activo o inactivo
        private static string LeerEstado()
        {
            string estado = "";

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  Estado inicial del usuario:");
            Console.ResetColor();
            Console.WriteLine("    1. Activo   (puede solicitar préstamos)");
            Console.WriteLine("    2. Inactivo (acceso restringido a préstamos)");
            Console.Write("\n  Seleccione una opción: ");

            string opcion = Console.ReadLine().Trim();

            // switch: asignar estado según opción
            switch (opcion)
            {
                case "1":
                    estado = "activo";
                    break;

                case "2":
                    estado = "inactivo";
                    break;

                default:
                    Advertencia("Opción no válida. Se asignará estado \"activo\" por defecto.");
                    estado = "activo";
                    break;
            }

            return estado;
        }

        // ════════════════════════════════════════════════════
        //  PRESENTACIÓN DE FICHA DE USUARIO
        // ════════════════════════════════════════════════════
        private static void MostrarFichaUsuario(Usuario u, int numero)
        {
            // switch: elegir color e ícono según estado del usuario
            ConsoleColor colorEstado;
            string       iconoEstado;

            switch (u.Estado.ToLower())
            {
                case "activo":
                    colorEstado = ConsoleColor.Green;
                    iconoEstado = "● ACTIVO";
                    break;

                case "inactivo":
                    colorEstado = ConsoleColor.DarkGray;
                    iconoEstado = "○ INACTIVO";
                    break;

                default:
                    colorEstado = ConsoleColor.Yellow;
                    iconoEstado = $"? {u.Estado.ToUpper()}";
                    break;
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"  ┌─── Usuario N° {numero} " + new string('─', 32) + "┐");
            Console.ResetColor();

            FilaFicha("Carné",          u.Carne);
            FilaFicha("Nombre",         u.NombreCompleto);
            FilaFicha("Carrera",        u.Carrera);
            FilaFicha("Correo",         u.CorreoElectronico);
            FilaFicha("Teléfono",       u.Telefono);

            // Fila de estado con color diferenciado
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("  │  ");
            Console.ResetColor();
            Console.Write($"{"Estado",-12}: ");
            Console.ForegroundColor = colorEstado;
            Console.WriteLine($"{iconoEstado,-31}│");
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

        // for: verificar que todos los caracteres sean dígitos
        private static bool SoloDigitos(string texto)
        {
            for (int i = 0; i < texto.Length; i++)
            {
                if (!char.IsDigit(texto[i]))
                    return false;
            }
            return true;
        }

        // Método público para verificar si un carné existe (usado por GestionPrestamos)
        public static int BuscarIndicePorCarne(Usuario[] usuarios, int totalUsuarios, string carne)
        {
            // for: recorrer el arreglo buscando el carné exacto
            for (int i = 0; i < totalUsuarios; i++)
            {
                if (usuarios[i].Carne == carne)
                    return i;
            }
            return -1;
        }

        // Método público para verificar si un usuario está activo (usado por GestionPrestamos)
        public static bool EstaActivo(Usuario usuario)
        {
            return usuario.Estado.ToLower() == "activo";
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
            Console.WriteLine("  " + new string('─', 110));
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
