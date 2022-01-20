using Egx.EgxDataAccess;
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
    
    public partial class EgxTextLookup :EgxTextBox
    {
        public EgxLookupForm lupForm = new EgxLookupForm();
        public delegate void LookupClosing(object sender);
        public event LookupClosing LookupClosingEvent;
        public string LookupQuery { get; set; }
        public string LookupValueMember { get; set; }
        public string LookupID { get; set; }
        public object DataSource { get; set; }
        public object Value { get; set; }
        private ToolTip ttp = new ToolTip();
        public DataGridViewRow CurrentRow { get; set; }
        public virtual void onLookupClosing(object sender) 
        {
            if (LookupClosingEvent != null)
            {
                LookupClosingEvent(this);
            }
            
        }
        public EgxTextLookup()
        {

            LookupID = ""; LookupQuery = ""; LookupValueMember = "";
            InitializeComponent();
         //   this.DoubleClick+=EgxTextLookup_DoubleClick;   
            //this.GotFocus+=EgxTextLookup_GotFocus;
           // this.LostFocus+=EgxTextLookup_LostFocus;
            
        }

        private void EgxTextLookup_DoubleClick(object sender, EventArgs e)
        {
            
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            base.OnDoubleClick(e);
            if ((LookupQuery == "" || LookupValueMember == "") && LookupID.Trim().Length > 0)
            {
                lupForm = new EgxLookupForm();
                lupForm.FormClosing += luform_FormClosing;
                lupForm.LookupID = LookupID;
                
                lupForm.ShowDialog();
            }
            else if(DataSource!=null)
            {
                lupForm = new EgxLookupForm();
                lupForm.FormClosing += luform_FormClosing;
                lupForm.dgv.DataSource = DataSource;
                lupForm.DataSource = DataSource;
                lupForm.ValueMember = LookupValueMember;
                lupForm.Show();
            }
            else
            {
                if (LookupQuery.Trim().Length > 0)
                {
                    lupForm = new EgxLookupForm();
                    lupForm.FormClosing += luform_FormClosing;
                    lupForm.SqlQuery = LookupQuery;
                    lupForm.ValueMember = LookupValueMember;
                    lupForm.dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    lupForm.dgv.RowHeadersVisible = false;
                    lupForm.ShowDialog();
                }
            }
        }
        private void EgxTextLookup_LostFocus(object sender, EventArgs e)
        {


        }
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if ((LookupQuery == "" || LookupValueMember == "") && LookupID.Trim().Length > 0)
            {

                lupForm = new EgxLookupForm();
                lupForm.FormClosing += luform_FormClosing;
                lupForm.LookupID = LookupID;
                lupForm.FillForm();
                if (this.Text.Length > 0)
                {
                    string filterQ = lupForm.ValueMember + "='" + this.Text + "'";
                    string s = Egx.EgxDataAccess.DataAccess.GetDataTable(lupForm.SqlQuery).Select(filterQ)[0][lupForm.DisplayMember].ToString();
                    ttp.IsBalloon = true;
                    ttp.Show(s, this);
                }
            }
            else
            {
                if (LookupQuery.Trim().Length > 0)
                {
                    lupForm = new EgxLookupForm();
                    lupForm.FormClosing += luform_FormClosing;
                    lupForm.SqlQuery = LookupQuery;
                    lupForm.ValueMember = LookupValueMember;
                    lupForm.dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    // lupForm.ShowDialog();
                }
            }
        }

       // protected override void OnGotFocus(EventArgs e)
       // {
         //  base.OnGotFocus(e);
          ////  EgxTextLookup etl = (EgxTextLookup)sender;
          //  if ((LookupQuery == "" || LookupValueMember == "") && LookupID.Trim().Length > 0)
          //  {

          //      lupForm = new EgxLookupForm();
          //      lupForm.FormClosing += luform_FormClosing;
          //      lupForm.LookupID = LookupID;
          //      lupForm.FillForm();
          //      if (this.Text.Length > 0)
          //      {
          //          string filterQ = lupForm.ValueMember + "='" + this.Text + "'";
          //          string s = DataAccess.GetDataTable(lupForm.SqlQuery).Select(filterQ)[0][lupForm.DisplayMember].ToString();
          //          ttp.IsBalloon = true;
          //          ttp.Show(s, this);
          //      }
          //  }
          //  else
          //  {
          //      if (LookupQuery.Trim().Length > 0)
          //      {
          //          lupForm = new EgxLookupForm();
          //          lupForm.FormClosing += luform_FormClosing;
          //          lupForm.SqlQuery = LookupQuery;
          //          lupForm.ValueMember = LookupValueMember;
          //          lupForm.dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
          //          // lupForm.ShowDialog();
          //      }
          //  }
          
       // }

        private void EgxTextLookup_GotFocus(object sender, EventArgs e)
        {
           
        }

     

        //private void EgxTextLookup_DoubleClick(object sender, EventArgs e)
        //{
           
            
        //}


        private void luform_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (lupForm.Value != null)
            {
               
                if (lupForm.Value.ToString().Trim().Length > 0)
                {
                    this.Value = lupForm.Value.ToString();
                    Text = Value.ToString();
                    CurrentRow = lupForm.dgv.CurrentRow;
                    onLookupClosing(this);
                }

            }   
        }

        protected override void OnCreateControl()
        {

            base.OnCreateControl();
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
