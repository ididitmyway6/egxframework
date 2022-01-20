using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Egx.EgxDataAccess;
namespace Egx.EgxControl
{
    public partial class EgxLookupForm : EgxForm
    {
        DataAccess da;
        public string SqlQuery { get; set; }
        public string LookupID { get; set; }
        public object Value { get; set; }
        public string ValueMember { get; set; }
        public string DisplayMember { get; set; }
        public object DataSource { get; set; }
        public DataTable dt;
        public EgxLookupForm()
        {
            InitializeComponent();
            da = new DataAccess();
            dgv.CellDoubleClick += dgv_CellDoubleClick;
            this.Shown += EgxLookupForm_Shown;
            dt = new DataTable();
            LookupID = "";
             
        }

        private void EgxLookupForm_Shown(object sender, EventArgs e)
        {
            FillForm();
            string typeName = "";
            egxComboBox1.Items.Clear();
            foreach (DataGridViewColumn dgvc in dgv.Columns)
            {
                typeName = dgvc.ValueType.Name;
                if (typeName == "String")
                {
                    egxComboBox1.Items.Add(dgvc.HeaderText);
                }

            }
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (dgv[ValueMember, e.RowIndex].Value != null)
                {
                    Value = dgv[ValueMember, e.RowIndex].Value;
                    this.Close();
                }
                else { Value = null; }
            }
        }
        public void FillForm()
        {
            try
            {
                if (DataSource != null) {
                    dgv.DataSource = DataSource;
                    dgv.RowHeadersVisible = false;
                }
                else
                {
                    dgv.RowHeadersVisible = true;
                    if (LookupID.Trim().Length > 0)
                    {
                        DataRow dr = DataAccess.GetDataRow("SELECT * FROM Lookup WHERE ID=" + LookupID + "", EgxDataType.Temp);
                        switch (DBConfig.Lang)
                        {
                            case "AR":
                                SqlQuery = dr["AR_Sql"].ToString();
                                DisplayMember = dr["DisplayMember"].ToString();
                                break;
                            case "EN":
                                SqlQuery = dr["EN_Sql"].ToString();
                                DisplayMember = dr["DisplayMember"].ToString();
                                break;
                            default:
                                SqlQuery = dr["AR_Sql"].ToString();
                                DisplayMember = dr["DisplayMember"].ToString();
                                break;

                        }
                        ValueMember = dr["ValueMember"].ToString();
                    }
                    dt.Load(da.ExecuteSQL(SqlQuery));
                    dgv.DataSource = dt;
                }
            }
            catch (Exception exc) { MessageBox.Show("There is no query for Entry Form Lookup ...... \n"+exc.Message, "Error"); }
        }

        private void egxClick2_Click(object sender, EventArgs e)
        {
            if (egxComboBox1.Text.Length > 0 && egxTextBox1.Text.Length > 0)
            {
                DataTable tmp = new DataTable();
                tmp = dt;
                string fltr = "[" + egxComboBox1.Text + "] like '%" + egxTextBox1.Text + "%'";
                if (tmp.Select(fltr).Length > 0)
                {
                    tmp = tmp.Select(fltr).CopyToDataTable();
                }
                else
                {
                    MessageBox.Show("غير موجود", CompanyName);
                }
                dgv.DataSource = tmp;
            }
            else
            {
                MessageBox.Show("مربع البحث فارغ", CompanyName);
            }
        }

        private void egxClick1_Click(object sender, EventArgs e)
        {

            if (dgv[ValueMember, dgv.CurrentCell.RowIndex].Value != null)
            {
                Value = dgv[ValueMember, dgv.CurrentCell.RowIndex].Value;
                this.Close();
            }
            else { Value = null; }
        }

    }


}

