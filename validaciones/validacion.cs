/ ============================================================
//  BibliotecaUDB | Validaciones/validacion.cs
//  Validaciones centralizadas para los archivos CSV
// ============================================================

using System;

namespace BibliotecaUDB.Validaciones
{
    // Estructura pública para devolver el resultado de las validaciones
    public struct ResultadoValidacion
    {
        public bool Valido;
        public string Mensaje;

        public static ResultadoValidacion Ok()
        {
            return new ResultadoValidacion { Valido = true, Mensaje = "OK" };
        }

        public static ResultadoValidacion Fallo(string mensaje)
        {
            return new ResultadoValidacion { Valido = false, Mensaje = mensaje };
        }
    }

    public static class Validador
    {
        // ════════════════════════════════════════════════════
        // VALIDACIÓN DE LIBROS (7 campos)
        // ════════════════════════════════════════════════════
        public static ResultadoValidacion ValidarCamposCSVLibro(string[] campos)
        {
            if (campos == null || campos.Length != 7)
                return ResultadoValidacion.Fallo($"Se esperaban 7 campos y se recibieron {(campos == null ? 0 : campos.Length)}.");

            if (string.IsNullOrWhiteSpace(campos[0])) return ResultadoValidacion.Fallo("El código está vacío.");
            if (string.IsNullOrWhiteSpace(campos[1])) return ResultadoValidacion.Fallo("El título está vacío.");
            
            // Validar que Año y Ejemplares sean números válidos
            if (!int.TryParse(campos[4].Trim(), out _)) return ResultadoValidacion.Fallo("El año de publicación no es numérico.");
            if (!int.TryParse(campos[6].Trim(), out _)) return ResultadoValidacion.Fallo("La cantidad de ejemplares no es numérica.");

            return ResultadoValidacion.Ok();
        }

        // ════════════════════════════════════════════════════
        // VALIDACIÓN DE PRÉSTAMOS (5 campos)
        // ════════════════════════════════════════════════════
        public static ResultadoValidacion ValidarCamposCSVPrestamo(string[] campos)
        {
            if (campos == null || campos.Length != 5)
                return ResultadoValidacion.Fallo($"Se esperaban 5 campos y se recibieron {(campos == null ? 0 : campos.Length)}.");

            if (string.IsNullOrWhiteSpace(campos[0])) return ResultadoValidacion.Fallo("El carné del usuario está vacío.");
            if (string.IsNullOrWhiteSpace(campos[1])) return ResultadoValidacion.Fallo("El código del libro está vacío.");
            if (string.IsNullOrWhiteSpace(campos[2])) return ResultadoValidacion.Fallo("La fecha de préstamo está vacía.");
            if (string.IsNullOrWhiteSpace(campos[3])) return ResultadoValidacion.Fallo("La fecha de devolución está vacía.");
            if (string.IsNullOrWhiteSpace(campos[4])) return ResultadoValidacion.Fallo("El estado del préstamo está vacío.");

            return ResultadoValidacion.Ok();
        }
    }
}
