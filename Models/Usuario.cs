namespace ProyectoPyme.Models
{
	public class Usuario
	{
		public int Id { get; set; }

		public string Nombre { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public string PasswordHash { get; set; } = string.Empty;

		public bool Activo { get; set; }

		public int RolId { get; set; }

		public Rol? Rol { get; set; }
	}
}