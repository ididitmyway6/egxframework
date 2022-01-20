using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using Egx.EgxDataAccess;
using System.Drawing.Design;
using System.Windows.Forms.Design;
namespace Egx.EgxControl
{
    public enum RowAction{Insert , Update , Delete , Other};
    public partial class EgxGridView : DataGridView
    {
        #region EgxEvents
        public delegate void AfterAction(object sender, ActionEventArgs e);
        public delegate void BeforeAction(object sender, ActionEventArgs e);
        public delegate bool Validate(object sender, ActionEventArgs e);
        [Category("EgxEvents")]
        public event AfterAction AfterActionEvent;
        [Category("EgxEvents")]
        public event BeforeAction BeforeActionEvent;
        [Category("EgxEvents")]
        public event Validate ValidateEvent;
        public virtual void onBeforeAction(ActionEventArgs e) 
        {
            if (BeforeActionEvent != null) 
            {
                BeforeActionEvent(this, e);
            }
        }
        public virtual void OnAfterAction(ActionEventArgs e)
        {
            if (AfterActionEvent != null)
            {
                AfterActionEvent(this, e);
            }
        }
        public virtual bool onValidate(ActionEventArgs e) 
        {
            if (ValidateEvent != null)
            {
               return ValidateEvent(this, e);
                 
            }
            else { return false; }
        }
        #endregion
        DataSet ds;
        SqlDataAdapter da;
        DataTable dt;
        DataTable SchemaInfo;
        SqlConnection conn;
        SqlDataReader reader;
        BindingSource source = new BindingSource();
        EgxLookupForm luf;
        DataAccess dAccess;
        DataColumn AutoId;
        BindingSource bs;
        ErrorProvider errp;
        private string sqlquery ;
        [Category("EgxProperties")]
        public string SqlQuery { get { return sqlquery; } set { sqlquery = value; InitializeConnection(); } }
        //private Dictionary<string, string> __fields = new Dictionary<string, string>();
       // [Category("EgxProperties")]
       // [Editor(typeof(DictionaryEditor),
       //typeof(UITypeEditor))]
       // public Dictionary<string, string> SelectedFields { get { return __fields; } set { __fields = value; } }
        [Category("EgxProperties")]
        public string TableName { get; set; }
        [Category("EgxProperties")]
        public Color AlternateRowColor { get; set; }
        [Category("EgxProperties")]
        public Color AlternateRowForeColor { get; set; }
        [Category("EgxProperties")]
        public bool AlternativeRowEffect { get; set; }
        [Category("EgxProperties")]
        public List<string> DeleteByColumnName { get; set; }
        [Category("EgxProperties")]
        public bool  ObjectiveMood { get; set; }
        [Category("EgxProperties")]
        public Type ObjectType { get; set; }
        [Category("EgxProperties")]
        public bool EgxFeatures { get; set; }
        [Category("EgxProperties")]
        public string ListingMethod { get; set; }
        [Category("EgxProperties")]
        public string InsertMethod { get; set; }
        [Category("EgxProperties")]
        public string UpdateMethod { get; set; }
        [Category("EgxProperties")]
        public string DeleteMethod { get; set; }
        [Category("EgxProperties")]
        public Dictionary<string, string> HiddenFields { get; set; }
        //[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
        public string QueryFilter { get; set; }
        private List<string> AIColumns { get; set; }
        private List<EgxDataGridViewColumnInfo> PKs { get; set; }
        private object CurrentList { get; set; }
        private void GetSchemaInfo() 
        {
            if (!DesignMode && TableName.Length>0) 
            {
                dAccess = new DataAccess();
                List<string> lst = dAccess.GetPrimaryKeys(TableName);
                string colname=null;
                PKs = new List<EgxDataGridViewColumnInfo>();
                foreach (string colName in lst) 
                {
                    PKs.Add(new EgxDataGridViewColumnInfo() { ColumnName = colName });
                }
                if (PKs.Count > 0) 
                {
                    foreach (DataGridViewColumn dgvc in Columns) 
                    {
                        foreach (EgxDataGridViewColumnInfo dgvci in PKs) 
                        {
                            if (dgvc.GetType().Name.StartsWith("Egx"))
                            {
                                colname = dgvc.GetType().GetProperty("DataFieldName").GetValue(dgvc, null).ToString();
                                if (dgvci.ColumnName == colname)
                                {
                                    dgvci.Index = dgvc.Index;
                                }
                            }
                        }
                    }

                    
                }
                AIColumns = dAccess.getAutoIdentity(TableName);
                SchemaInfo = new System.Data.DataTable();
                da = new SqlDataAdapter("SELECT * FROM "+ TableName, conn);
                da.Fill(SchemaInfo);
            }
        }

