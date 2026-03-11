using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoPyme.Models
{
    public class Carrito
    {
        [Key]
        public int IdCarrito { get; set; }

        public int UsuarioId { get; set; }

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        [ForeignKey("IdProducto")]
        public Productos Producto { get; set; }
    }
}

