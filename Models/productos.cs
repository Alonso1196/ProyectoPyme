using System.ComponentModel.DataAnnotations;

namespace ProyectoPyme.Models
{
    public class Productos
    {
        [Key]
        public int ProductoId { get; set; }

        [Required]
        public string Nombre { get; set; }

        public decimal Precio { get; set; }

        public int CategoriaId { get; set; }

        public Categoria? Categoria { get; set; }

        public bool Disponibilidad { get; set; }

        public string? RutaImagen { get; set; }

    }
}
