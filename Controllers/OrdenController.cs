using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoPyme.Models;
using Stripe.Checkout;
using System.Security.Claims;

namespace ProyectoPyme.Controllers
{
    public class OrdenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public OrdenController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearOrden(Orden orden)
        {
            int usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var carritoItems = _context.Carrito
                .Include(c => c.Producto)
                .Where(c => c.UsuarioId == usuarioId)
                .ToList();

            if (!carritoItems.Any())
                return RedirectToAction("Index", "Carrito");

            decimal total = carritoItems.Sum(c => c.Producto.Precio * c.Cantidad);

            orden.NumeroOrden = "ORD-" + DateTime.Now.Ticks;
            orden.Fecha = DateTime.Now;
            orden.UsuarioId = usuarioId;
            orden.Total = total;

            if (string.IsNullOrEmpty(orden.MetodoPago))
                orden.MetodoPago = "Stripe";

            if (orden.MetodoPago == "Stripe")
            {
                orden.Estado = "Pendiente";
                _context.Ordenes.Add(orden);
                _context.SaveChanges();

                var lineItems = carritoItems.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = item.Producto.Precio * 100,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Producto.Nombre
                        }
                    },
                    Quantity = item.Cantidad
                }).ToList();


                // Construir baseUrl con puerto si corresponde
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var successUrl = $"{baseUrl}/Orden/PagoExitoso?ordenId={orden.IdOrden}";
                var cancelUrl = $"{baseUrl}/Orden/PagoCancelado?ordenId={orden.IdOrden}";

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = ["card"],
                    LineItems = lineItems,
                    Mode = "payment",
                    SuccessUrl = successUrl,
                    CancelUrl = cancelUrl
                };

                var service = new SessionService();
                var session = service.Create(options);

                return Redirect(session.Url);
            }

            // Tarjeta o Efectivo: se registra como pagado directamente
            orden.Estado = "Pagado";
            _context.Ordenes.Add(orden);
            _context.SaveChanges();

            _context.Carrito.RemoveRange(carritoItems);
            _context.SaveChanges();

            return RedirectToAction("PagoExitoso", new { ordenId = orden.IdOrden });
        }

        public IActionResult PagoExitoso(int ordenId)
        {
            var orden = _context.Ordenes.Find(ordenId);
            if (orden != null)
            {
                orden.Estado = "Pagado";
                _context.SaveChanges();

                var carritoItems = _context.Carrito
                    .Where(c => c.UsuarioId == orden.UsuarioId)
                    .ToList();
                _context.Carrito.RemoveRange(carritoItems);
                _context.SaveChanges();
            }

            return View(orden);
        }

        public IActionResult PagoCancelado(int ordenId)
        {
            var orden = _context.Ordenes.Find(ordenId);
            if (orden != null)
            {
                orden.Estado = "Cancelado";
                _context.SaveChanges();
            }

            return View(orden);
        }
    }
}