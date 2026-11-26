namespace InventarioVentas
{
    public class Producto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal PrecioBase { get; set; }
        public int Stock { get; set; }
        public bool AplicaGarantia { get; set; }

        public Producto() { }

        public Producto(string codigo, string nombre, string categoria,
                        decimal precioBase, int stock, bool aplicaGarantia)
        {
            Codigo = codigo;
            Nombre = nombre;
            Categoria = categoria;
            PrecioBase = precioBase;
            Stock = stock;
            AplicaGarantia = aplicaGarantia;
        }

        public decimal CalcularPrecioFinal(bool incluirGarantia)
            => incluirGarantia ? PrecioBase * 1.10m : PrecioBase;

        public bool DescontarStock(int cantidad)
        {
            if (cantidad <= 0 || cantidad > Stock)
                return false;

            Stock -= cantidad;
            return true;
        }

        public override string ToString()
            => $"{Codigo} - {Nombre} - ${PrecioBase:N2} - Stock: {Stock}";
    }
}
