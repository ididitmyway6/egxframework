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
    public partial class EgxClick : Label
    {
        [DefaultValue(null)]
        public Image MouseMoveIcon { get; set; }
        [DefaultValue(null)]
        public Image MouseLeaveIcon { get; set; }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (MouseMoveIcon != null && MouseLeaveIcon != null)
            {
                this.MouseMove += EgxClick_MouseMove;
                this.MouseLeave += EgxClick_MouseLeave;
                this.Image = MouseLeaveIcon;
                this.AutoSize = false;
                this.Size = Image.Size;
                this.Text = "";
            }
        }
        public EgxClick()
        {
            InitializeComponent();
            //if (MouseMoveIcon != null && MouseLeaveIcon != null)
            //{ 
            //    this.MouseMove+=EgxClick_MouseMove;
            //    this.MouseLeave+=EgxClick_MouseLeave;
            //}
          
            this.Cursor = Cursors.Hand;
        }

        private void EgxClick_MouseLeave(object sender, EventArgs e)
        {
            this.Image = MouseLeaveIcon;
        }

        private void EgxClick_MouseMove(object sender, MouseEventArgs e)
        {
            this.Image = MouseMoveIcon;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (DesignMode)
            {
                if (MouseMoveIcon != null && MouseLeaveIcon != null)
                {
                    this.MouseMove += EgxClick_MouseMove;
                    this.MouseLeave += EgxClick_MouseLeave;
                    this.Image = MouseLeaveIcon;
                    this.AutoSize = false;
                    this.Size = Image.Size;
                    this.Text = "";
               }
           }
        }
    }
}
