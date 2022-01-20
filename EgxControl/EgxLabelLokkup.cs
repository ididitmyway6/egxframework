using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Egx.EgxControl
{
    public class EgxLabelLokkup : FlowLayoutPanel
    {
        public TextBox _textBox;
        public Label _label;

        public EgxLabelLokkup() 
        {
            _textBox = new TextBox();
            _label = new Label();
            _label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
           // FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.Controls.Add(_textBox);
            this.Controls.Add(_label);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}