        private string GetSelectedFieldsString() 
        {
            StringBuilder sb=new StringBuilder();
            int cnt = Columns.Count;
            int i = 0;
            if (Columns.Count > 0)
            {
                foreach (DataGridViewColumn dgvc in Columns)
                {
                    i++;
                    if (i != cnt)
                    {

                        sb.Append(dgvc.GetType().GetProperty("DataFieldName").GetValue(dgvc, null).ToString() + " AS " + "[" + dgvc.HeaderText + "]" + " , ");
                    }
                    else
                    {
                        sb.Append(dgvc.GetType().GetProperty("DataFieldName").GetValue(dgvc, null).ToString() + " AS " + "[" + dgvc.HeaderText + "]");
                    }

                }
            }
            
         
            return sb.ToString();
        }
        private void InitializeConnection() 
        {
            try
            {
                string q = "";
            
                if (!DesignMode && !ObjectiveMood)
                {
                    if (TableName.Length > 0)
                    {
                        dt = new System.Data.DataTable();
                        ds = new System.Data.DataSet();
                        
                        if (QueryFilter.Length == 0 && SqlQuery.Length==0)
                        {
                            SqlQuery = "SELECT " + GetSelectedFieldsString() + " FROM " + TableName;
                            AutoGenerateColumns = false;
                        }
                        else if (SqlQuery.Length == 0) { SqlQuery = "SELECT " + GetSelectedFieldsString() + " FROM " + TableName + " WHERE " + QueryFilter; AutoGenerateColumns = false; }
                       // DBConfig.ConnectionString = "Server='" + DBConfig.ServerName + "';Database='" + DBConfig.DbName + "';User Id='" + DBConfig.UserName + "'; Password='" + DBConfig.Password + "';";
                        conn = new SqlConnection(DBConfig.ConnectionString);
                        da = new SqlDataAdapter(SqlQuery, conn);
                        da.Fill(dt);
                        DataSource = dt;
                        
                        GetSchemaInfo();
                        dt.RowChanged += dt_RowChanged;

                    }
                }
                else if (!DesignMode) 
                {
                    bs = new BindingSource();
                    if (Columns.Count > 0)
                    {
                        
                        AutoGenerateColumns = false;
                        var lst = ObjectType.GetMethod(ListingMethod).Invoke(Activator.CreateInstance(ObjectType),null);
                        CurrentList = lst;
                        bs.DataSource = CurrentList;
                        DataSource = bs;
                    }
                    else 
                    {
                        AutoGenerateColumns = true;
                        var lst = ObjectType.GetMethod(ListingMethod).Invoke(Activator.CreateInstance(ObjectType), null);
                        bs.DataSource = lst;
                        DataSource = bs;
                    }
                    AllowUserToAddRows = true;
                   
                    
                }
                CellEndEdit += EgxGridView_CellEndEdit;

            }
            catch (Exception e) { string m = e.Message; }
        }

