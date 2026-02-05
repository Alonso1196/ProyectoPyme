using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoPyme.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

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
        // GET: Productos
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var Productos = _context.Productos.Include(p => p.Categoria);
            return View(await Productos.ToListAsync());
        }

        // ✅ Solo Admin
        // GET: Crear Producto
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaId", "Nombre");
            return View();
        }

        // ✅ Solo Admin
        // POST: Crear Producto (+ imagen)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Productos producto, IFormFile? Imagen)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
                return View(producto);
            }

            // ✅ Guardar imagen en wwwroot/images/Productos
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

        // ✅ Solo Admin
        // GET: Editar Producto
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // ✅ Solo Admin
        // POST: Editar Producto (+ opcional nueva imagen)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Productos producto, IFormFile? Imagen)
        {
            if (id != producto.ProductoId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
                return View(producto);
            }

            var productoDb = await _context.Productos.AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductoId == id);

            if (productoDb == null) return NotFound();

            producto.RutaImagen = productoDb.RutaImagen;

            if (Imagen != null && Imagen.Length > 0)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "productos");
                Directory.CreateDirectory(carpeta);

                var ext = Path.GetExtension(Imagen.FileName);
                var nombreArchivo = $"{Guid.NewGuid()}{ext}";
                var rutaFisica = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaFisica, FileMode.Create))
                {
                    await Imagen.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(productoDb.RutaImagen))
                {
                    var vieja = productoDb.RutaImagen.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
                    var rutaViejaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", vieja);

                    if (System.IO.File.Exists(rutaViejaFisica))
                        System.IO.File.Delete(rutaViejaFisica);
                }

                producto.RutaImagen = $"/images/productos/{nombreArchivo}";
            }

            try
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Productos.AnyAsync(e => e.ProductoId == producto.ProductoId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ Solo Admin
        // GET: Eliminar Producto (confirmación)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);

            if (producto == null) return NotFound();

            return View(producto);
        }

        // ✅ Solo Admin
        // POST: Confirmar eliminación
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return RedirectToAction(nameof(Index));

            if (!string.IsNullOrEmpty(producto.RutaImagen))
            {
                var rel = producto.RutaImagen.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
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
