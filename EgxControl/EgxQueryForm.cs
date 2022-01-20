using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Egx.EgxControl;
using Egx.EgxDataAccess;
namespace Egx.EgxControl
{
    public partial class EgxQueryForm : EgxForm
    {
        #region Variables
        DataAccess da;
        #endregion

        #region Properties
        [DefaultValue("")]
        public string SqlQuery { get; set; }
        public string QueryBuilder { get; set; }
        [DefaultValue(false)]
        public bool ViewEngineEnabled { get; set; }
        public object ViewObject { get; set; }
        public List<object> CurrentView { get; set; }
        public EgxEntryForm ReferenceForm { get; set; }
        [DefaultValue("")]
        public string ReferenceID { get; set; }
        System.Data.DataTable dt;
        #endregion
        public EgxQueryForm()
        {
            InitializeComponent();
            da = new DataAccess();
            splitContainer1.Panel1Collapsed = true;

        }
        protected override void OnCreateControl()
        {
            if (!DesignMode)
            {
                if (ViewEngineEnabled)
                {

                }
                else
                {
                    FillGrid(SqlQuery);
                }
            }
        }

        public void FillGrid(string query)
        {
            dt = new DataTable();
            dt.Load(da.ExecuteSQL(query));
            dgv.DataSource = dt;
            FillComboBox();
        }

        public void FillGrid(DataTable table)
        {
            dgv.DataSource = table;
            FillComboBox();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }



        private void egxGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ReferenceID.Length > 0)
            {
                int? ID = (int?)dgv[ReferenceID, e.RowIndex].Value;
                if (ID != null)
                {
                    EgxEntryForm frm = new EgxEntryForm();
                    frm = ReferenceForm;
                    frm.egxNavigator1.StartupID = ID.Value;
                    frm.ShowDialog();
                }
            }
        }
        /// <summary>
        /// Setup Query Form With It's Reference form and ID
        /// </summary>
        /// <param name="RefForm">The Popup Form which will be shown on DataGrid Row's Double Click</param>
        /// <param name="RefID">The ID which Popup Form Curser to be set</param>
        public void SetupQueryForm(EgxEntryForm RefForm, string RefID, string TableName)
        {
            this.ReferenceForm = RefForm;
            this.ReferenceID = RefID;
            this.dgv.TableName = TableName;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            DataTable tmp = new DataTable();
            tmp = dt;

            int cnt = 0;

            QueryBuilder = "";

            foreach (DataGridViewRow dgvr in egxGridView1.Rows)
            {

                object val = dgvr.Cells[1].Value;
                if (val != null)
                {
                    cnt++;
                    if (cnt == 1)
                    {
                        sb.Append("[" + dgvr.Cells[0].Value.ToString() + "]" + " " + val.ToString() + " '" + dgvr.Cells[2].Value + "' ");
                    }
                    else
                    {
                        sb.Append(" AND " + "[" + dgvr.Cells[0].Value.ToString() + "]" + " " + val.ToString() + " '" + dgvr.Cells[2].Value + "' ");
                    }
                }
            }
            QueryBuilder = SqlQuery + " Where " + sb.ToString();
            if (tmp.Select(sb.ToString()).Length > 0)
            {
                tmp = tmp.Select(sb.ToString()).CopyToDataTable();
                SqlTxt.Text = sb.ToString();
            }
            FillGrid(tmp);
        }
        public void FillComboBox()
        {
            string typeName = "";
            egxComboBox1.Items.Clear();
            DataGridViewRow dgvr;
            int i = 0;
            int n = 0;
            i = dgv.Columns.Count;
            egxGridView1.Rows.Clear();
            egxGridView1.Rows.Add(i);
            foreach (DataGridViewColumn dgvc in dgv.Columns)
            {
                typeName = dgvc.ValueType.Name;
                if (typeName == "String")
                {
                    dgvr = new DataGridViewRow();
                    egxComboBox1.Items.Add(dgvc.HeaderText);
                }
                egxGridView1.Rows[n].Cells[0].Value = dgvc.HeaderText;
                n++;

            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            FillGrid(SqlQuery);
            splitContainer1.Panel1Collapsed = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
