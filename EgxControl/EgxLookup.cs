using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Egx.EgxDataAccess;
namespace Egx.EgxControl
{
    public partial class EgxLookup : TextBox
    {
       // private TextBox tb;
        public delegate void IDChangeEventHandler(object sender, EventArgs e);
        public event IDChangeEventHandler IDChangedEvent;
        public virtual void OnIDChanged(EventArgs e) 
        {
            if (IDChangedEvent == null) 
            { 
                IDChangedEvent(this, e);
            }
            else
            {
                if (!popup.Visible)
                {
                    GetResult(null);
                    this.Text = "";
                    if (Find(ReturnedIDColumn, this.id.ToString()))
                    {
                        this.Text = dgv[ReturnedIDColumn, dgv.SelectedCells[0].RowIndex].Value.ToString() + " - " + dgv[ReturnedValueColumn, dgv.SelectedCells[0].RowIndex].Value.ToString();
                        ValueType = dgv[ReturnedValueColumn, dgv.SelectedCells[0].RowIndex].Value.GetType().Name;
                        switch (ValueType)
                        {
                            case "Int32":
                                this.Value = Int32.Parse(dgv[ReturnedValueColumn, dgv.SelectedCells[0].RowIndex].Value.ToString());
                                break;
                            case "String":
                                this.Value = dgv[ReturnedValueColumn, dgv.SelectedCells[0].RowIndex].Value.ToString();
                                break;
                            case "DateTime":
                                this.Value = DateTime.Parse(dgv[ReturnedValueColumn, dgv.SelectedCells[0].RowIndex].Value.ToString());
                                break;
                        }
                        this.Value = dgv[ReturnedValueColumn, dgv.SelectedCells[0].RowIndex].Value;
                    }
                }

            } 
        }
        private ComboBox cb = new ComboBox();
        DataGridViewRow dgvr;
        private DataGridView dgv;
        private ToolStripControlHost tsc;
        private ToolStripDropDown popup;
        public string ReturnedIDColumn { get; set; }
        public string ReturnedValueColumn { get; set; }
        public static DBConfig DBConfigClass { get; set; }
        [DefaultValue("")]
        public string ConnectionString { get; set; }
        public string SqlQuery { get; set; }
        public object Value { get; set; }
        private string constr = null;
        private SqlConnection conn = null;
        private SqlDataAdapter da = null;
        private DataSet ds = null;
        private string ValueType = null;
        private TextBox fndTxt = new TextBox();
        private Button selectButton = new Button();
        private int id;
        public int ID
        {
            //get ;set;//
            get{return id;}
            set
            {
                id = value;
               // OnIDChanged(EventArgs.Empty);
              // SelectValue();
            }
        }
        
        public EgxLookup()
        {
            InitializeComponent();
            this.BorderStyle = BorderStyle.FixedSingle;
            this.BackColor = Color.White;
            this.KeyDown+=EgxLookup_KeyDown; 
            dgvr = new DataGridViewRow();
            dgv = new EgxGridView();
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.BindingContext = new BindingContext();
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.ColumnHeadersHeight = 20;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.EnableHeadersVisualStyles = false;
            this.dgv.Location = new System.Drawing.Point(0, 0);
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.MultiSelect = false;
            this.dgv.Size = new System.Drawing.Size(473, 100);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            Panel p = new Panel();
            Panel gridPanel = new Panel();
            FlowLayoutPanel fp = new FlowLayoutPanel();
            p.Size = new System.Drawing.Size(473, 200);
            selectButton.Click+=selectButton_Click;
            
            Label lbl = new Label();
            Label lblFnd = new Label();
            fndTxt.TextChanged+=fndTxt_TextChanged;
            fndTxt.Width = 200;
            lbl.Text = "بحث بواسطة";
            lbl.AutoSize = true;
            cb.Width = 100;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            selectButton.Text = "اختيار";
           // fp.Location = new Point(0, 0);
            fp.Size =new System.Drawing.Size(473,fndTxt.Size.Height+5);
            fp.Controls.Add(lbl);
            fp.Controls.Add(cb);
            fp.Controls.Add(fndTxt);
            fp.Controls.Add(selectButton);
            gridPanel.Controls.Add(dgv);
          //  gridPanel.Location = new Point(0, fndTxt.Size.Height + 10);
            gridPanel.Location = new Point(0, 0);
            gridPanel.Size = new System.Drawing.Size(473, 150);
            fp.Location = new Point(0, gridPanel.Height);
            fp.BackColor = Color.White;
            p.Controls.Add(gridPanel);
            p.Location = new Point(0, 0);
            p.Controls.Add(fp);
            p.BorderStyle = System.Windows.Forms.BorderStyle.None;
            tsc = new ToolStripControlHost(p);
            tsc.Padding = new Padding(0);
            tsc.Margin = new Padding(0);
            tsc.AutoSize = false;
            this.BackColor = Color.White;
            p.BackColor = Color.White;
            popup = new ToolStripDropDown();
            popup.Padding = new Padding(0);
            popup.Margin = new Padding(0);
            popup.AutoSize = true;
            popup.DropShadowEnabled = false;
            popup.Items.Add(tsc);
            popup.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            popup.DefaultDropDownDirection = ToolStripDropDownDirection.BelowLeft;
            tsc.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Click+=tb_Click;
            dgv.CellClick+=dgv_CellClick;
            if (!DesignMode) 
            {
                //Initialze Database Configuration Class
                try 
                {
                    if (ConnectionString != null && SqlQuery != null)
                    {
                        
                       

                    }
                    else if (ConnectionString!=null)
                    {
                        
                    }
                    else 
                    {
                       // throw new Exception("No Connection Data is Available !!");
                    }
                }
                catch { //throw new Exception("No Connection Data is Available !!"); 
                }
                //Initialize Control Required Data
                try
                {
                    if (ReturnedIDColumn == null || ReturnedValueColumn == null)
                    {
                       // throw new Exception("Returned ID Column name is null !!");
                    }
                }
                catch { //throw new Exception("Returned ID Column name or Returned Value Column name is null !!"); 
                }
            }
        }

