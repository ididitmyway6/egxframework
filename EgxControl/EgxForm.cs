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
    public partial class EgxForm : Form
    {
        [DefaultValue(false)]
        public bool IsMainForm { get; set; }
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        [DefaultValue("Title")]
        public string WindowTitle { get; set; }
        public bool TitleBar { get; set; }
        [DefaultValue(false)]
        public static bool isClassicTheme { get; set; }
        //public bool MaximizeForm { get { if (WindowState == FormWindowState.Maximized) { return true; } else { return false; } } set { WindowState = FormWindowState.Maximized ;MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;  } }
        protected override void OnCreateControl()
        {
          
                if (TitleBar) { toolStrip1.Visible = true; } else { toolStrip1.Visible = false; }
                base.OnCreateControl();
            
        }
        private void EgxForm_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
           
        }

        private void EgxForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void EgxForm_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
        public EgxForm()
        {
            if (isClassicTheme) 
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                this.ControlBox = true;
                this.MinimizeBox = true;
                this.MaximizeBox = true;
                
            }
            InitializeComponent();
            if (!isClassicTheme)
            {
                this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
                this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
             
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.ControlBox = false;
                this.MinimizeBox = false;
                this.MaximizeBox = false;
                TitleBar = true;
                toolStrip1.MouseUp += EgxForm_MouseUp;
                toolStrip1.MouseMove += EgxForm_MouseMove;
                toolStrip1.MouseDown += EgxForm_MouseDown;
                this.Paint += EgxForm_Paint;
                this.toolStrip1.Paint += toolStrip1_Paint;
                this.BackColor = Color.White;
                this.WindowTitleLbl.Text = WindowTitle;
            }
           
        }
       
        private void toolStrip1_Paint(object sender, PaintEventArgs e)
        {
            if(!isClassicTheme)
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, 1, ButtonBorderStyle.Solid, Color.Black, 1, ButtonBorderStyle.Outset, Color.Black,1, ButtonBorderStyle.Outset, Color.Black,1, ButtonBorderStyle.Solid);
        }

        private void EgxForm_Paint(object sender, PaintEventArgs e)
        {
            if(!isClassicTheme)
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, 1,ButtonBorderStyle.Solid,Color.MediumBlue,1,ButtonBorderStyle.Outset,Color.Black,1,ButtonBorderStyle.Outset,Color.Black,1,ButtonBorderStyle.Solid);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
       
                if (DesignMode)
                {
                    this.BackColor = Color.White;
                    this.WindowTitleLbl.Text = WindowTitle;
                }
                this.BackColor = Color.White;
            
          
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (IsMainForm)
            {
                Application.Exit();
            }
            else 
            {
                this.Close();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

       
      
       

        
    }
}
