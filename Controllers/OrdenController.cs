using Microsoft.AspNetCore.Mvc;
using ProyectoPyme.Models;
using MySql.Data.MySqlClient;
using System;

namespace ProyectoPyme.Controllers
{
    public class OrdenController : Controller
    {

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

            string conexion = "server=localhost;port=3306;database=proyectopyme_db;user=root;password=1122;";

            using (MySqlConnection conn = new MySqlConnection(conexion))
            {
                conn.Open();

                string query = @"INSERT INTO Ordenes
                (NumeroOrden, UsuarioId, Fecha, NombreCliente, Direccion, Telefono, MetodoPago, Total, Estado)
                VALUES
                (@NumeroOrden, @UsuarioId, @Fecha, @NombreCliente, @Direccion, @Telefono, @MetodoPago, @Total, @Estado)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@NumeroOrden", orden.NumeroOrden);
                cmd.Parameters.AddWithValue("@UsuarioId", orden.UsuarioId);
                cmd.Parameters.AddWithValue("@Fecha", orden.Fecha);
                cmd.Parameters.AddWithValue("@NombreCliente", orden.NombreCliente);
                cmd.Parameters.AddWithValue("@Direccion", orden.Direccion);
                cmd.Parameters.AddWithValue("@Telefono", orden.Telefono);
                cmd.Parameters.AddWithValue("@MetodoPago", orden.MetodoPago);
                cmd.Parameters.AddWithValue("@Total", 0);
                cmd.Parameters.AddWithValue("@Estado", orden.Estado);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index", "Home");
        }

    }
}