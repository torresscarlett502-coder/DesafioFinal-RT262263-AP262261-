// ============================================================
//  BibliotecaUDB | BaseDeDatos/ArchivoUsuarios.cs
//  Persistencia – Módulo B (Usuarios)
//
//  CORRECCIÓN respecto a una implementación básica:
//    ✔ Métodos Cargar() y Guardar() para uso desde MenuPrincipal.cs
//    ✔ Validación centralizada de cada línea CSV dentro de este archivo
//    ✔ Manejo de errores por línea inválida sin detener la carga
//    ✔ Control de duplicados por carné
//
//  Estructuras de control:
//    ✔ if-else    → existencia de archivo/carpeta, límites, validaciones
//    ✔ for        → recorrer el arreglo al guardar y verificar duplicados
//    ✔ while      → leer línea a línea hasta EOF
//    ✔ try-catch  → IOException, FormatException, OverflowException
//
//  Formato CSV (6 campos, sin cabecera):
//    Carne,NombreCompleto,Carrera,CorreoElectronico,Telefono,Estado
// ============================================================

using System;
using System.IO;
using BibliotecaUDB.Modelos;

namespace BibliotecaUDB.BaseDeDatos
{
    static class ArchivoUsuarios
    {
        // ────────────────────────────────────────────────────
        //  Constantes del módulo
        // ────────────────────────────────────────────────────
        private const int MAX_USUARIOS = 5;
        private const int CAMPOS_CSV   = 6;
        private const int LONGITUD_CARNE = 8;

        private const string CARPETA = "data";
        private static readonly string ARCHIVO = Path.Combine(CARPETA, "usuarios.csv");

        // ════════════════════════════════════════════════════
        //  CARGAR  →  archivo CSV  ──►  arreglo en memoria
        // ════════════════════════════════════════════════════
        /// <summary>
        /// Lee usuarios.csv y llena el arreglo usuarios[].
        /// Si el archivo no existe, arranca con usuarios vacíos.
        /// Cada línea se valida antes de convertirse en un struct Usuario.
        /// </summary>
        public static void Cargar(Usuario[] usuarios, ref int totalUsuarios)
        {
            totalUsuarios = 0;

            // if-else: primer arranque — archivo aún no existe
            if (!File.Exists(ARCHIVO))
            {
                Informacion($"Archivo \"{ARCHIVO}\" no encontrado. Se iniciará con usuarios vacíos.");
                return;
            }

            try
            {
                StreamReader sr = new StreamReader(ARCHIVO);
                string linea;
                int nLinea = 0;
                int ignoradas = 0;

                // while: leer línea a línea hasta fin de archivo (EOF)
                while ((linea = sr.ReadLine()) != null)
                {
                    nLinea++;

                    // if-else: ignorar líneas vacías
                    if (string.IsNullOrWhiteSpace(linea))
                        continue;

                    // if-else: arreglo lleno → detener lectura
                    if (totalUsuarios >= MAX_USUARIOS)
                    {
                        Advertencia(
                            $"Límite de {MAX_USUARIOS} usuarios alcanzado. " +
                            $"Las líneas restantes del archivo serán ignoradas."
                        );
                        break;
                    }

                    string[] campos = linea.Split(',');

                    // ── VALIDACIÓN CENTRALIZADA ───────────────────────
                    ResultadoValidacion rv = ValidarCamposCSVUsuario(campos, usuarios, totalUsuarios);

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
                        Usuario usuario = new Usuario();
                        usuario.Carne             = campos[0].Trim();
                        usuario.NombreCompleto     = campos[1].Trim();
                        usuario.Carrera            = campos[2].Trim();
                        usuario.CorreoElectronico  = campos[3].Trim();
                        usuario.Telefono           = campos[4].Trim();
                        usuario.Estado             = NormalizarEstado(campos[5].Trim());

                        usuarios[totalUsuarios] = usuario;
                        totalUsuarios++;
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

                sr.Close();

                // if-else: informe final diferenciado según resultado
                if (totalUsuarios > 0)
                    Exito($"Usuarios cargados: {totalUsuarios} registro(s) desde \"{ARCHIVO}\"." +
                          (ignoradas > 0 ? $" ({ignoradas} línea(s) ignorada(s))" : ""));
                else
                    Informacion($"\"{ARCHIVO}\" existe pero no contiene registros válidos.");
            }
            catch (IOException ex)
            {
                Error($"Error de acceso al archivo \"{ARCHIVO}\": {ex.Message}");
                Error("El sistema continuará sin datos de usuarios.");
            }
            catch (Exception ex)
            {
                Error($"Error inesperado al cargar usuarios: {ex.Message}");
            }
        }

        // ════════════════════════════════════════════════════
        //  GUARDAR  →  arreglo en memoria  ──►  archivo CSV
        // ════════════════════════════════════════════════════
        /// <summary>
        /// Sobreescribe usuarios.csv con el contenido actual del arreglo.
        /// Crea la carpeta data/ si aún no existe.
        /// </summary>
        public static void Guardar(Usuario[] usuarios, int totalUsuarios)
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
                for (int i = 0; i < totalUsuarios; i++)
                {
                    // El orden de campos DEBE coincidir con Cargar()
                    sw.WriteLine(
                        EscaparCampo(usuarios[i].Carne) + "," +
                        EscaparCampo(usuarios[i].NombreCompleto) + "," +
                        EscaparCampo(usuarios[i].Carrera) + "," +
                        EscaparCampo(usuarios[i].CorreoElectronico) + "," +
                        EscaparCampo(usuarios[i].Telefono) + "," +
                        EscaparCampo(NormalizarEstado(usuarios[i].Estado))
                    );
                }

                sw.Close();

                // if-else: mensaje diferenciado si no hay registros
                if (totalUsuarios > 0)
                    Exito($"Usuarios guardados: {totalUsuarios} registro(s) en \"{ARCHIVO}\".");
                else
                    Informacion($"No hay usuarios que guardar. \"{ARCHIVO}\" quedará vacío.");
            }
            catch (IOException ex)
            {
                Error($"Error de escritura en \"{ARCHIVO}\": {ex.Message}");
                Error("Los datos NO fueron guardados. Verifique permisos de escritura.");
            }
            catch (Exception ex)
            {
                Error($"Error inesperado al guardar usuarios: {ex.Message}");
            }
        }

