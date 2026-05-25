// ============================================================
//  BibliotecaUDB | Modelos/Libro.cs
//  Struct que representa un libro en la biblioteca.
//  Máximo 10 instancias en el arreglo libros[].
// ============================================================

namespace BibliotecaUDB.Modelos
{
    struct Libro
    {
        public string Codigo;                // Alfanumérico, exactamente 8 caracteres. Ej: LIB00001
        public string Titulo;                // No vacío
        public string Autor;                 // No vacío
        public string Editorial;             // No vacío
        public int    AnioPublicacion;       // Entre 1900 y año actual
        public string Categoria;             // No vacío
        public int    EjemplaresDisponibles; // Entero >= 0
    }
}
