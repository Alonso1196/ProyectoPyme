using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoPyme.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoPyme.Controllers
{
	public class ProductosController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ProductosController(ApplicationDbContext context)
		{
			_context = context;
		}

		// Público: Visitante / Cliente / Admin
		[AllowAnonymous]
		public async Task<IActionResult> Index(string? q)
		{
			if (Request.Path.StartsWithSegments("/Admin") && !User.IsInRole("Admin"))
				return Forbid();

			var productos = _context.Productos
				.Include(p => p.Categoria)
				.Include(p => p.Esencia)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(q))
			{
				var term = q.Trim().ToLower();
				productos = productos.Where(p =>
					p.Nombre.ToLower().Contains(term) ||
					(p.Categoria != null && p.Categoria.Nombre.ToLower().Contains(term)) ||
					(p.Esencia != null && p.Esencia.Nombre.ToLower().Contains(term)));
			}

			ViewBag.SearchQuery = q;
			return View(await productos.ToListAsync());
		}

		// Filtrar productos por categoría (público)
		[AllowAnonymous]
		public async Task<IActionResult> Categoria(int id)
		{
			var productos = _context.Productos
				.Include(p => p.Categoria)
				.Include(p => p.Esencia)
				.Where(p => p.CategoriaId == id);

			return View("Index", await productos.ToListAsync());
		}

		// Solo Admin (GET)
		[Authorize(Roles = "Admin")]
		public IActionResult Create()
		{
			CargarCombos();
			return View();
		}

		// Solo Admin (POST)
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create(Productos producto, IFormFile? Imagen)
		{
			ValidarProducto(producto, Imagen);

			if (!ModelState.IsValid)
			{
				CargarCombos(producto.CategoriaId, producto.EsenciaId);
				return View(producto);
			}

			if (Imagen != null && Imagen.Length > 0)
			{
				producto.RutaImagen = await GuardarImagenAsync(Imagen);
			}

			_context.Productos.Add(producto);
			await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { success = true });
		}

		// Solo Admin (GET)
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
				return NotFound();

			var producto = await _context.Productos.FindAsync(id);
			if (producto == null)
				return NotFound();

			CargarCombos(producto.CategoriaId, producto.EsenciaId);
			return View(producto);
		}

		// Solo Admin (POST)
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int id, Productos producto, IFormFile? Imagen)
		{
			if (id != producto.ProductoId)
				return NotFound();

			var productoDb = await _context.Productos
				.AsNoTracking()
				.FirstOrDefaultAsync(p => p.ProductoId == id);

			if (productoDb == null)
				return NotFound();

			// Conservar imagen anterior por defecto
			producto.RutaImagen = productoDb.RutaImagen;

			ValidarProducto(producto, Imagen);

			if (!ModelState.IsValid)
			{
				CargarCombos(producto.CategoriaId, producto.EsenciaId);
				return View(producto);
			}

			if (Imagen != null && Imagen.Length > 0)
			{
				// Borrar imagen anterior si existía
				if (!string.IsNullOrWhiteSpace(productoDb.RutaImagen))
				{
					var rutaViejaRelativa = productoDb.RutaImagen.TrimStart('/')
						.Replace("/", Path.DirectorySeparatorChar.ToString());

                    var rutaViejaFisica = Path.Combine(
						Directory.GetCurrentDirectory(),
						"wwwroot",
						rutaViejaRelativa
					);

					if (System.IO.File.Exists(rutaViejaFisica))
					{
						System.IO.File.Delete(rutaViejaFisica);
					}
				}

				producto.RutaImagen = await GuardarImagenAsync(Imagen);
			}

			_context.Update(producto);
			await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { success = true });
		}

		// Solo Admin (GET)
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
				return NotFound();

			var producto = await _context.Productos
				.Include(p => p.Categoria)
				.Include(p => p.Esencia)
				.FirstOrDefaultAsync(m => m.ProductoId == id);

			if (producto == null)
				return NotFound();

			return View(producto);
		}

		// Solo Admin (POST)
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var producto = await _context.Productos.FindAsync(id);
			if (producto == null)
				return RedirectToAction(nameof(Index));

			if (!string.IsNullOrWhiteSpace(producto.RutaImagen))
			{
				var rutaRelativa = producto.RutaImagen.TrimStart('/')
					.Replace("/", Path.DirectorySeparatorChar.ToString());

                var rutaFisica = Path.Combine(
					Directory.GetCurrentDirectory(),
					"wwwroot",
					rutaRelativa
				);

				if (System.IO.File.Exists(rutaFisica))
				{
					System.IO.File.Delete(rutaFisica);
				}
			}

			_context.Productos.Remove(producto);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index), new { success = true });
		}

		// ─── AJAX: Búsqueda en vivo (sugerencias) ───
		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> BuscarAjax(string q)
		{
			if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
				return Json(new List<object>());

			var term = q.Trim().ToLower();

			var resultados = await _context.Productos
				.Include(p => p.Categoria)
				.Include(p => p.Esencia)
				.Where(p =>
					p.Nombre.ToLower().Contains(term) ||
					(p.Categoria != null && p.Categoria.Nombre.ToLower().Contains(term)) ||
					(p.Esencia != null && p.Esencia.Nombre.ToLower().Contains(term)))
				.Take(6)
				.Select(p => new
				{
					p.ProductoId,
					p.Nombre,
					Categoria = p.Categoria != null ? p.Categoria.Nombre : "",
					Esencia   = p.Esencia != null ? p.Esencia.Nombre : "",
					Precio    = p.Precio.ToString("N2"),
					p.Disponibilidad,
					Imagen = p.RutaImagen ?? ""
				})
				.ToListAsync();

			return Json(resultados);
		}

		private void CargarCombos(int? categoriaId = null, int? esenciaId = null)
		{
			ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaId", "Nombre", categoriaId);
			ViewBag.Esencias = new SelectList(_context.Esencias, "EsenciaId", "Nombre", esenciaId);
		}

		private void ValidarProducto(Productos producto, IFormFile? imagen)
		{
			if (producto.Precio <= 0)
			{
				ModelState.AddModelError("Precio", "El precio debe ser mayor a 0.");
			}

			if (producto.Stock < 0)
			{
				ModelState.AddModelError("Stock", "El stock no puede ser negativo.");
			}

			if (producto.CategoriaId <= 0)
			{
				ModelState.AddModelError("CategoriaId", "Debe seleccionar una categoría válida.");
			}

			if (producto.EsenciaId <= 0)
			{
				ModelState.AddModelError("EsenciaId", "Debe seleccionar una esencia válida.");
			}

			if (imagen != null && imagen.Length > 0)
			{
				var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
				var extension = Path.GetExtension(imagen.FileName).ToLowerInvariant();

				if (!extensionesPermitidas.Contains(extension))
				{
					ModelState.AddModelError("RutaImagen", "Solo se permiten imágenes JPG, JPEG, PNG o WEBP.");
				}

				if (imagen.Length > 2 * 1024 * 1024)
				{
					ModelState.AddModelError("RutaImagen", "La imagen no debe superar los 2 MB.");
				}
			}
		}

		private async Task<string> GuardarImagenAsync(IFormFile imagen)
		{
            var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Productos");
			Directory.CreateDirectory(carpeta);

			var extension = Path.GetExtension(imagen.FileName).ToLowerInvariant();
			var nombreArchivo = $"{Guid.NewGuid()}{extension}";
			var rutaFisica = Path.Combine(carpeta, nombreArchivo);

			using (var stream = new FileStream(rutaFisica, FileMode.Create))
			{
				await imagen.CopyToAsync(stream);
			}

            return $"/images/Productos/{nombreArchivo}";
		}
	}
}