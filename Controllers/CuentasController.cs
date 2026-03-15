using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoPyme.Models;
using ProyectoPyme.Models.ViewModels;
using System.Security.Claims;

namespace ProyectoPyme.Controllers
{
	public class CuentasController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CuentasController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View(new RegisterViewModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(string email, string password)
		{
			if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
			{
				ViewBag.Error = "Debe ingresar correo y contraseña.";
				return View();
			}

			var emailNormalizado = email.Trim().ToLower();

			var usuario = await _context.Usuarios
				.Include(u => u.Rol)
				.FirstOrDefaultAsync(u => u.Email.ToLower() == emailNormalizado);

			if (usuario == null || usuario.PasswordHash != password)
			{
				ViewBag.Error = "Correo o contraseña incorrectos.";
				return View();
			}

			if (!usuario.Activo)
			{
				ViewBag.Error = "Su cuenta se encuentra inactiva. Por favor, contacte con un administrador para reactivarla.";
				return View();
			}

			var rolNombre = usuario.Rol?.Nombre ?? "Cliente";

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
				new Claim(ClaimTypes.Name, string.IsNullOrWhiteSpace(usuario.Nombre) ? usuario.Email : usuario.Nombre),
				new Claim(ClaimTypes.Email, usuario.Email),
				new Claim(ClaimTypes.Role, rolNombre)
			};

			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(identity);

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var nombreNormalizado = model.Nombre.Trim();
			var emailNormalizado = model.Email.Trim().ToLower();

			var existe = await _context.Usuarios
				.AnyAsync(u => u.Email.ToLower() == emailNormalizado);

			if (existe)
			{
				ModelState.AddModelError("Email", "Este correo ya está registrado.");
				return View(model);
			}

			var usuario = new Usuario
			{
				Nombre = nombreNormalizado,
				Email = emailNormalizado,
				PasswordHash = model.Password,
				Activo = true,
				RolId = 2
			};

			_context.Usuarios.Add(usuario);
			await _context.SaveChangesAsync();

			TempData["Success"] = "Cuenta creada correctamente.";
			return RedirectToAction("Login");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}
	}
}