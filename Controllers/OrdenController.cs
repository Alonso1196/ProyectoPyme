using Microsoft.AspNetCore.Mvc;
using ProyectoPyme.Models;

namespace ProyectoPyme.Controllers
{
    public class OrdenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdenController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearOrden(Orden orden)
        {
            orden.NumeroOrden = "ORD-" + DateTime.Now.Ticks;
            orden.Fecha = DateTime.Now;
            orden.UsuarioId = 1;
            orden.Estado = "Pendiente";
            orden.Total = 0;

            _context.Ordenes.Add(orden);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}