using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoPyme.Models
{
	public class Productos
	{
		[Key]
		public int ProductoId { get; set; }

		[Required(ErrorMessage = "El nombre del producto es obligatorio.")]
		[StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
		public string Nombre { get; set; } = string.Empty;

		[Required(ErrorMessage = "El precio es obligatorio.")]
		[Range(0.01, 9999999.99, ErrorMessage = "El precio debe ser mayor a 0.")]
		[Column(TypeName = "decimal(10,2)")]
		public decimal Precio { get; set; }

		[Required(ErrorMessage = "Debe seleccionar una categoría.")]
		[Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
		public int CategoriaId { get; set; }

		public Categoria? Categoria { get; set; }

		[Required(ErrorMessage = "Debe seleccionar una esencia.")]
		[Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una esencia válida.")]
		public int EsenciaId { get; set; }

		public Esencia? Esencia { get; set; }

		public bool Disponibilidad { get; set; }

		public string? RutaImagen { get; set; }

		[Required(ErrorMessage = "El stock es obligatorio.")]
		[Range(0, 999999, ErrorMessage = "El stock no puede ser negativo.")]
		public int Stock { get; set; }

		public ICollection<Carrito>? Carritos { get; set; }
	}
}