        private void EgxGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (bs != null)
            {
                dAccess = new DataAccess();
                int NewIndex = (int)dAccess.GetSqlValue("SELECT COUNT(*) FROM " + (ObjectiveMood ? ObjectType.GetMethod("ToString").Invoke(Activator.CreateInstance(ObjectType), null) : TableName));
                if (e.RowIndex != NewIndex)
                {
                    Update();
                    //if (Columns[e.ColumnIndex].GetType().Name.StartsWith("EgxTextBoxColumn"))
                    //{
                    //    if ((Columns[e.ColumnIndex] as EgxTextBoxColumn).ValidationRoles.Trim().Length > 0)
                    //    {

                    //    }
                    //}                }

                }
                //----------------------------------------

                int intTest = 0;
                if (RightToLeft == System.Windows.Forms.RightToLeft.Yes) { errp.RightToLeft = true; errp.SetIconAlignment(this, ErrorIconAlignment.MiddleRight); }
                base.BackColor = Color.White;
                if (Columns[e.ColumnIndex].GetType().Name.StartsWith("EgxTextBoxColumn"))
                {

                    switch ((Columns[e.ColumnIndex] as EgxTextBoxColumn).Type)
                    {
                        case TextType.Integer:
                            if (!int.TryParse(CurrentCell.Value.ToString(), out intTest) && CurrentCell.Value.ToString().Trim().Length > 0) { Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "يجب ادخال الحقل بشكل صحيح"; } else { Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = ""; }
                            break;
                    }
                }
            }
        }

      
        public EgxGridView()
        {
           InitializeComponent();
           TableName = "";
           QueryFilter = "";
           HiddenFields = new Dictionary<string, string>();
           if (DesignMode) { QueryFilter = ""; }
           this.AllowUserToDeleteRows = true;
           this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      
           
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (EgxFeatures)
            {
                if (keyData == Keys.Enter)
                {
                    dAccess = new DataAccess();
                    int NewIndex = (int)dAccess.GetSqlValue("SELECT COUNT(*) FROM " + (ObjectiveMood ? ObjectType.GetMethod("ToString").Invoke(Activator.CreateInstance(ObjectType), null) : TableName));
                    if (CurrentRow.Index == NewIndex)
                    {
                        DataGridViewCellEventArgs args = new DataGridViewCellEventArgs(CurrentCell.ColumnIndex, CurrentRow.Index);
                        EgxGridView_RowValidated(this, args);
                        return true;
                    }

                }
                return base.ProcessCmdKey(ref msg, keyData);
            }
            else { return base.ProcessCmdKey(ref msg, keyData); }
        }

    

        private void dt_RowChanged(object sender, System.Data.DataRowChangeEventArgs e)
        {
            if (e.Action == System.Data.DataRowAction.Change)
            {
                DataTable tbl = dt.GetChanges();
                Update();
                OnAfterAction(new ActionEventArgs() { ActionType = RowAction.Update, CurrentRow = CurrentRow.Index });
            }
        }

        private bool EgxGridView_ValidateEvent(object sender, ActionEventArgs e)
        {
            if (EgxFeatures)
            {
                bool x = false;
                List<string> lst = new List<string>();
                foreach (DataGridViewCell dgvc in CurrentRow.Cells)
                {

                    if (dgvc.OwningColumn.GetType().Name.StartsWith("Egx"))
                    {
                        string s = dgvc.OwningColumn.GetType().Name;
                        x = (bool)dgvc.OwningColumn.GetType().GetProperty("Required").GetValue(dgvc.OwningColumn, null);
                        if (x)
                        {
                            if (dgvc.EditedFormattedValue == DBNull.Value || dgvc.EditedFormattedValue == null)
                            {
                                lst.Add(dgvc.OwningColumn.HeaderText);
                            }
                        }
                    }

                }
                if (lst.Count > 0)
                {
                    MessageBox.Show("Missing Cells");
                    return false;
                }
                else { return true; }
            }
            else { return true; }
        }

        private void EgxGridView_RowValidated(object sender, DataGridViewCellEventArgs e)
        {

            if (EgxFeatures)
            {
                if (Rows.Count > 1 && AllowUserToAddRows)
                {

                    dAccess = new DataAccess();
                    int NewIndex = (int)dAccess.GetSqlValue("SELECT COUNT(*) FROM " + (ObjectiveMood ? ObjectType.GetMethod("ToString").Invoke(Activator.CreateInstance(ObjectType), null) : TableName));
                    if (e.RowIndex == NewIndex)
                    {
                        try
                        {
                            bool x = onValidate(new ActionEventArgs() { ActionType = RowAction.Insert, CurrentRow = e.RowIndex });
                            if (x)
                            {
                                onBeforeAction(new ActionEventArgs() { ActionType = RowAction.Insert, CurrentRow = e.RowIndex });
                                Insert();
                                OnAfterAction(new ActionEventArgs() { ActionType = RowAction.Insert, CurrentRow = e.RowIndex });
                            }
                        }
                        catch (Exception EXC) { try { if (conn.State == System.Data.ConnectionState.Open) { conn.Close(); } } catch { } }
                    }

                }
            }
        }

