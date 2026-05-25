// ============================================================
//  BibliotecaUDB | Modelos/Prestamo.cs
//  Struct que vincula un Usuario con un Libro.
//  Fechas almacenadas como cadenas en formato dd/mm/yyyy.
//  Máximo 10 instancias en el arreglo prestamos[].
// ============================================================

namespace BibliotecaUDB.Modelos
{
    struct Prestamo
    {
        public string CarneUsuario;    // FK → Usuario.Carne
        public string CodigoLibro;     // FK → Libro.Codigo
        public string FechaPrestamo;   // Formato dd/mm/yyyy
        public string FechaDevolucion; // Formato dd/mm/yyyy — debe ser >= FechaPrestamo
        public string Estado;          // "activo" o "devuelto"
    }
}
