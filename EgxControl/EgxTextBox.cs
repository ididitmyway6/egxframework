using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Egx.EgxDataAccess;
using System.Globalization;

namespace Egx.EgxControl
{
    public partial class EgxTextBox : TextBox
    {
        ErrorProvider errp;
        private const uint ECM_FIRST = 0x1500;
        private const uint EM_SETCUEBANNER = ECM_FIRST + 1;
        [DefaultValue(typeof(Color),"Gray")]
        public Color FocusColor { get; set; }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        private string watermarkText;
        [Category("EGX PROPERTIES"), Description("النص المائى")]
        public string WatermarkText
        {
            get { return watermarkText; }
            set
            {
                watermarkText = value;
                SetWatermark(DesignMode ? Name : watermarkText);
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
        public TextType Type { get; set; }

        public bool StopTabOnEnter { get; set; }

        private void SetWatermark(string watermarkText)
        {
            SendMessage(this.Handle, EM_SETCUEBANNER, 0, watermarkText);
        }


        protected override void OnEnter(EventArgs e)
        {
            if (Text == "")
                SetWatermark(DesignMode ? Name : watermarkText);
            BackColor = FocusColor;
            if (Text != "")
                this.SelectAll();
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            if (!DesignMode)
            {
                try
                {
                    int intTest = 0;
                    if (RightToLeft == System.Windows.Forms.RightToLeft.Yes) { errp.RightToLeft = true; errp.SetIconAlignment(this, ErrorIconAlignment.MiddleRight); }
                    SetWatermark(DesignMode ? Name : watermarkText);
                    base.BackColor = Color.White;
                    switch (Type)
                    {
                        case TextType.Integer:
                            if (!int.TryParse(Text, out intTest) && Text.Trim().Length > 0) { Focus(); errp.SetError(this, "يجب ادخال قيمة صحيحة"); } else { errp.SetError(this, String.Empty); errp.Clear(); }
                            break;
                        case TextType.Decimal:
                            NumberFormatInfo nfi = CultureInfo.CurrentCulture.NumberFormat;
                            var s = string.Format("{0:c}", Decimal.Parse(this.Text)).Replace(nfi.CurrencySymbol, "");
                            this.Text = s;
                            break;
                    }
                    this.BackColor = Color.White;
                    this.ForeColor = Color.Black;
                }
                catch (Exception exp) { this.BackColor = Color.Red; this.ForeColor = Color.Black; this.Focus(); MessageBox.Show("Input Error please check your input .." + Environment.NewLine + exp.Message); }
            } base.OnLeave(e);
        }

      //  [Category("EGX PROPERTIES"), Description("نوع النص ")]
      //  public TextBoxTypes TextBoxType { get; set; }

        public EgxTextBox()
        {
            InitializeComponent();
            BorderStyle = BorderStyle.FixedSingle;
            errp = new ErrorProvider();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
                if (e.KeyCode == Keys.Enter)
                {
                    if (!StopTabOnEnter)
                    {
                        SendKeys.Send("{TAB}");
                        e.SuppressKeyPress = true;
                    }
                }
            
            base.OnKeyDown(e);

        }
        protected override void OnCreateControl()
        {
            SetWatermark(DesignMode ? Name : watermarkText);
            base.OnCreateControl();
        }

    }
}
