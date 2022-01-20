using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Egx.EgxControl
{
    public partial class EgxWink : Control
    {
        Panel p;
        PictureBox img;
        Label lbl;
        Label line;
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                lbl.Text = value;
                base.Text = value;
            }
        }

        public override Image BackgroundImage
        {
            get
            {
                return img.Image;
            }
            set
            {
                img.Image = value;
            }
        }
        public EgxWink()
        {
            InitializeComponent();
            p = new Panel();
            p.Dock = DockStyle.Top ;
            img = new PictureBox();
            lbl = new Label();
            line = new Label();
            line.AutoSize = false;
            line.Width = this.Width;
            line.Top = this.Height;
            line.Left = 0;
            line.Height = 4;
            line.BorderStyle = BorderStyle.Fixed3D;
            img.Size = new Size(64, 64);
            img.Location = new Point(5, 5);
            lbl.Top = 72;
            lbl.AutoSize = false;
            lbl.Left = 0;
            lbl.Width = this.Width;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Text = "TextWink ;)";
            this.Controls.Add(p);
            this.BackColor = Color.White ;
            p.Controls.Add(img);
            p.Controls.Add(lbl);
            this.Controls.Add(line);
            line.Dock = DockStyle.Bottom;
            foreach (Control c in p.Controls) 
            {
                c.MouseEnter+=c_MouseEnter;
                c.MouseLeave+=c_MouseLeave;
                c.Click+=c_Click;
            }
            img.Dock = DockStyle.Top;
            lbl.Dock = DockStyle.Bottom ;
            img.SizeMode = PictureBoxSizeMode.CenterImage;
            p.MouseEnter += p_MouseEnter;
        }

        private void c_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        private void p_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            img.Location = new Point(5, 5);
            line.BackColor = Color.DodgerBlue;
        }

        private void c_MouseLeave(object sender, EventArgs e)
        {
            img.Location = new Point(8, 2);
            line.BackColor = Color.White ;
        }

        private void c_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            img.Location = new Point(5, 5);
            line.BackColor = Color.DodgerBlue;
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            line.Width = this.Width;
            line.Top = this.Height;
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
