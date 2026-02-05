using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoPyme.Models;
using System.IO;

namespace ProyectoPyme.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Público: Visitante / Cliente / Admin
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Esencia);

            return View(await productos.ToListAsync());
        }

        // ✅ Solo Admin (GET)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaId", "Nombre");
            ViewBag.Esencias = new SelectList(_context.Esencias, "EsenciaId", "Nombre");
            return View();
        }

        // ✅ Solo Admin (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Productos producto, IFormFile? Imagen)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
                ViewBag.Esencias = new SelectList(_context.Esencias, "EsenciaId", "Nombre", producto.EsenciaId);
                return View(producto);
            }

            // Guardar imagen
            if (Imagen != null && Imagen.Length > 0)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Productos");
                Directory.CreateDirectory(carpeta);

                var ext = Path.GetExtension(Imagen.FileName);
                var nombreArchivo = $"{Guid.NewGuid()}{ext}";
                var rutaFisica = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaFisica, FileMode.Create))
                {
                    await Imagen.CopyToAsync(stream);
                }

                producto.RutaImagen = $"/images/Productos/{nombreArchivo}";
            }

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ✅ Solo Admin (GET)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
            ViewBag.Esencias = new SelectList(_context.Esencias, "EsenciaId", "Nombre", producto.EsenciaId);

            return View(producto);
        }

        // ✅ Solo Admin (POST) - conserva imagen si no se sube una nueva
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Productos producto, IFormFile? Imagen)
        {
            if (id != producto.ProductoId) return NotFound();

            // Traer registro real para conservar RutaImagen
            var productoDb = await _context.Productos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductoId == id);

            if (productoDb == null) return NotFound();

            // Mantener imagen anterior si no suben una nueva
            producto.RutaImagen = productoDb.RutaImagen;

            // Si ModelState falla, devolver la vista con combos y con RutaImagen intacta
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
                ViewBag.Esencias = new SelectList(_context.Esencias, "EsenciaId", "Nombre", producto.EsenciaId);
                return View(producto);
            }

            // Si subieron una nueva imagen, guardar y reemplazar
            if (Imagen != null && Imagen.Length > 0)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Productos");
                Directory.CreateDirectory(carpeta);

                var ext = Path.GetExtension(Imagen.FileName);
                var nombreArchivo = $"{Guid.NewGuid()}{ext}";
                var rutaFisica = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaFisica, FileMode.Create))
                {
                    await Imagen.CopyToAsync(stream);
                }

                // borrar imagen anterior del disco (opcional)
                if (!string.IsNullOrEmpty(productoDb.RutaImagen))
                {
                    var vieja = productoDb.RutaImagen.TrimStart('/')
                        .Replace("/", Path.DirectorySeparatorChar.ToString());

                    var rutaViejaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", vieja);

                    if (System.IO.File.Exists(rutaViejaFisica))
                        System.IO.File.Delete(rutaViejaFisica);
                }

                producto.RutaImagen = $"/images/Productos/{nombreArchivo}";
            }

            _context.Update(producto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ✅ Solo Admin (GET)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Esencia)
                .FirstOrDefaultAsync(m => m.ProductoId == id);

            if (producto == null) return NotFound();

            return View(producto);
        }

        // ✅ Solo Admin (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return RedirectToAction(nameof(Index));

            // borrar imagen física (opcional)
            if (!string.IsNullOrEmpty(producto.RutaImagen))
            {
                var rel = producto.RutaImagen.TrimStart('/')
                    .Replace("/", Path.DirectorySeparatorChar.ToString());

                var rutaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", rel);

                if (System.IO.File.Exists(rutaFisica))
                    System.IO.File.Delete(rutaFisica);
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
