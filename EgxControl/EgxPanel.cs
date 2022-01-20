using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Egx.EgxControl
{
    public partial class EgxPanel : Panel
    {
        public Panel pHeader;
        public Panel pFooter;
        public Label lHeader;
        public ContentAlignment HeaderTextAlign
        {
            get { return lHeader.TextAlign; }
            set { lHeader.TextAlign = value; }
        }
        public string HeaderText
        {
            get { return lHeader.Text; }
            set { lHeader.Text = value; }
        }
        [DefaultValue(20)]
        public int HeaderHeight { get { return pHeader.Height; } set { pHeader.Height = value; } }
        public Font HeaderFont { get { return lHeader.Font; } set { lHeader.Font = value; } }
        public Color HeaderForeColor { get { return lHeader.ForeColor; } set { lHeader.ForeColor = value; } }
        [DefaultValue(typeof(Color), "0xFFFFFF")]
        public Color HeaderGradientColorX { get; set; }
        [DefaultValue(typeof(Color), "0x888888 ")]
        public Color HeaderGradientColorY { get; set; }
        [DefaultValue(80f)]
        public float HeaderGradientAngle { get; set; }
        public Border3DStyle HeaderBorder3DStyle { get; set; }
        public EgxPanel()
        {
            InitializeComponent();
            pHeader = new Panel() {Name="pHeader", Dock= DockStyle.Top , Height=20};
            pFooter = new Panel();
            lHeader = new Label() { Name = "pLabel", Dock = DockStyle.Fill, AutoSize = false, BackColor = Color.Transparent };
            pHeader.Paint+=pHeader_Paint;
            pHeader.Controls.Add(lHeader);
            this.Controls.Add(pHeader);
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            foreach (Control c in this.Controls) 
            {
                c.Click+=c_Click;
            }
        }

        private void c_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            MessageBox.Show((sender as Control).Name);
            if ((sender as Control).Name == "pContent") 
            {
                foreach (Control c in (sender as Control).Controls) 
                {
                    sb.Append(c.Name);
                }
                MessageBox.Show(sb.ToString());
            }
        }

        private void pHeader_Paint(object sender, PaintEventArgs e)
        {
            
            Graphics gfx = e.Graphics;
            Rectangle rect = new Rectangle(0, 0, this.lHeader.Width, this.lHeader.Height);
            using (LinearGradientBrush lgb = new LinearGradientBrush(rect, HeaderGradientColorX, HeaderGradientColorY, HeaderGradientAngle, true))
            {
                gfx.FillRectangle(lgb, rect);
                ControlPaint.DrawBorder3D(gfx, rect, HeaderBorder3DStyle);

            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
