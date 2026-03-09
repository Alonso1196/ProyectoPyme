using ProyectoPyme.Models;

public class DetalleOrden
{
    public int IdDetalle { get; set; }
    public int OrdenId { get; set; }
    public int ProductoId { get; set; }
    public string NombreProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public int IdOrden { get; internal set; }
    public int IdProducto { get; internal set; }
    public decimal Subtotal { get; internal set; }
}

public class DetalleOrdenViewModel
{
    public Orden Orden { get; set; }
    public List<DetalleOrden> Detalles { get; set; }
}