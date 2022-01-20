using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel.Design;

namespace Egx.EgxControl
{
    public partial class EgxDateText : TextBox
    {
        DateTime test = DateTime.Now;
        protected override void OnCreateControl()
        {
           // if(Text.Length==0)
            this.Text = DateTime.Now.ToShortDateString();
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                try
                {
                    if (value == null) { value = null; }
                    else
                    {
                        base.Text = (DateTime.Parse(value)).ToShortDateString();
                    }
                }
                catch
                {
                    if (!this.DesignMode) { base.Text = "Invalid"; } else { base.Text = DateTime.Now.ToShortDateString(); }
                }
            }
        }

        [Category("EGX Validations"), Description("Is Required Field")]
        [DefaultValue(false)]
        public bool IsMandatory
        {
            get;
            set;
        }
        [Category("EGX Validations"), Description("Required Error Text")]
        [DefaultValue("")]
        public string RequiredErrorText
        {
            get;
            set;
        }

        CultureInfo provider = CultureInfo.InvariantCulture;
        public bool HasError { get; set; }

        [Description("ترجع بقيمة تاريخ من نوع يونيفرسال")]
        public DateTime? Value
        {
            //get;
            //set;
            get
            {
                if (DateTime.TryParse(this.Text, out test))
                {
                    return DateTime.Parse(this.Text);
                }
                else { return null; }
            }
            set
            {
                if (DateTime.TryParse(this.Text, out test))
                {
                    value = DateTime.Parse(this.Text);
                }
                else
                { value = null; }

            }
        }

        public EgxDateText()
        {
            InitializeComponent();
            this.Value = null;
            this.LostFocus += EgxDateText_LostFocus;
            this.KeyDown += EgxDateText_KeyDown;
            this.Enter += EgxDateText_Enter;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.BackColor = Color.White;
        }



        private void EgxDateText_Enter(object sender, EventArgs e)
        {
            HasError = false;
        }

        private void EgxDateText_LostFocus(object sender, EventArgs e)
        {
            doMethod();
        }

        private void EgxDateText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                doMethod();
                e.SuppressKeyPress = true;
                this.GetNextControl((Control)sender, true);
                SendKeys.Send("{TAB}");

            }
        }

        public void doMethod()
        {
            if (!HasError)
            {
                string x = this.Text;

                bool IsValid = DateTime.TryParse(this.Text, out test);

                try
                {
                    if (x.Length == 4)
                    {
                        string format = "ddMM";
                        DateTime dt = DateTime.ParseExact(x, format, provider);
                        this.Text = dt.ToShortDateString();
                        this.Value = DateTime.Parse(this.Text);
                    }
                    else if (x.Length == 8)
                    {
                        string format = "ddMMyyyy";
                        DateTime dt = DateTime.ParseExact(x, format, provider);
                        this.Text = dt.ToShortDateString();
                        this.Value = DateTime.Parse(this.Text);
                    }
                    else if (this.Text.Trim().Length == 0)
                    {
                        this.Value = null;
                    }
                    else if (!IsValid)
                    {
                        this.Text = "Invalid";
                        this.HasError = true;
                        this.Value = null;
                    }
                }
                catch
                {
                    this.Text = "Invalid"; this.HasError = true;
                    this.Value = null;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            //base.OnPaint(pe);
            if (this.DesignMode)
            {
                this.Text = DateTime.Now.ToString();
            }
        }

        private EgxCalender Calender;
        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            Calender = new EgxCalender(this);
            Calender.TopMost = true;
            Calender.ShowDialog(this);
        }
        
    }
}
