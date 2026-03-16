using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoPyme.Controllers
{
    public class ErrorController : Controller
    {
        // GET /Error
        [Route("Error")]
        public IActionResult Index()
        {
            // Obtener la excepcion capturada por el middleware (equivale a Server.GetLastError())
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.Codigo = 500;
            ViewBag.Titulo = "Error interno del servidor";
            ViewBag.Mensaje = exceptionFeature?.Error?.Message
                ?? "Ocurrio un error inesperado en el sistema.";
            ViewBag.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            Response.StatusCode = 500;
            return View("Index");
        }

        // GET /Error/404, /Error/500, etc.
        [Route("Error/{codigo:int}")]
        public IActionResult HttpStatusCodeHandler(int codigo)
        {
            ViewBag.Codigo = codigo;
            ViewBag.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            switch (codigo)
            {
                case 404:
                    ViewBag.Titulo = "Pagina no encontrada";
                    ViewBag.Mensaje = "La ruta que buscas no existe o fue movida.";
                    break;
                case 403:
                    ViewBag.Titulo = "Acceso denegado";
                    ViewBag.Mensaje = "No tienes permisos para acceder a este recurso.";
                    break;
                case 500:
                    ViewBag.Titulo = "Error interno del servidor";
                    ViewBag.Mensaje = "Ocurrio un error inesperado en el sistema.";
                    break;
                default:
                    ViewBag.Titulo = "Error";
                    ViewBag.Mensaje = "Ocurrio un error al procesar tu solicitud.";
                    break;
            }

            Response.StatusCode = codigo;
            return View("Index");
        }
    }
}
