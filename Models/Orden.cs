using System;

namespace ProyectoPyme.Models
{
    public class Orden
    {
        public int IdOrden { get; set; }
        public string NumeroOrden { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreCliente { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string MetodoPago { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
    }
}