using System.ComponentModel.DataAnnotations;

namespace ProyectoPyme.Models.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "El nombre es obligatorio.")]
		[StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
		public string Nombre { get; set; } = string.Empty;

		[Required(ErrorMessage = "El correo es obligatorio.")]
		[EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
		[StringLength(100, ErrorMessage = "El correo no puede superar los 100 caracteres.")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "La contraseña es obligatoria.")]
		[StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$",
			ErrorMessage = "La contraseña debe contener al menos una letra y un número.")]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage = "Debe confirmar la contraseña.")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}