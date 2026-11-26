using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioVentas
{
    public static class Repositorio
    {
        public static List<Producto> Productos { get; set; } = new List<Producto>();
        public static List<Venta> Ventas { get; set; } = new List<Venta>();

        public static int SiguienteIdVenta()
            => Ventas.Count == 0 ? 1 : Ventas.Max(v => v.IdVenta) + 1;
    }
}