        private void EgxGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (EgxFeatures)
            {
                string tpe = Columns[e.ColumnIndex].GetType().Name;
                if (tpe == "EgxGridViewLookupColumn")
                {
                    luf = new EgxLookupForm();
                    luf.FormClosing += luf_FormClosing;
                    luf.SqlQuery = (Columns[e.ColumnIndex] as EgxGridViewLookupColumn).LookupQuery;
                    luf.ValueMember = (Columns[e.ColumnIndex] as EgxGridViewLookupColumn).ValueMember;
                    luf.ShowDialog();
                }
            }
        }

        private void luf_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (luf.Value != null)
                {
                    CurrentCell.Value = luf.Value.ToString();
                    RefreshEdit();
                }
            }
            catch (Exception exp) { MessageBox.Show(exp.Message, CompanyName); }
        }

       
        
        private void EgxGridView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (EgxFeatures)
                {
                    if (e.Control && e.KeyCode == Keys.Z)
                    {
                         InitializeConnection(); 
                    }
                    else if (e.KeyCode == Keys.Escape)
                    {
                        int NewIndex = (int)dAccess.GetSqlValue("SELECT COUNT(*) FROM " + TableName);
                        if (CurrentRow.Index == NewIndex)
                        {
                            this.Rows.Remove(CurrentRow);

                        }
                    }
                    else
                    {
                        if (e.KeyCode == Keys.Delete)
                        {
                            onBeforeAction(new ActionEventArgs() { ActionType = RowAction.Delete, CurrentRow = CurrentRow.Index });
                            if (!ObjectiveMood)
                            {
                                int colcnt = DeleteByColumnName.Count;
                                StringBuilder sb = new StringBuilder();
                                for (int i = 0; i < colcnt; i++)
                                {
                                    if (i < colcnt - 1)
                                    {
                                        sb.Append("[" + PKs[i].ColumnName + "]" + " = " + "'" + this[PKs[i].Index, CurrentRow.Index].Value.ToString() + "'" + " OR " + "[" + DeleteByColumnName[i] + "] IS NULL" + " AND ");
                                    }
                                    else
                                    {
                                        sb.Append("[" + DeleteByColumnName[i] + "]" + " = " + "'" + this[i, CurrentRow.Index].Value.ToString() + "'" + " OR " + "[" + DeleteByColumnName[i] + "] IS NULL");
                                    }
                                }
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = conn;
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.CommandText = "DELETE FROM " + TableName + " WHERE " + sb.ToString(); ;
                                cmd.Connection.Open();
                                cmd.ExecuteNonQuery();
                                cmd.Connection.Close();
                                dt.Rows[CurrentRow.Index].Delete();
                            }
                            else 
                            {
                                if (DeleteMethod.Trim().Length > 0) 
                                {
                                    EgxBusiness.SystemMessage sm = new EgxBusiness.SystemMessage();
                                    var obj = Activator.CreateInstance(ObjectType);

                                    sm = (EgxBusiness.SystemMessage)CurrentRow.DataBoundItem.GetType().GetMethod(DeleteMethod).Invoke(CurrentRow.DataBoundItem, null);
                                    if (sm.Type == EgxBusiness.MessageType.Fail || sm.Type == EgxBusiness.MessageType.Stop)
                                    {
                                        MessageBox.Show(sm.Message);
                                    }
                                    else 
                                    {

                                        AllowUserToDeleteRows = true; bs.RemoveAt(CurrentRow.Index); AllowUserToDeleteRows = false;
                                    }
                                }
                                else 
                                { throw new Exception("The passed object does not contains any method with the same name "); }
                            }
                            OnAfterAction(new ActionEventArgs() { ActionType = RowAction.Delete, CurrentRow = CurrentRow.Index });
                        }
                    }
                }
            }
            catch (Exception ee) { string m = ee.Message; }
        }

        public void Insert()
        {
            if (!ObjectiveMood)
            {
                RefreshEdit();
                SqlCommand cmd = new SqlCommand();
                string insertQuery = "";
                StringBuilder sb1 = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                int colCnt = SchemaInfo.Columns.Count;
                int cntr = 0;
                string dfn = "";
                if (HiddenFields.Count > 0) 
                {
                    foreach (KeyValuePair<string, string> kvp in HiddenFields) 
                    {
                        sb1.Append(kvp.Key.ToString()+" , ");
                        sb2.Append("'" + kvp.Value.ToString() + "'" + " , ");
                    }
                }
                foreach (DataColumn dc in SchemaInfo.Columns)
                {
                    if (cntr < colCnt - 1)
                    {
                        if (!AIColumns.Contains(dc.ColumnName))
                        {
                            sb1.Append(dc.ColumnName + " , ");
                            sb2.Append("@" + dc.ColumnName + " , ");
                        }
                    }
                    else
                    {
                        if (!AIColumns.Contains(dc.ColumnName))
                        {
                            sb1.Append(dc.ColumnName);
                            sb2.Append("@" + dc.ColumnName);
                        }
                    }
                    if (Columns.Contains(dc.ColumnName) && !AIColumns.Contains(dc.ColumnName))
                    {
                        cmd.Parameters.AddWithValue("@" + dc.ColumnName, this[dc.ColumnName, CurrentRow.Index].Value);
                    }
                    else if (!AIColumns.Contains(dc.ColumnName))
                    {
                        cmd.Parameters.AddWithValue("@" + dc.ColumnName, DBNull.Value);
                    }
                    cntr++;
                }
                insertQuery = "INSERT INTO " + TableName + "(" + sb1.ToString() + ")" + " VALUES " + "(" + sb2.ToString() + ")";
                cmd.Connection = conn;
                cmd.CommandText = insertQuery;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }
            else 
            {
                EgxBusiness.SystemMessage sm = new EgxBusiness.SystemMessage();
                sm = (EgxBusiness.SystemMessage)CurrentRow.DataBoundItem.GetType().GetMethod(InsertMethod).Invoke(CurrentRow.DataBoundItem, null);
                if (sm.Type == EgxBusiness.MessageType.Stop || sm.Type == EgxBusiness.MessageType.Fail)
                {
                    MessageBox.Show(sm.Message);
                }
                else { }
            }
        }

        public void Update()
        {
            if (!ObjectiveMood)
            {
                RefreshEdit();
                SqlCommand cmd = new SqlCommand();
                string updateQuery = "";
                StringBuilder sb1 = new StringBuilder();
                int colCnt = dt.Columns.Count;
                int cntr = 0;
                foreach (DataColumn dc in SchemaInfo.Columns)
                {
                    if (cntr < colCnt - 1)
                    {
                        if (!AIColumns.Contains(dc.ColumnName) && Columns.Contains(dc.ColumnName))
                        {
                            sb1.Append(dc.ColumnName + " = @" + dc.ColumnName + " , ");
                        }
                    }
                    else
                    {
                        if (!AIColumns.Contains(dc.ColumnName) && Columns.Contains(dc.ColumnName))
                        {
                            sb1.Append(dc.ColumnName + " = @" + dc.ColumnName);
                        }
                    }
                    if (Columns.Contains(dc.ColumnName) && !AIColumns.Contains(dc.ColumnName))
                    {
                        cmd.Parameters.AddWithValue("@" + dc.ColumnName, this[dc.ColumnName, CurrentRow.Index].Value);
                    }

                    cntr++;
                }
                int colcnt = DeleteByColumnName.Count;
                StringBuilder PKsBuilder = new StringBuilder();
                for (int i = 0; i < colcnt; i++)
                {
                    if (i < colcnt - 1)
                    {
                        PKsBuilder.Append("[" + PKs[i].ColumnName + "]" + " = " + "'" + this[PKs[i].Index, CurrentRow.Index].Value.ToString() + "'" + " OR " + "[" + DeleteByColumnName[i] + "] IS NULL" + " AND ");
                    }
                    else
                    {
                        PKsBuilder.Append("[" + DeleteByColumnName[i] + "]" + " = " + "'" + this[i, CurrentRow.Index].Value.ToString() + "'" + " OR " + "[" + DeleteByColumnName[i] + "] IS NULL");
                    }
                }
                updateQuery = "UPDATE " + TableName + " SET " + sb1.ToString() + " WHERE " + PKsBuilder.ToString();
                cmd.Connection = conn;
                cmd.CommandText = updateQuery;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }
            else 
            {
                EgxBusiness.SystemMessage sm = new EgxBusiness.SystemMessage();
                sm = (EgxBusiness.SystemMessage)CurrentRow.DataBoundItem.GetType().GetMethod(UpdateMethod).Invoke(CurrentRow.DataBoundItem, null);
                if (sm.Type == EgxBusiness.MessageType.Stop || sm.Type == EgxBusiness.MessageType.Fail)
                {
                    MessageBox.Show(sm.Message);
                }
                else { }
            }
        }

       
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!DesignMode)
            {
                this.CellPainting += EgxGridView_CellPainting;
                this.AllowUserToDeleteRows = false;
                this.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                this.BackgroundColor = System.Drawing.Color.White;
                this.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8F);
                dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DodgerBlue;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Yellow;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                this.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                this.ColumnHeadersHeight = 20;
                this.EnableHeadersVisualStyles = false;
                this.Location = new System.Drawing.Point(12, 127);
                this.RowHeadersVisible = false;
                this.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                if (EgxFeatures)
                {
                    AutoGenerateColumns = true;
                    this.RowValidated += EgxGridView_RowValidated;
                    this.KeyDown += EgxGridView_KeyDown;
                    this.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = AlternateRowColor, ForeColor = AlternateRowForeColor };
                    this.CellDoubleClick += EgxGridView_CellDoubleClick;

                    this.ValidateEvent += EgxGridView_ValidateEvent;
                    errp = new ErrorProvider();

                    if (!DesignMode)
                    {

                        InitializeConnection();
                    }
                }
            }
           
        }
        
        private void EgxGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                Color c1 = Color.White;
                Color c2 = Color.Gray;
                Color c3 = Color.White;
                LinearGradientBrush br = new LinearGradientBrush(e.CellBounds, c1, c3, 90, true);
                ColorBlend cb = new ColorBlend();
                cb.Positions = new[] { 0, (float)0.5, 1 };
                cb.Colors = new[] { c1, c2, c3 };
                br.InterpolationColors = cb;
                e.Graphics.FillRectangle(br, e.CellBounds);
                e.PaintContent(e.ClipBounds);
                e.Handled = true;
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            
            base.OnPaint(pe);
        }
       
    }


    public class ActionEventArgs : EventArgs
    {
        public int CurrentRow { get; set; }
        public RowAction ActionType { get; set; }
        public object ReturnedObject { get; set; }
    }
    public class DictionaryEditor : UITypeEditor 
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorservice = null;
            if (provider != null)
            {
                editorservice = (IWindowsFormsEditorService)
                provider.GetService(typeof(
                IWindowsFormsEditorService));
            }
            if (editorservice != null)
            {
                SelectedFieldsEditor editor = new SelectedFieldsEditor();
                editorservice.ShowDialog(editor);
                return editor.dic;
            }
            else
            {
                return value;
            }
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            base.PaintValue(e);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return base.GetPaintValueSupported(context);
        }

    }
    public class EgxGridViewLookupColumn : DataGridViewTextBoxColumn 
    {
        public string LookupQuery { get; set; }
        public string ValueMember { get; set; }
        public bool Required { get; set; }
        public string DataFieldName { get; set; }
        public EgxGridViewLookupColumn() 
        {
        }
        public override object Clone()
        {
            EgxGridViewLookupColumn copy = base.Clone() as EgxGridViewLookupColumn;
            copy.ValueMember = ValueMember;
            copy.LookupQuery = LookupQuery;
            copy.Required = this.Required;
            copy.DataFieldName = this.DataFieldName;
            copy.Visible = Visible;
            return copy;
        }
    }
    public class EgxTextBoxColumn : DataGridViewTextBoxColumn
    {
        public string DataFieldName { get; set; }
        public bool Required { get; set; }
        public string ValidationRoles { get; set; }
        public TextType Type { get; set; }
        public EgxTextBoxColumn()
        {
        }
        public override object Clone()
        {
            EgxTextBoxColumn tb = base.Clone() as EgxTextBoxColumn;
            tb.Required = this.Required;
            tb.DataFieldName = this.DataFieldName;
            tb.Type = this.Type;
            return tb;
        }
    }
    public class EgxDataGridViewColumn : DataGridViewColumn 
    {
        public string DataFieldName { get; set; }
        public override object Clone() 
        {
            EgxDataGridViewColumn edgvc = base.Clone() as EgxDataGridViewColumn;
            edgvc.DataFieldName = DataFieldName;
            return edgvc;
        }
    }
    public class EgxDataGridViewColumnInfo 
    {
        public string ColumnName { get; set; }
        public int Index { get; set; }
    }
}
