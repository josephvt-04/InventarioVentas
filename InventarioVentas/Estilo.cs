using System;
using System.Collections.Generic;
using System.Text;

namespace InventarioVentas
{
    /// <summary>Colores, fuentes y ayudas de diseño compartidas por todos los formularios.</summary>
    public static class Estilo
    {
        public static readonly Color Primario = Color.FromArgb(37, 99, 235);
        public static readonly Color PrimarioOscuro = Color.FromArgb(29, 78, 216);
        public static readonly Color EncabezadoColor = Color.FromArgb(15, 23, 42);
        public static readonly Color Fondo = Color.FromArgb(241, 245, 249);
        public static readonly Color Texto = Color.FromArgb(30, 41, 59);
        public static readonly Color TextoSuave = Color.FromArgb(100, 116, 139);
        public static readonly Color Exito = Color.FromArgb(22, 163, 74);
        public static readonly Color Alerta = Color.FromArgb(220, 38, 38);
        public static readonly Color Neutro = Color.FromArgb(226, 232, 240);

        public static readonly Font Normal = new("Segoe UI", 10f);
        public static readonly Font Negrita = new("Segoe UI Semibold", 10f);
        public static readonly Font Pequena = new("Segoe UI", 9f);

        /// <summary>Llamar justo después de InitializeComponent().</summary>
        public static void Base(Form f)
        {
            f.Font = Normal;
            f.BackColor = Fondo;
            f.ForeColor = Texto;
        }

        /// <summary>Llamar al FINAL del constructor (se agrega de último para que dock funcione bien).</summary>
        public static void Encabezado(Form f, string titulo, string subtitulo)
        {
            var enc = new Panel { Dock = DockStyle.Top, Height = 76, BackColor = EncabezadoColor };
            enc.Controls.Add(new Label
            {
                Text = titulo,
                ForeColor = Color.White,
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 18f),
                Location = new Point(24, 8)
            });
            enc.Controls.Add(new Label
            {
                Text = subtitulo,
                ForeColor = Color.FromArgb(148, 163, 184),
                AutoSize = true,
                Font = Pequena,
                Location = new Point(27, 46)
            });
            f.Controls.Add(enc);
        }

        public static Panel Tarjeta() => new Panel { BackColor = Color.White, Padding = new Padding(20) };

        public static Label Seccion(string texto) => new Label
        {
            Text = texto,
            Dock = DockStyle.Top,
            Height = 36,
            AutoSize = false,
            Font = new Font("Segoe UI Semibold", 13f),
            ForeColor = Texto
        };

        /// <summary>Etiqueta pequeña arriba + control debajo. Avanza la posición Y.</summary>
        public static void Campo(Control padre, string etiqueta, Control c, ref int y, int ancho)
        {
            padre.Controls.Add(new Label
            {
                Text = etiqueta,
                AutoSize = true,
                ForeColor = TextoSuave,
                Font = Pequena,
                Location = new Point(20, y)
            });
            c.Font = Normal;
            c.SetBounds(20, y + 22, ancho, 30);
            padre.Controls.Add(c);
            y += 66;
        }

        public static void Boton(Button b, bool primario = true)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.Font = Negrita;
            b.Cursor = Cursors.Hand;
            b.Height = 40;
            b.FlatAppearance.BorderSize = primario ? 0 : 1;
            b.FlatAppearance.BorderColor = Neutro;
            b.BackColor = primario ? Primario : Color.White;
            b.ForeColor = primario ? Color.White : Texto;
            b.FlatAppearance.MouseOverBackColor = primario ? PrimarioOscuro : Neutro;
        }

        public static void Lista(ListBox lb)
        {
            lb.BorderStyle = BorderStyle.None;
            lb.BackColor = Color.White;
            lb.Font = Normal;
            lb.DrawMode = DrawMode.OwnerDrawFixed;
            lb.ItemHeight = 34;
            lb.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;
                bool sel = (e.State & DrawItemState.Selected) != 0;
                Color fondo = sel ? Color.FromArgb(219, 234, 254)
                            : e.Index % 2 == 0 ? Color.White : Color.FromArgb(248, 250, 252);
                using var brocha = new SolidBrush(fondo);
                e.Graphics.FillRectangle(brocha, e.Bounds);
                string texto = lb.Items[e.Index]?.ToString() ?? "";
                var area = new Rectangle(e.Bounds.X + 12, e.Bounds.Y, e.Bounds.Width - 12, e.Bounds.Height);
                TextRenderer.DrawText(e.Graphics, texto, lb.Font, area, Texto,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            };
        }
    }
}

