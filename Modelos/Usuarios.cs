// ============================================================
//  BibliotecaUDB | Modelos/Usuario.cs
//  Struct que representa un usuario de la biblioteca.
//  Máximo 5 instancias en el arreglo usuarios[].
// ============================================================

namespace BibliotecaUDB.Modelos
{
    struct Usuario
    {
        public string Carne;              // Exactamente 8 dígitos numéricos. Ej: 20230001
        public string NombreCompleto;     // No vacío
        public string Carrera;            // No vacío
        public string CorreoElectronico;  // Debe contener '@' y un punto después del @
        public string Telefono;           // No vacío, solo dígitos y guiones
        public string Estado;             // "activo" o "inactivo"  (minúsculas)
    }
}
