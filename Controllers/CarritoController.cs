using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoPyme.Models;
using System.Security.Claims;

public class CarritoController : Controller
{
    private readonly ApplicationDbContext _context;

    public CarritoController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Agregar(int id)
    {
        int usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var item = _context.Carrito
            .FirstOrDefault(c => c.UsuarioId == usuarioId && c.IdProducto == id);

        if (item != null)
        {
            item.Cantidad += 1;
        }
        else
        {
            Carrito nuevo = new Carrito
            {
                UsuarioId = usuarioId,
                IdProducto = id,
                Cantidad = 1
            };

            _context.Carrito.Add(nuevo);
        }

        _context.SaveChanges();

        return RedirectToAction("Index");
    }
    public IActionResult Index()
    {
        int usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var carrito = _context.Carrito
            .Include(c => c.Producto)
            .Where(c => c.UsuarioId == usuarioId)
            .ToList();

        return View(carrito);
    }

    // Aumentar Cantidad
    public IActionResult Sumar(int id)
    {
        var item = _context.Carrito.Find(id);

        if (item != null)
        {
            item.Cantidad++;
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
    // Adisminuir cantidad
    public IActionResult Restar(int id)
    {
        var item = _context.Carrito.Find(id);

        if (item != null)
        {
            if (item.Cantidad > 1)
            {
                item.Cantidad--;
            }

            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
    // eliminar producto
    public IActionResult Eliminar(int id)
    {
        var item = _context.Carrito.Find(id);

        if (item != null)
        {
            _context.Carrito.Remove(item);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

}