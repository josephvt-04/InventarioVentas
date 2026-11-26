using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace InventarioVentas
{
    public partial class FormProductos : Form
    {
        private readonly TextBox txtCodigo = new() { Name = "txtCodigo" };
        private readonly TextBox txtNombre = new() { Name = "txtNombre" };
        private readonly TextBox txtPrecio = new() { Name = "txtPrecio" };
        private readonly TextBox txtStock = new() { Name = "txtStock" };
        private readonly ComboBox cmbCategoria = new() { Name = "cmbCategoria", DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly CheckBox chkGarantia = new() { Name = "chkGarantia", Text = "Aplica garantía extendida", AutoSize = true };
        private readonly Button btnGuardar = new() { Name = "btnGuardar", Text = "Guardar" };
        private readonly Button btnLimpiar = new() { Name = "btnLimpiar", Text = "Limpiar" };
        private readonly ListBox lstProductos = new() { Name = "lstProductos" };
        private readonly Label lblTituloLista = Estilo.Seccion("Inventario actual");

        public FormProductos()
        {
            InitializeComponent();
            Estilo.Base(this);
            Text = "Gestión de inventario";
            Size = new Size(1000, 660);
            MinimumSize = new Size(900, 620);
            StartPosition = FormStartPosition.CenterParent;

            cmbCategoria.Items.AddRange(new object[] { "Oficina", "Electrónica", "Limpieza", "Herramientas", "Otros" });
            cmbCategoria.SelectedIndex = 0;

            var cuerpo = new Panel { Dock = DockStyle.Fill, Padding = new Padding(24) };
            var derecha = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20, 0, 0, 0) };
            var izquierda = Estilo.Tarjeta();
            izquierda.Dock = DockStyle.Left;
            izquierda.Width = 380;

            // ---- Tarjeta izquierda: formulario ----
            int y = 64;
            int ancho = 340;
            Estilo.Campo(izquierda, "Código", txtCodigo, ref y, ancho);
            Estilo.Campo(izquierda, "Nombre", txtNombre, ref y, ancho);
            Estilo.Campo(izquierda, "Categoría", cmbCategoria, ref y, ancho);
            Estilo.Campo(izquierda, "Precio base", txtPrecio, ref y, ancho);
            Estilo.Campo(izquierda, "Stock", txtStock, ref y, ancho);

            chkGarantia.Font = Estilo.Normal;
            chkGarantia.Location = new Point(20, y);
            izquierda.Controls.Add(chkGarantia);
            y += 46;

            Estilo.Boton(btnGuardar, true);
            Estilo.Boton(btnLimpiar, false);
            btnGuardar.SetBounds(20, y, 165, 42);
            btnLimpiar.SetBounds(195, y, 165, 42);
            izquierda.Controls.Add(btnGuardar);
            izquierda.Controls.Add(btnLimpiar);
            izquierda.Controls.Add(Estilo.Seccion("Nuevo producto"));

            // ---- Tarjeta derecha: lista ----
            var tarjetaLista = Estilo.Tarjeta();
            tarjetaLista.Dock = DockStyle.Fill;
            Estilo.Lista(lstProductos);
            lstProductos.Dock = DockStyle.Fill;
            tarjetaLista.Controls.Add(lstProductos);
            tarjetaLista.Controls.Add(lblTituloLista);
            derecha.Controls.Add(tarjetaLista);

            cuerpo.Controls.Add(derecha);
            cuerpo.Controls.Add(izquierda);
            Controls.Add(cuerpo);
            Estilo.Encabezado(this, "Inventario", "Registra y consulta los productos disponibles");

            btnGuardar.Click += btnGuardar_Click;
            btnLimpiar.Click += (s, e) => LimpiarCampos();
            AcceptButton = btnGuardar;

            ActualizarLista();
        }

        private void btnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                string codigo = txtCodigo.Text.Trim();
                string nombre = txtNombre.Text.Trim();

                if (codigo == "" || nombre == "")
                {
                    MessageBox.Show("El código y el nombre son obligatorios.", "Validación");
                    return;
                }
                if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
                {
                    MessageBox.Show("Ingrese un precio válido mayor que 0.", "Validación");
                    return;
                }
                if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
                {
                    MessageBox.Show("Ingrese un stock válido (entero mayor o igual a 0).", "Validación");
                    return;
                }
                if (Repositorio.Productos.Any(p => p.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Ya existe un producto con ese código.", "Código repetido");
                    return;
                }

                var producto = new Producto
                {
                    Codigo = codigo,
                    Nombre = nombre,
                    Categoria = cmbCategoria.Text,
                    PrecioBase = precio,
                    Stock = stock,
                    AplicaGarantia = chkGarantia.Checked
                };
                Repositorio.Productos.Add(producto);

                ActualizarLista();
                LimpiarCampos();
                MessageBox.Show("Producto guardado correctamente.", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el producto: " + ex.Message, "Error");
            }
        }

        private void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            cmbCategoria.SelectedIndex = 0;
            chkGarantia.Checked = false;
            txtCodigo.Focus();
        }

        private void ActualizarLista()
        {
            lstProductos.Items.Clear();
            foreach (var p in Repositorio.Productos)
                lstProductos.Items.Add(p.ToString());
            lblTituloLista.Text = $"Inventario actual ({Repositorio.Productos.Count})";
        }
    }
}