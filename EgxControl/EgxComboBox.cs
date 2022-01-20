using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Egx.EgxDataAccess;

namespace Egx.EgxControl
{
    public partial class EgxComboBox : ComboBox
    {

        public EgxComboBox()
        {
            InitializeComponent();
            // this.BorderStyle = BorderStyle.FixedSingle;
            KeyDown += EgxTextBox_KeyDown;
        }

        private Color _FocusColor = Color.Orange;
        [DefaultValue(typeof(Color), "Gray")]
        public Color FocusColor { get { return _FocusColor; } set { _FocusColor = value; } }
        private Egx.EgxDataAccess.EgxDataType egxdatatype = EgxDataType.Mssql;
        [Category("EGX PROPERTIES"), Description("نوع قاعدة البيانات")]
        public EgxDataType Egxdatatype
        {
            get { return egxdatatype; }
            set { egxdatatype = value; }
        }
        [DefaultValue(-1)]
        public int AppCode { get; set; }
      
        public bool FillBySql { get; set; }

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

        [Category("EGX PROPERTIES"), Description("جملة السيكول")]
        public string SqlStr { get; set; }

        private void EgxTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.GetNextControl((Control)sender, true);
                SendKeys.Send("{TAB}");
            }
        }

        protected override void OnCreateControl()
        {
            Text = DesignMode ? Name : "";
            BackColor = Color.White;
            base.OnCreateControl();
            if (!DesignMode)
            {
                try
                {
                    if (FillBySql)
                    {
                        if (SqlStr.Nz("") == "" && Items.Count > 0)
                            this.Fill();
                        else if (SqlStr.Nz("") != "")
                            this.Fill(SqlStr, egxdatatype);
                    }
                    else if (AppCode > 0)
                    {
                        this.Fill("select code,AR_Desc from AppCodes WHERE ID='"+AppCode.ToString()+"'", egxdatatype);

                    }
                }
                catch (Exception exc) { MessageBox.Show(exc.Message); }
            }

           

        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            BackColor = Color.White;
        }
        protected override void OnPaint(PaintEventArgs pe)
        {

            base.OnPaint(pe);
            this.FlatStyle = FlatStyle.Popup;
            ControlPaint.DrawBorder(pe.Graphics, this.ClientRectangle, Color.Black, 1, ButtonBorderStyle.Solid, Color.Black, 1, ButtonBorderStyle.Outset, Color.Black, 1, ButtonBorderStyle.Outset, Color.Black, 1, ButtonBorderStyle.Solid);
        }
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            BackColor = FocusColor;
        }
    }
}
