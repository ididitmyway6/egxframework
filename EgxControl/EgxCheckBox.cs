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
    public partial class EgxCheckBox : CheckBox
    {
        public EgxCheckBox()
        {
            InitializeComponent();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            FlatStyle = FlatStyle.Flat;
            BackColor = Color.White;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                GetNextControl(this, true);
                SendKeys.Send("{TAB}");
            }
        }
     
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

    }
}
