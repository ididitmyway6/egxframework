using Egx.EgxDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Egx.EgxControl
{
    public class EgxGridViewWrapper
    {

        #region Setting Variables
        DataAccess dataAccess;
        private DataTable schema;
        string _condition;
        #endregion
        #region Events
        public delegate void ConditionChage(EventArgs args);
        public delegate void InsertAction(EventArgs args);
        public event InsertAction InsertingEvent;
        public event ConditionChage ConditionChangedEvent;
        public virtual void onCoditionChanged(EventArgs e) 
        {
            if (ConditionChangedEvent != null) 
            {
                ConditionChangedEvent(e);
            }
            if (IsInitialized)
            {
                StartSelectQuery();
            }
        }
        public virtual void onInserting(EventArgs e) 
        {
            if (InsertingEvent != null) 
            {
                InsertingEvent(null);
            }
        }
        #endregion
        public DataGridView CurrentGridView { get; set; }
        public string SqlInsert { get { return PrepareInsertQuery(); } }
        public string SqlSelect { get { return PrepareSelectQuery(); } }
        public string SqlUpdate { get; set; }
        public string SqlDelete { get; set; }
        public string TableName { get; set; }
        public bool IsInitialized { get; set; }
        public DataTable CurrentGridItems { get; set; }
        public string SelectCondition { get { return _condition; } set { _condition = value; onCoditionChanged(null); } }
        private Dictionary<string,string> Fields { get; set; }
        private Dictionary<string, string> HiddenFields { get; set; }
        public Dictionary<string, string> UpdateParameters { get; set; }
        public string UpdateQuery { get; set; }
        public EgxGridViewWrapper() 
        {
            IsInitialized = false;
            Fields = new Dictionary<string, string>();
            HiddenFields = new Dictionary<string, string>();
            TableName = "";
            SelectCondition = "";
            _condition = "";
            UpdateParameters = new Dictionary<string, string>();
            CurrentGridItems = new DataTable();
            dataAccess = new DataAccess();
             IsInitialized = true;
        }
        private void CurrentGridItems_RowChanging(object sender, DataRowChangeEventArgs e)
        {
            onInserting(null);
            if (e.Action == DataRowAction.Add) 
            {
                DataAccess.ExecData(SqlInsert);
            }

        }
        public EgxGridViewWrapper(DataGridView dataGridView) :this()
        {
            this.CurrentGridView = dataGridView;
        }
        public EgxGridViewWrapper(DataGridView dataGridView, string SqlSelect) :this()
        {
            
            this.CurrentGridView = dataGridView;
            CurrentGridView.DataSource = DataAccess.GetDataTable(SqlSelect);
            var cmd =(SqlCommand)DataAccess.GetDbCommand(SqlSelect, EgxDataType.Mssql);
            cmd.Connection = (SqlConnection)DataAccess.GetConnection(DBConfig.ConnectionString, EgxDataType.Mssql);
            cmd.Connection.Open();
            var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
            
            schema = reader.GetSchemaTable();
            cmd.Connection.Close();
            //this.SqlSelect = SqlSelect;
            schema = new DataTable();
            schema = DataAccess.GetTbSchma(SqlSelect);
            
        }
        public string StartSelectQuery() 
        {
            if (CheckWrapperValidations()) 
            {
              CurrentGridItems= DataAccess.GetDataTable(SqlSelect);
              CurrentGridView.DataSource = CurrentGridItems;
              
              CurrentGridItems.RowChanging += CurrentGridItems_RowChanging;

            }
            return SqlSelect;
        }
        private bool CheckWrapperValidations() 
        {
            if (TableName.Trim().Length == 0) { throw new Exception("Invalid or empty TableName "); }
            if (Fields.Count < 0) { throw new Exception("Invalid or empty Fields , Fields Dictionary must have atleast one record <FieldName,Caption> "); }
            return true;
        }
        private string PrepareSelectQuery() 
        {
            if (TableName.Length > 0)
            {
                if (Fields.Count > 0) 
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (KeyValuePair<string,string> kvp in Fields) 
                    {
                        sb.Append(kvp.Key + " AS " + "[" + kvp.Value + "]");
                        if (!kvp.Equals(Fields.Last())) 
                        {
                            sb.Append(",");
                        }
                    }
                    return SelectCondition.Length > 0 ? "SELECT " + sb.ToString() + " FROM " + TableName + " WHERE " + SelectCondition : "SELECT " + sb.ToString() + "FROM " + TableName;
                }
                else { return SelectCondition.Length > 0 ? "select * from " + TableName + " WHERE " + SelectCondition : "select * from "+TableName; }
            }
            else { throw new Exception("يجب اسناد قيمة صحيحة لاسم الجدول"); }
        }
        private string PrepareInsertQuery() 
        {
            if (CheckWrapperValidations())
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                foreach (KeyValuePair<string, string> kvp in Fields)
                { 
                    sb.Append(kvp.Key);
                    sb2.Append("'"+CurrentGridView[kvp.Value,CurrentGridView.CurrentRow.Index].Value.ToString()+"'");
                    if (!kvp.Equals(Fields.Last()))
                    {
                        sb.Append(",");
                        sb2.Append(",");
                    }
                    else 
                    {
                        
                        if (HiddenFields.Count > 0) 
                        {
                            sb.Append(",");
                            sb2.Append(",");
                            foreach (KeyValuePair<string, string> kvp_h in HiddenFields) 
                            {
                                sb.Append(kvp_h.Key);
                                sb2.Append("'"+kvp_h.Value+"'");
                                if (!kvp_h.Equals(HiddenFields.Last()))
                                {
                                    sb.Append(",");
                                    sb2.Append(",");
                                }
                            }
                        }
                    }

                }
                return "INSERT INTO "+TableName+" ("+sb.ToString()+") "+"VALUES ( "+sb2.ToString()+" )";

            }
            else { return null; }
        }
        private string PrepareUpdateQuery() 
        {

            return "";

        }
        public void StartInsertQuery() 
        {

        }
        public void AddHiddenField(string DataField,string Value) 
        {
            if (!HiddenFields.Keys.Contains(DataField))
            {
                HiddenFields.Add(DataField, Value);
            }
            else 
            {
                HiddenFields[DataField] = Value;
            }
        }
        public void AddField(string DataField, string Caption)
        {
            if (!dataAccess.getAutoIdentity(TableName).Contains(DataField))
            {
                if (!Fields.Keys.Contains(DataField))
                {
                    Fields.Add(DataField, Caption);
                }
            }
        }
    }
    
}
 