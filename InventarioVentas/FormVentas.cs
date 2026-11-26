namespace InventarioVentas
{
    public partial class FormVentas : Form
    {
        private readonly ComboBox cmbProductos = new() { Name = "cmbProductos", DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly TextBox txtCliente = new() { Name = "txtCliente" };
        private readonly TextBox txtCantidad = new() { Name = "txtCantidad" };
        private readonly CheckBox chkGarantia = new() { Name = "chkGarantia", Text = "Incluir garantía extendida (+10%)", AutoSize = true };
        private readonly Button btnProcesarVenta = new() { Name = "btnProcesarVenta", Text = "Procesar venta" };
        private readonly ListBox lstVentas = new() { Name = "lstVentas" };
        private readonly Button btnCalcularEstadisticas = new() { Name = "btnCalcularEstadisticas", Text = "Calcular estadísticas" };

        private readonly Label lblDetalle = new() { AutoSize = false, ForeColor = Estilo.TextoSuave };
        private readonly Label lblResumen = new() { AutoSize = false };
        private readonly Label lblTotalRecaudado = new();
        private readonly Label lblMasVendido = new();
        private readonly Label lblStockBajo = new();

        public FormVentas()
        {
           
            Estilo.Base(this);
            Text = "Punto de venta";
            Size = new Size(1060, 700);
            MinimumSize = new Size(940, 640);
            StartPosition = FormStartPosition.CenterParent;

            var cuerpo = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24) };
            var derecha = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20, 0, 0, 0) };
            var izquierda = Estilo.Tarjeta();
            izquierda.Dock = DockStyle.Left;
            izquierda.Width = 370;

            ConstruirTransaccion(izquierda);
            ConstruirDerecha(derecha);

            cuerpo.Controls.Add(derecha);
            cuerpo.Controls.Add(izquierda);
            Controls.Add(cuerpo);
            Estilo.Encabezado(this, "Punto de venta", "Procesa ventas, descuenta stock y consulta estadísticas");

            cmbProductos.SelectedIndexChanged += (s, e) => ActualizarGarantia();
            txtCantidad.TextChanged += (s, e) => ActualizarResumen();
            chkGarantia.CheckedChanged += (s, e) => ActualizarResumen();
            btnProcesarVenta.Click += btnProcesarVenta_Click;
            btnCalcularEstadisticas.Click += btnCalcularEstadisticas_Click;
            AcceptButton = btnProcesarVenta;

            CargarProductos();
            CargarVentas();
        }

        private void ConstruirTransaccion(Panel card)
        {
            int y = 64;
            int ancho = 330;
            Estilo.Campo(card, "Producto", cmbProductos, ref y, ancho);
            Estilo.Campo(card, "Cliente", txtCliente, ref y, ancho);
            Estilo.Campo(card, "Cantidad", txtCantidad, ref y, 140);

            lblDetalle.Font = Estilo.Pequena;
            lblDetalle.SetBounds(20, y - 10, ancho, 24);
            card.Controls.Add(lblDetalle);

            chkGarantia.Font = Estilo.Normal;
            chkGarantia.Location = new Point(20, y + 24);
            card.Controls.Add(chkGarantia);

            lblResumen.Font = new Font("Segoe UI Semibold", 13f);
            lblResumen.ForeColor = Estilo.Primario;
            lblResumen.SetBounds(20, y + 64, ancho, 34);
            card.Controls.Add(lblResumen);

            Estilo.Boton(btnProcesarVenta, true);
            btnProcesarVenta.SetBounds(20, y + 112, ancho, 46);
            card.Controls.Add(btnProcesarVenta);

            card.Controls.Add(Estilo.Seccion("Nueva venta"));
        }

        private void ConstruirDerecha(Panel derecha)
        {
            // Historial
            var historial = Estilo.Tarjeta();
            historial.Dock = DockStyle.Fill;
            Estilo.Lista(lstVentas);
            lstVentas.Dock = DockStyle.Fill;
            historial.Controls.Add(lstVentas);
            historial.Controls.Add(Estilo.Seccion("Historial de ventas"));

            // Estadísticas
            var contenedor = new Panel { Dock = DockStyle.Bottom, Height = 176, Padding = new Padding(0, 16, 0, 0) };
            var stats = Estilo.Tarjeta();
            stats.Dock = DockStyle.Fill;

            var tabla = new TableLayoutPanel { Dock = DockStyle.Bottom, Height = 76, ColumnCount = 3, RowCount = 1 };
            for (int i = 0; i < 3; i++) tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            tabla.Controls.Add(Mini("Total recaudado", lblTotalRecaudado, Estilo.Exito, 10), 0, 0);
            tabla.Controls.Add(Mini("Producto más vendido", lblMasVendido, Estilo.Primario, 10), 1, 0);
            tabla.Controls.Add(Mini("Stock bajo (menos de 5)", lblStockBajo, Estilo.Alerta, 0), 2, 0);

            Estilo.Boton(btnCalcularEstadisticas, true);
            btnCalcularEstadisticas.SetBounds(20, 16, 220, 38);

            stats.Controls.Add(tabla);
            stats.Controls.Add(btnCalcularEstadisticas);
            contenedor.Controls.Add(stats);

            derecha.Controls.Add(historial);
            derecha.Controls.Add(contenedor);
        }

        private static Panel Mini(string titulo, Label valor, Color color, int margenDerecho)
        {
            valor.Text = "—";
            valor.Dock = DockStyle.Fill;
            valor.TextAlign = ContentAlignment.MiddleLeft;
            valor.Font = new Font("Segoe UI Semibold", 13f);
            valor.ForeColor = color;

            var t = new Label
            {
                Text = titulo, Dock = DockStyle.Top, Height = 22,
                ForeColor = Estilo.TextoSuave, Font = Estilo.Pequena
            };
            var p = new Panel
            {
                Dock = DockStyle.Fill, BackColor = Estilo.Fondo,
                Padding = new Padding(14, 8, 8, 8), Margin = new Padding(0, 0, margenDerecho, 0)
            };
            p.Controls.Add(valor);
            p.Controls.Add(t);
            return p;
        }

        private void CargarProductos(int seleccion = -1)
        {
            cmbProductos.Items.Clear();
            foreach (var p in Repositorio.Productos)
                cmbProductos.Items.Add(p);

            if (cmbProductos.Items.Count > 0)
                cmbProductos.SelectedIndex = seleccion >= 0 && seleccion < cmbProductos.Items.Count ? seleccion : 0;

            ActualizarGarantia();
        }

        private void ActualizarGarantia()
        {
            bool aplica = cmbProductos.SelectedItem is Producto p && p.AplicaGarantia;
            chkGarantia.Enabled = aplica;
            if (!aplica) chkGarantia.Checked = false;
            ActualizarResumen();
        }

        private void ActualizarResumen()
        {
            if (cmbProductos.SelectedItem is Producto p)
            {
                lblDetalle.Text = $"Precio unitario: ${p.PrecioBase:N2}   ·   Stock: {p.Stock}";
                lblResumen.Text = int.TryParse(txtCantidad.Text, out int c) && c > 0
                    ? $"Total: ${p.CalcularPrecioFinal(chkGarantia.Checked) * c:N2}"
                    : "Total: —";
            }
            else
            {
                lblDetalle.Text = "No hay productos registrados.";
                lblResumen.Text = "Total: —";
            }
        }

        private static string FormatoVenta(Venta v)
            => $"#{v.IdVenta}   {v.Fecha:dd/MM HH:mm}   {v.Cliente}   ·   {v.ProductoVendido.Nombre} x{v.Cantidad}   ·   ${v.MontoTotal:N2}";

        private void CargarVentas()
        {
            lstVentas.Items.Clear();
            foreach (var v in Repositorio.Ventas)
                lstVentas.Items.Add(FormatoVenta(v));
            if (lstVentas.Items.Count > 0)
                lstVentas.TopIndex = lstVentas.Items.Count - 1;
        }

        private void btnProcesarVenta_Click(object? sender, EventArgs e)
        {
            try
            {
                if (cmbProductos.SelectedItem is not Producto producto)
                {
                    MessageBox.Show("Seleccione un producto.", "Validación");
                    return;
                }

                string cliente = txtCliente.Text.Trim();
                if (cliente == "")
                {
                    MessageBox.Show("Ingrese el nombre del cliente.", "Validación");
                    return;
                }

                if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
                {
                    MessageBox.Show("Ingrese una cantidad válida (entero mayor que 0).", "Validación");
                    return;
                }

                if (cantidad > producto.Stock)
                {
                    MessageBox.Show($"Stock insuficiente. Disponible: {producto.Stock}.", "Validación");
                    return;
                }

                decimal monto = producto.CalcularPrecioFinal(chkGarantia.Checked) * cantidad;
                var venta = new Venta
                {
                    IdVenta = Repositorio.SiguienteIdVenta(),
                    Cliente = cliente,
                    ProductoVendido = producto,
                    Cantidad = cantidad,
                    MontoTotal = monto,
                    Fecha = DateTime.Now
                };

                producto.Stock -= cantidad;
                Repositorio.Ventas.Add(venta);

                CargarVentas();
                CargarProductos(cmbProductos.SelectedIndex);
                txtCantidad.Clear();
                txtCliente.Clear();

                MessageBox.Show($"Venta procesada. Total: ${monto:N2}", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar la venta: " + ex.Message, "Error");
            }
        }

        private void btnCalcularEstadisticas_Click(object? sender, EventArgs e)
        {
            try
            {
                decimal total = Repositorio.Ventas.Sum(v => v.MontoTotal);

                var masVendido = Repositorio.Ventas
                    .GroupBy(v => v.ProductoVendido.Codigo)
                    .Select(g => new { Nombre = g.First().ProductoVendido.Nombre, Unidades = g.Sum(v => v.Cantidad) })
                    .OrderByDescending(x => x.Unidades)
                    .FirstOrDefault();

                int stockBajo = Repositorio.Productos.Count(p => p.Stock < 5);

                lblTotalRecaudado.Text = $"${total:N2}";
                lblMasVendido.Text = masVendido == null ? "N/A" : $"{masVendido.Nombre} ({masVendido.Unidades})";
                lblStockBajo.Text = stockBajo.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al calcular estadísticas: " + ex.Message, "Error");
            }
        }
    }
}