        private void EgxLookup_KeyDown(object sender, KeyEventArgs e)
        {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    this.GetNextControl((Control)sender, true);
                    SendKeys.Send("{TAB}");
                    try
                    {
                      
                        int o = 0;
                        if (int.TryParse(this.Text, out o))
                        {
                            
                            this.ID = int.Parse(this.Text);
                            SelectValue();
                        }
                    }
                    catch { }
                }
                else if (e.KeyCode == Keys.Down) 
                {
                    popup.Show();
                }
        }
        
        private void selectButton_Click(object sender, EventArgs e)
        {
           SelectValue();
        }

        private void SelectValue() 
        {
            if (SqlQuery != null)
            {   
                    this.Text = "";
                    this.Text = dgv[ReturnedIDColumn, dgv.SelectedCells[0].RowIndex].Value.ToString() + " - " + dgv[ReturnedValueColumn, dgv.SelectedCells[0].RowIndex].Value.ToString();
                    ValueType = dgv[ReturnedValueColumn, dgv.SelectedCells[0].RowIndex].Value.GetType().Name;
                    switch (ValueType)
                    {
                        case "Int32":
                            this.Value = Int32.Parse(dgv[ReturnedValueColumn, dgv.SelectedCells[0].RowIndex].Value.ToString());
                            break;
                        case "String":
                            this.Value = dgv[ReturnedValueColumn, dgv.SelectedCells[0].RowIndex].Value.ToString();
                            break;
                        case "DateTime":
                            this.Value = DateTime.Parse(dgv[ReturnedValueColumn, dgv.SelectedCells[0].RowIndex].Value.ToString());
                            break;
                    }
                    this.Value = dgv[ReturnedValueColumn, dgv.SelectedCells[0].RowIndex].Value;
                  this.ID = (int)dgv[ReturnedIDColumn, dgv.SelectedCells[0].RowIndex].Value;
                    popup.Hide();
               
            }
        }

        public bool Find(string FindBy,object value)
        {
            try
            {
               
                    if (FindBy == null)
                    {
                        dgvr = dgv.Rows.Cast<DataGridViewRow>().Where(c => c.Cells[cb.Text].Value.ToString().StartsWith(fndTxt.Text)).First();
                        dgv.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                        dgv.CurrentCell = dgv.Rows[dgvr.Index].Cells[cb.Text];
                    }
                    else
                    {
                        dgvr = dgv.Rows.Cast<DataGridViewRow>().Where(c => c.Cells[FindBy].Value.ToString().StartsWith(this.ID.ToString())).First();
                        dgv.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                        dgv.CurrentCell = dgv.Rows[dgvr.Index].Cells[FindBy];
                    }

                    return true;
            }

            catch { this.Text = ""; this.Value = null; return false; }
        }

        private void fndTxt_TextChanged(object sender, EventArgs e)
        {
            Find(null,null);
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                SelectValue();
            }
            catch { }
        }


        private void tb_Click(object sender, EventArgs e)
        {
            try
            {
                #region Setup DB Connection 
                //if (ConnectionString != null)
                //{
                //    constr = ConnectionString;
                //}
                //else
                //{
                //    constr = "Server='" + DBConfig.ServerName + "';Database='" + DBConfig.DBName + "';User Id='" + DBConfig.UserName + "'; Password='" + DBConfig.Password + "';";
                //}
                //    conn = new SqlConnection(constr);
                //da = new SqlDataAdapter(SqlQuery, conn);
                //ds = new DataSet();
                //da.Fill(ds);
                //dgv.DataSource = ds.Tables[0];
                GetResult(null);
                #endregion

                #region List Initialization

                #endregion

                #region Setup Control Layout
                Point frmlocation = this.FindForm().Location;
                frmlocation.X += this.Left;
                frmlocation.Y += this.Top + 60;
                //dgv.AutoSize = true;
                dgv.AutoSizeColumnsMode =DataGridViewAutoSizeColumnsMode.Fill;
                popup.AutoSize = true;
                popup.AllowTransparency = true;
                popup.DropShadowEnabled = true;
                popup.Show(frmlocation);
                #endregion
                cb.Items.Clear();
                foreach (DataGridViewColumn dgvc in dgv.Columns) 
                {
                    cb.Items.Add(dgvc.HeaderText);
                }
            }
            catch { }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }


        private void GetResult(string condition) 
        {
            if (ConnectionString != null)
            {
                constr = ConnectionString;
            }
            else
            {
                constr = "Server='" + DBConfig.ServerName + "';Database='" + DBConfig.DbName + "';User Id='" + DBConfig.UserName + "'; Password='" + DBConfig.Password + "';";
            }
            conn = new SqlConnection(constr);
            if (condition == null)
            {
                da = new SqlDataAdapter(SqlQuery, conn);
            }
            else 
            {
                da = new SqlDataAdapter(SqlQuery+condition, conn);
            }
            if (SqlQuery != null)
            {
                ds = new DataSet();
                da.Fill(ds);
                dgv.DataSource = ds.Tables[0];
            }
        }

        

        public void SetLookupList(object list) 
        {

            dgv.DataSource = null;
            dgv.DataSource = list;
        }

    }
}
