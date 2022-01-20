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
    public partial class SelectedFieldsEditor : Form
    {
        private Dictionary<string, string> __dic = new Dictionary<string, string>();
        public Dictionary<string, string> dic { get { return __dic; } }
        public SelectedFieldsEditor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            __dic.Add(textBox1.Text, textBox2.Text);
        }
    }
}
