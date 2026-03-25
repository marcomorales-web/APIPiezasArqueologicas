namespace ApiPiezasArqueologicas.Models
{
    public class Pieza
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Origen { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string Topico { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public string Proveedor { get; set; } = string.Empty;
        public string Coleccion { get; set; } = string.Empty;
    }
}
