using Microsoft.AspNetCore.Mvc;
using ProyectoPyme.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ProyectoPyme.Controllers
{
    public class AdminController : Controller
    {
        string conexion = "server=localhost;port=3306;database=proyectopyme_db;user=root;password=1122;";

        // Lista de órdenes
        public IActionResult Ordenes()
        {
            List<Orden> listaOrdenes = new List<Orden>();

            using (MySqlConnection conn = new MySqlConnection(conexion))
            {
                conn.Open();
                string query = "SELECT * FROM Ordenes ORDER BY Fecha DESC";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaOrdenes.Add(new Orden
                        {
                            IdOrden = reader.GetInt32("IdOrden"),
                            NumeroOrden = reader.GetString("NumeroOrden"),
                            NombreCliente = reader.GetString("NombreCliente"),
                            Fecha = reader.GetDateTime("Fecha"),
                            Total = reader.GetDecimal("Total"),
                            Estado = reader.GetString("Estado")
                        });
                    }
                }
            }

            return View(listaOrdenes);
        }

        // Actualizar estado de la orden
        [HttpPost]
        public IActionResult ActualizarEstado(int idOrden, string nuevoEstado)
        {
            using (MySqlConnection conn = new MySqlConnection(conexion))
            {
                conn.Open();
                string query = "UPDATE Ordenes SET Estado = @Estado WHERE IdOrden = @IdOrden";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Estado", nuevoEstado);
                cmd.Parameters.AddWithValue("@IdOrden", idOrden);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Ordenes");
        }

        // Detalle de una orden
        public IActionResult DetalleOrden(int idOrden)
        {
            Orden orden = null;
            List<DetalleOrden> detalles = new List<DetalleOrden>();

            using (MySqlConnection conn = new MySqlConnection(conexion))
            {
                conn.Open();

                // Información de la orden
                string queryOrden = "SELECT * FROM Ordenes WHERE IdOrden = @IdOrden";
                MySqlCommand cmdOrden = new MySqlCommand(queryOrden, conn);
                cmdOrden.Parameters.AddWithValue("@IdOrden", idOrden);

                using (var reader = cmdOrden.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        orden = new Orden
                        {
                            IdOrden = reader.GetInt32("IdOrden"),
                            NumeroOrden = reader.GetString("NumeroOrden"),
                            UsuarioId = reader.GetInt32("UsuarioId"),
                            Fecha = reader.GetDateTime("Fecha"),
                            NombreCliente = reader.GetString("NombreCliente"),
                            Direccion = reader.GetString("Direccion"),
                            Telefono = reader.GetString("Telefono"),
                            MetodoPago = reader.GetString("MetodoPago"),
                            Total = reader.GetDecimal("Total"),
                            Estado = reader.GetString("Estado")
                        };
                    }
                }

                // Detalle de productos de la orden
                string queryDetalle = @"SELECT IdDetalle, IdOrden, IdProducto, NombreProducto, Cantidad, Precio, (Cantidad*Precio) AS Subtotal
                                        FROM DetalleOrden
                                        WHERE IdOrden = @IdOrden";
                MySqlCommand cmdDetalle = new MySqlCommand(queryDetalle, conn);
                cmdDetalle.Parameters.AddWithValue("@IdOrden", idOrden);

                using (var reader = cmdDetalle.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        detalles.Add(new DetalleOrden
                        {
                            IdDetalle = reader.GetInt32("IdDetalle"),
                            IdOrden = reader.GetInt32("IdOrden"),
                            IdProducto = reader.GetInt32("IdProducto"),
                            NombreProducto = reader.GetString("NombreProducto"),
                            Cantidad = reader.GetInt32("Cantidad"),
                            Precio = reader.GetDecimal("Precio"),
                            Subtotal = reader.GetDecimal("Subtotal")
                        });
                    }
                }
            }

            var modelo = new DetalleOrdenViewModel
            {
                Orden = orden,
                Detalles = detalles
            };

            return View(modelo);
        }
    }
}