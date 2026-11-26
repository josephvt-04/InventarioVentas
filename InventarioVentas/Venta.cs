namespace InventarioVentas
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public Producto ProductoVendido { get; set; } = new();
        public int Cantidad { get; set; }
        public decimal MontoTotal { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        public Venta() { }

        public Venta(int idVenta, string cliente, Producto productoVendido, int cantidad, bool incluirGarantia)
        {
            IdVenta = idVenta;
            Cliente = cliente;
            ProductoVendido = productoVendido;
            Cantidad = cantidad;
            MontoTotal = productoVendido.CalcularPrecioFinal(incluirGarantia) * cantidad;
            Fecha = DateTime.Now;
        }

        public override string ToString()
            => $"Venta ID: {IdVenta} - Cliente: {Cliente} - Producto: {ProductoVendido.Nombre} - Cantidad: {Cantidad} - Monto Total: ${MontoTotal:N2} - Fecha: {Fecha}";
    }
}

