namespace InventarioVentas
{
    public partial class FormPrincipal : Form
    {
        private readonly Button btnMasOpciones = new() { Name = "btnMasOpciones", Text = "Más Opciones" };
        private readonly Button btnInventario = new() { Name = "btnInventario", Text = "Inventario" };
        private readonly Button btnVentas = new() { Name = "btnVentas", Text = "Ventas" };
        private readonly Button btnAcerca = new() { Name = "btnAcerca", Text = "Acerca de" };

        private readonly Panel hero = new() { Dock = DockStyle.Fill, BackColor = Estilo.Fondo };
        private readonly TextBox txtTitulo = new() { Name = "txtTitulo" };
        private readonly Panel linea = new() { Size = new Size(90, 5), BackColor = Estilo.Primario };
        private readonly Label lblSub = new();

        public FormPrincipal()
        {
            InitializeComponent();
            Estilo.Base(this);
            Text = "Inventario y Ventas";
            Size = new Size(1100, 695);
            MinimumSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;

            ConstruirCentro();
            var lateral = ConstruirLateral();

            Controls.Add(hero);
            Controls.Add(lateral);

            hero.Resize += (s, e) => CentrarCentro();
            CentrarCentro();
        }

        private void ConstruirCentro()
        {
            txtTitulo.Text = "Inventario y Ventas";
            txtTitulo.ReadOnly = true;
            txtTitulo.TabStop = false;
            txtTitulo.BorderStyle = BorderStyle.None;
            txtTitulo.BackColor = Estilo.Fondo;
            txtTitulo.ForeColor = Estilo.EncabezadoColor;
            txtTitulo.Font = new Font("Segoe UI Semibold", 38f);
            txtTitulo.TextAlign = HorizontalAlignment.Center;
            txtTitulo.Width = 680;
            txtTitulo.Cursor = Cursors.Default;

            lblSub.Text = "Gestiona tu catálogo, procesa ventas y consulta estadísticas";
            lblSub.AutoSize = false;
            lblSub.Size = new Size(680, 30);
            lblSub.TextAlign = ContentAlignment.MiddleCenter;
            lblSub.ForeColor = Estilo.TextoSuave;
            lblSub.Font = new Font("Segoe UI", 12f);

            hero.Controls.Add(txtTitulo);
            hero.Controls.Add(linea);
            hero.Controls.Add(lblSub);
        }

        private void CentrarCentro()
        {
            int cx = hero.ClientSize.Width / 2;
            int cy = hero.ClientSize.Height / 2 - 70;
            txtTitulo.Location = new Point(cx - txtTitulo.Width / 2, cy);
            linea.Location = new Point(cx - linea.Width / 2, txtTitulo.Bottom + 14);
            lblSub.Location = new Point(cx - lblSub.Width / 2, linea.Bottom + 18);
        }

        private Panel ConstruirLateral()
        {
            var lateral = new Panel { Dock = DockStyle.Left, Width = 285, BackColor = Estilo.EncabezadoColor };

            lateral.Controls.Add(new Label
            {
                Text = "InventarioVentas",
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 15f),
                Location = new Point(24, 28)
            });
            lateral.Controls.Add(new Label
            {
                Text = "Sistema de gestión",
                AutoSize = true,
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = Estilo.Pequena,
                Location = new Point(26, 60)
            });

            EstiloBotonLateral(btnMasOpciones, 130);
            EstiloBotonLateral(btnInventario, 186);
            EstiloBotonLateral(btnVentas, 242);
            EstiloBotonLateral(btnAcerca, 298);
            lateral.Controls.AddRange(new Control[] { btnMasOpciones, btnInventario, btnVentas, btnAcerca });

            var menuMasOpciones = new ContextMenuStrip();
            menuMasOpciones.Items.Add("Limpiar inventario", null, (s, e) => LimpiarInventario());
            menuMasOpciones.Items.Add("Salir", null, (s, e) => Close());
            btnMasOpciones.Click += (s, e) => menuMasOpciones.Show(btnMasOpciones, new Point(btnMasOpciones.Width, 0));

            btnInventario.Click += (s, e) => AbrirInventario();
            btnVentas.Click += (s, e) => AbrirVentas();
            btnAcerca.Click += (s, e) => MostrarAcercaDe();

            return lateral;
        }

        private static void EstiloBotonLateral(Button b, int y)
        {
            b.SetBounds(0, y, 240, 52);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
            b.FlatAppearance.MouseDownBackColor = Estilo.Primario;
            b.BackColor = Estilo.EncabezadoColor;
            b.ForeColor = Color.White;
            b.Font = new Font("Segoe UI", 11f);
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Padding = new Padding(26, 0, 0, 0);
            b.Cursor = Cursors.Hand;
        }

        private void LimpiarInventario()
        {
            if (Repositorio.Productos.Count == 0)
            {
                MessageBox.Show("El inventario ya está vacío.", "Limpiar inventario",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var respuesta = MessageBox.Show(
                "¿Desea eliminar todos los productos del inventario? Esta acción no se puede deshacer.",
                "Confirmar limpieza", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (respuesta != DialogResult.Yes)
                return;

            Repositorio.Productos.Clear();
            MessageBox.Show("El inventario se limpió correctamente.", "Limpiar inventario",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AbrirInventario()
        {
            using var f = new FormProductos();
            f.ShowDialog(this);
        }

        private void AbrirVentas()
        {
            using var f = new FormVentas();
            f.ShowDialog(this);
        }

        private void MostrarAcercaDe()
        {
            MessageBox.Show(
                "Inventario y Ventas\n\nEstudiante: [TU NOMBRE]\nMatrícula: 2026-0193",
                "Acerca de", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
