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
    public partial class EgxIcon : Control
    {
        Panel panel;
        Timer t;
        public Image IconImage { get; set; }
        Panel labeledPanel;
        PictureBox pic = new PictureBox();
        Label lbl1;
        Label lbl2;
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                lbl1.Text = value;
                lbl2.Text = value;
                base.Text = value;
            }
        }
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                panel.BackColor = value;
                base.BackColor = value;
            }
        }
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                pic.Image = value;
               // base.BackgroundImage = value;
            }
        }
        public override System.Drawing.Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                lbl1.ForeColor = value;
                base.ForeColor = value;
            }
        }
        public EgxIcon()
        {
            //this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            InitializeComponent();
            t = new Timer();
            t.Interval = 1;
            t.Tick+=t_Tick;
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            panel = new Panel();
            panel.Dock = DockStyle.Fill;
            pic.Location = new Point(5, 5);
            pic.Size = new System.Drawing.Size(32, 32);
            panel.Controls.Add(pic);
            lbl1 = new Label();
            lbl2 = new Label();

            this.Text = "EgxIcon";
            lbl1.Location = new Point(37, 7);
            lbl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            panel.Controls.Add(lbl1);
            this.Controls.Add(panel);
            panel.Cursor = Cursors.Hand;
            lbl2.AutoSize = false;
            lbl2.TextAlign = ContentAlignment.MiddleCenter;
            lbl2.Location = new Point(0, 0);
            lbl2.Width = 0;
            lbl2.BackColor = Color.Black;
            lbl2.ForeColor = Color.White;
            lbl2.Height = this.Height;
            lbl2.MouseLeave+=lbl2_MouseLeave;
            panel.MouseEnter+=panel_MouseEnter;
            this.Controls.Add(lbl2);
        }

        private void lbl2_MouseLeave(object sender, EventArgs e)
        {
            lbl2.Width = 0;
            panel.Show();
            t.Stop();
        }


     


        private void panel_MouseEnter(object sender, EventArgs e)
        {
            panel.Hide();
            t.Start();
        }

        private void t_Tick(object sender, EventArgs e)
        {
            if (lbl2.Width >= panel.Width)
            {
                t.Stop();
            }
            else { lbl2.Width += 10; }
        }
 
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
 
            //lbl2.Width = 0;
            //t.Stop();
            
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            lbl2.Height = this.Height;
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