        // ════════════════════════════════════════════════════
        //  VALIDACIÓN DE REGISTROS CSV
        // ════════════════════════════════════════════════════
        private static ResultadoValidacion ValidarCamposCSVUsuario(
            string[] campos, Usuario[] usuarios, int totalUsuarios)
        {
            // if-else: cantidad exacta de campos
            if (campos == null || campos.Length != CAMPOS_CSV)
                return ResultadoValidacion.Fallo(
                    $"Se esperaban {CAMPOS_CSV} campos y se recibieron {(campos == null ? 0 : campos.Length)}."
                );

            string carne            = campos[0].Trim();
            string nombreCompleto    = campos[1].Trim();
            string carrera           = campos[2].Trim();
            string correoElectronico = campos[3].Trim();
            string telefono          = campos[4].Trim();
            string estado            = campos[5].Trim();

            // if-else: campos obligatorios no vacíos
            if (string.IsNullOrWhiteSpace(carne))
                return ResultadoValidacion.Fallo("El carné está vacío.");
            if (string.IsNullOrWhiteSpace(nombreCompleto))
                return ResultadoValidacion.Fallo("El nombre completo está vacío.");
            if (string.IsNullOrWhiteSpace(carrera))
                return ResultadoValidacion.Fallo("La carrera está vacía.");
            if (string.IsNullOrWhiteSpace(correoElectronico))
                return ResultadoValidacion.Fallo("El correo electrónico está vacío.");
            if (string.IsNullOrWhiteSpace(telefono))
                return ResultadoValidacion.Fallo("El teléfono está vacío.");
            if (string.IsNullOrWhiteSpace(estado))
                return ResultadoValidacion.Fallo("El estado está vacío.");

            // if-else: carné numérico de exactamente 8 dígitos
            if (carne.Length != LONGITUD_CARNE)
                return ResultadoValidacion.Fallo($"El carné \"{carne}\" debe tener exactamente {LONGITUD_CARNE} dígitos.");
            if (!SoloDigitos(carne))
                return ResultadoValidacion.Fallo($"El carné \"{carne}\" debe contener solo dígitos.");

            // if-else: correo electrónico básico
            if (!CorreoValido(correoElectronico))
                return ResultadoValidacion.Fallo($"El correo \"{correoElectronico}\" no tiene un formato válido.");

            // if-else: teléfono básico (dígitos, espacios y guion)
            if (!TelefonoValido(telefono))
                return ResultadoValidacion.Fallo($"El teléfono \"{telefono}\" no tiene un formato válido.");

            // if-else: estado permitido
            string estadoNorm = estado.ToLower();
            if (estadoNorm != "activo" && estadoNorm != "inactivo")
                return ResultadoValidacion.Fallo($"El estado \"{estado}\" debe ser \"activo\" o \"inactivo\".");

            // if-else: carné duplicado en el archivo cargado hasta el momento
            for (int i = 0; i < totalUsuarios; i++)
            {
                if (usuarios[i].Carne == carne)
                    return ResultadoValidacion.Fallo($"El carné \"{carne}\" está duplicado en el archivo.");
            }

            return ResultadoValidacion.Ok();
        }

        // ════════════════════════════════════════════════════
        //  UTILIDADES DE VALIDACIÓN
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

        private static bool TelefonoValido(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return false;

            for (int i = 0; i < telefono.Length; i++)
            {
                if (!char.IsDigit(telefono[i]) && telefono[i] != '-' && telefono[i] != ' ')
                    return false;
            }

            return true;
        }

        private static bool CorreoValido(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            int posArroba = correo.IndexOf('@');
            if (posArroba < 1)
                return false;

            string dominio = correo.Substring(posArroba + 1);
            if (!dominio.Contains("."))
                return false;

            int ultimoPunto = dominio.LastIndexOf('.');
            if (ultimoPunto < 1 || ultimoPunto >= dominio.Length - 1)
                return false;

            return true;
        }

        private static string NormalizarEstado(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
                return "inactivo";

            string normalizado = estado.Trim().ToLower();

            if (normalizado == "activo")
                return "activo";

            if (normalizado == "inactivo")
                return "inactivo";

            return normalizado;
        }

        private static string EscaparCampo(string valor)
        {
            if (valor == null)
                return "";

            // CSV simple sin comillas. Se reemplazan saltos de línea y comas
            // para evitar romper el archivo.
            return valor.Replace("\r", " ")
                        .Replace("\n", " ")
                        .Replace(",", " ").Trim();
        }

        // ════════════════════════════════════════════════════
        //  TIPOS AUXILIARES DE VALIDACIÓN
        // ════════════════════════════════════════════════════
        private struct ResultadoValidacion
        {
            public bool Valido;
            public string Mensaje;

            public static ResultadoValidacion Ok()
            {
                return new ResultadoValidacion
                {
                    Valido = true,
                    Mensaje = "OK"
                };
            }

            public static ResultadoValidacion Fallo(string mensaje)
            {
                return new ResultadoValidacion
                {
                    Valido = false,
                    Mensaje = mensaje
                };
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
