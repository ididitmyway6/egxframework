/* This Data Access DLL Is Written By Developer // Ziad Ahmed Elnaggar
 * For More Information Contact Me On +20 101 615 5015
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.OleDb;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Egx.EgxDataAccess
{
    public class DataAccess 
    {
        
        #region DLL Variables
        private string __CONSTR;
        private SqlDependency __sql_dep;
        
        int test = 0;
        public static bool __db_swtich_mode = false;
        private DbConnection __CONN;
        private static EgxDataType __OLD_DB_TYPE;
        private static string __OLD_DB_NAME;
        private DbDataReader __Reader = null;
        private DbCommand __Command;
        private List<string> PKs;
        private List<string> Cols;
        List<object> ListOfObjects = new List<object>();
        string[] par;
        string PropType = "";
        private object __obj;
        private int Current=0, First=0, Last=0, BeforeLast = 0;
        public int CurrentIndex
        {
            get;
            set;
        }
        public int GetFirstIndex(string PKName)
        {
            if (NavigationObject != null && PKName != null)
            {
                object result=GetSqlValue("SELECT MIN(" + PKName + ") FROM " + NavigationObject.ToString() + " ") ;
                if (result!=null)
                {
                    return (int)result;
                }
                else { return -1; }
            }
            else { return -1; }
        }
        public int GetLastIndex(string PKName)
        {
            if (NavigationObject != null && PKName != null)
            {
           object result=GetSqlValue("SELECT MAX(" + PKName + ") FROM " + NavigationObject.ToString() + " ");
           if (result != null)
           {
               return (int)result;
           }
           else { return -1; }
            }
            else { return -1; }
        }
        public int GetBeforeLastIndex(string PKName)
        {
            if (NavigationObject != null && PKName != null)
            {
                object result = GetSqlValue("select MAX(" + PKName + ") FROM " + NavigationObject + " where " + PKName + " <(select MAX(" + PKName + ") from " + NavigationObject + ")");
                if (result != null)
                {
                    return (int)result;
                }
                else { return -1; }
            }
            else { return -1; }
        }
        public int GetAfterFirstIndex(string PKName)
        {
            if (NavigationObject != null && PKName != null)
            {
                object result = GetSqlValue("select MIN(" + PKName + ") FROM " + NavigationObject + " where " + PKName + " >(select MIN(" + PKName + ") from " + NavigationObject + ")");
                if(result!=null)
                {
                return (int)result;
                }
                else { return -1; }
            }
            else { return -1; }
        }
        public bool IsLast(string PKName) 
        {
            if (CurrentIndex == GetLastIndex(PKName)) { return true; } else { return false; }
        }
        public bool IsFirst(string PKName) 
        {
            if (CurrentIndex == GetFirstIndex(PKName)) { return true; } else { return false; }
        }
        public bool IsAfterFirst(string PKName)
        {
            if (CurrentIndex == GetAfterFirstIndex(PKName)) { return true; } else { return false; }
        }
        public bool IsBeforeLast(string PKName)
        {
            if (CurrentIndex == GetBeforeLastIndex(PKName)) { return true; } else { return false; }
        }
        public bool IsEmpty(object o) 
        {
          int result=  (int)GetSqlValue("SELECT COUNT(*) FROM "+o.ToString()+"");
          if (result > 0) { return false; } else { return true; }
        }
        public bool IsSingle(object o) 
        {
            int result = (int)GetSqlValue("SELECT COUNT(*) FROM " + o.ToString() + "");
            if (result > 1) { return false; } else { return true; }
        }
       
        [DefaultValue("")]
        public string NavigationCondition { get; set; }
        public object NavigationObject { get; set; }
        #endregion


        #region Constructor
        public DataAccess()
        {
            if (DBConfig.Datatype == EgxDataType.Mssql)
            {
                __CONSTR = "Server='" + DBConfig.ServerName + "';Database='" + DBConfig.DbName + "';User Id='" + DBConfig.UserName + "'; Password='" + DBConfig.Password + "';";
                __CONN = GetConnection(__CONSTR, EgxDataType.Mssql); //new SqlConnection(__CONSTR);
                __Command = GetDbCommand("", EgxDataType.Mssql); //new SqlCommand();
            }
            else if (DBConfig.Datatype == EgxDataType.Msaccess) 
            {
                __CONSTR = DBConfig.ConnectionString;
                __CONN = GetConnection(__CONSTR, EgxDataType.Msaccess); //new SqlConnection(__CONSTR);
                __Command = GetDbCommand("", EgxDataType.Msaccess); //new SqlCommand();

            }
            else if (DBConfig.Datatype == EgxDataType.GLConnection)
            {
                __CONSTR = DBConfig.GLConnectionString;
                __CONN = GetConnection(__CONSTR, EgxDataType.Mssql); //new SqlConnection(__CONSTR);
                __Command = GetDbCommand("", EgxDataType.Mssql); //new SqlCommand();
            }
            CurrentIndex = -1;
            NavigationObject = null;
            NavigationCondition = "";
        }
        public DataAccess(string ServerName,string DbName,string UserName,string Password) 
        {
            if (DBConfig.Datatype == EgxDataType.Mssql)
            {
                __CONSTR = "Server='" + ServerName + "';Database='" + DbName + "';User Id='" + UserName + "'; Password='" + Password + "';";
                __CONN = GetConnection(__CONSTR, EgxDataType.Mssql); //new SqlConnection(__CONSTR);
                __Command = GetDbCommand("", EgxDataType.Mssql); //new SqlCommand();
            }
            else if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONSTR = DBConfig.ConnectionString;
                __CONN = GetConnection(__CONSTR, EgxDataType.Msaccess); //new SqlConnection(__CONSTR);
                __Command = GetDbCommand("", EgxDataType.Msaccess); //new SqlCommand();

            }
            
            CurrentIndex = -1;
            NavigationObject = null;
            NavigationCondition = "";
        }

        #endregion


        #region Prepare Connection
        //Prepare Connection Object and Data Reader 
        public void prepareConnection()
        {
            if (__Reader != null && !__Reader.IsClosed)
            {
                __Reader.Close();
            }
            if (__CONN.State != ConnectionState.Open)
            {
                if (DBConfig.Datatype == EgxDataType.Mssql) { SqlConnection.ClearAllPools(); }
                __CONN.Open();
            }
             __Command = GetDbCommand("", DBConfig.Datatype);

            __Command.Parameters.Clear();
        }
        #endregion

        public static void ChangeDatebase(EgxDataType newDBType,string newDBName) 
        {
            __OLD_DB_TYPE= DBConfig.Datatype;
            __OLD_DB_NAME = DBConfig.DbName;
            DBConfig.Datatype = newDBType;
            DBConfig.DbName = newDBName;
            __db_swtich_mode = true;
        }

        public static void CommitDatabaseChanging() 
        {
           // if (newDBType.Length > 0)
           // {
                DBConfig.Datatype = __OLD_DB_TYPE;
                DBConfig.DbName = __OLD_DB_NAME;
                __db_swtich_mode = false;
         //   }
         //   else { throw new Exception("You have to change database name after commitment"); }
        }

        #region Insert,Update,Delete
        //Insert Into Specific Object that represent Database Table 
        public int Insert(object Obj)
        {
            try
            {
                List<string> columns = getColumns(Obj);
                string sql = "";
                PKs = new List<string>();
                PKs = getAutoIdentity(Obj);
                sql = "INSERT INTO " + Obj.ToString() + "(" + PrepareQueryParameters(Obj, EgxCommandType.Insert)[0] + ") " + " VALUES (" + PrepareQueryParameters(Obj, EgxCommandType.Insert)[1] + ")";
                prepareConnection();
                foreach (PropertyInfo property in Obj.GetType().GetProperties().Where(p => p.GetValue(Obj, null) != null && columns.Exists(c => c.EndsWith(p.Name))))
                {
                    if (PKs.Count == 0) { PKs.Add(""); }
                    foreach (string pk in PKs)
                    {
                        if (pk != property.Name)
                        {
                            if (DBConfig.Datatype == EgxDataType.Mssql)
                            {
                                if (property.GetValue(Obj, null) != null)
                                {
                                    __Command.Parameters.Add(GetDbParameter("@" + property.Name, property.GetValue(Obj, null), DBConfig.Datatype));
                                }
                            }
                            else
                            {
                                // Access Handler for DateTime is here ........
                                if (property.PropertyType == typeof(DateTime?) && property.GetValue(Obj, null) != null)
                                {
                                    __Command.Parameters.Add(GetDbParameter("@" + property.Name, GetOleDateTime(((DateTime?)property.GetValue(Obj, null)).Value), DBConfig.Datatype));
                                }
                                else if (property.GetValue(Obj, null) != null)
                                {
                                    __Command.Parameters.Add(GetDbParameter("@" + property.Name, property.GetValue(Obj, null), DBConfig.Datatype));
                                }
                            }

                        }
                    }
                }
                __Command.CommandType = CommandType.Text;
                __Command.Connection = __CONN;
               __Command.CommandText = sql;
            int res=  __Command.ExecuteNonQuery();
              if (DBConfig.Datatype == EgxDataType.Msaccess)
              {
                  __CONN.Close();
              }

              return res;
                
            }
            catch (Exception exc) { MessageBox.Show(exc.Message, "Insertion Error"); return 0; }
        }

        public DateTime GetOleDateTime(DateTime date) 
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }
        //Update Specific Table from Database
        public void Update(object Obj, string UpdateBy)
        {
            List<string> columns = getColumns(Obj);
            string sql = "";
            sql = "UPDATE " + Obj.ToString() + " SET " + PrepareQueryParameters(Obj, EgxCommandType.Update)[0] + " WHERE " + UpdateBy + "=@" + UpdateBy + " ";
            prepareConnection();
            int PropertyCount = Obj.GetType().GetProperties().Count(p => p.GetValue(Obj, null) != null && columns.Exists(c => c.EndsWith(p.Name)) || (p.PropertyType.IsGenericType && columns.Exists(c => c.EndsWith(p.Name))));
            foreach (PropertyInfo property in Obj.GetType().GetProperties().Where(p => p.GetValue(Obj, null) != null && columns.Exists(c => c.EndsWith(p.Name)) || (p.PropertyType.IsGenericType && columns.Exists(c => c.EndsWith(p.Name)))))
            {
                if (PKs.Count == 0) { PKs.Add(""); }
                foreach (string pk in PKs)
                {
                    //if (pk != property.Name)
                    //{
                    if (property.PropertyType.IsGenericType)
                    {
                        if (Nullable.GetUnderlyingType(property.PropertyType).Name == "DateTime" && property.GetValue(Obj, null) == null)
                        {
                            __Command.Parameters.Add(GetDbParameter("@" + property.Name, DBNull.Value, DBConfig.Datatype));
                        }
                        else if (property.PropertyType == typeof(DateTime?) && property.GetValue(Obj, null) != null && DBConfig.Datatype == EgxDataType.Msaccess)
                        {
                            __Command.Parameters.Add(GetDbParameter("@" + property.Name, GetOleDateTime(((DateTime?)property.GetValue(Obj, null)).Value), DBConfig.Datatype));
                        }
                        else
                        {
                            if (property.GetValue(Obj, null) != null) { __Command.Parameters.Add(GetDbParameter("@" + property.Name, property.GetValue(Obj, null), DBConfig.Datatype)); }
                        }
                    }
                    else
                    {
                        if (property.GetValue(Obj, null) != null) { __Command.Parameters.Add(GetDbParameter("@" + property.Name, property.GetValue(Obj, null), DBConfig.Datatype)); }
                    }


                    //}
                }
                //j
            }
            __Command.CommandType = CommandType.Text;
            __Command.Connection = __CONN;
            __Command.CommandText = sql;
            try
            {
                int res = -1;
                if (DBConfig.Datatype == EgxDataType.Msaccess)
                {
                    ProcessOleDbCommand((OleDbCommand)__Command);
                    res = __Command.ExecuteNonQuery();
                    __CONN.Close();
                }
                else { res = __Command.ExecuteNonQuery(); }
            }
            catch (Exception exc) { }
        }
        public void expUpdate(object Obj,string UpdateBy)
        {
            List<string> columns =getColumns(Obj);
            string sql = "";
            sql = "UPDATE " + Obj.ToString() + " SET " + PrepareQueryParameters(Obj, EgxCommandType.Update)[0] + " WHERE " + UpdateBy + "=@" + UpdateBy + " ";
            PKs = new List<string>();
            PKs.Add("id");
            prepareConnection();
            int PropertyCount = Obj.GetType().GetProperties().Count(p => p.GetValue(Obj, null) != null && columns.Exists(c => c.EndsWith(p.Name)) || (p.PropertyType.IsGenericType && columns.Exists(c => c.EndsWith(p.Name))));
            foreach (PropertyInfo property in Obj.GetType().GetProperties().Where(p => p.GetValue(Obj, null) != null && columns.Exists(c => c.EndsWith(p.Name)) || (p.PropertyType.IsGenericType && columns.Exists(c => c.EndsWith(p.Name)))))
            {
                if (PKs.Count == 0) { PKs.Add(""); }
                foreach (string pk in PKs)
                {
                    //if (pk != property.Name)
                    //{
                    if (property.PropertyType.IsGenericType)
                    {
                        if (Nullable.GetUnderlyingType(property.PropertyType).Name == "DateTime" && property.GetValue(Obj, null) == null)
                        {
                            __Command.Parameters.Add(GetDbParameter("@" + property.Name, DBNull.Value, DBConfig.Datatype));
                        }
                        else if (property.PropertyType == typeof(DateTime?) && property.GetValue(Obj, null) != null && DBConfig.Datatype == EgxDataType.Msaccess)
                        {
                            __Command.Parameters.Add(GetDbParameter("@" + property.Name, GetOleDateTime(((DateTime?)property.GetValue(Obj, null)).Value), DBConfig.Datatype));
                        }
                        else
                        {
                            if (property.GetValue(Obj, null) != null) 
                            {
                                __Command.Parameters.Add(new OleDbParameter("@" + property.Name, property.GetValue(Obj, null)));
                            }
                        }
                    }
                    else
                    {
                        if (property.GetValue(Obj, null) != null) 
                        { 
                            __Command.Parameters.Add(new OleDbParameter("@" + property.Name, property.GetValue(Obj, null))); 
                        }
                    }


                    //}
                }
                //j
            }
            //---------------------------------------------------------------
            __Command.Parameters.Clear();
            __Command.Parameters.Add(new OleDbParameter("@txt", "fffdddff"));
            __Command.Parameters.Add(new OleDbParameter("@id", 111));
            __Command.CommandType = CommandType.Text;
            __Command.Connection = __CONN;
            __Command.CommandText = sql;
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                ProcessOleDbCommand((OleDbCommand)__Command);
            }
            try
            {
                int res = __Command.ExecuteNonQuery();
                if (DBConfig.Datatype == EgxDataType.Msaccess)
                {
                    __CONN.Close();
                }
            }
            catch (Exception exc) { }
        
        }
        public void Update(object Obj, List<string> UpdateBy)
        {
            List<string> columns = getColumns(Obj);
            string updateByParams = "";
            StringBuilder sb = new StringBuilder(updateByParams);
            int cnt = UpdateBy.Count;
            for (int i=0;i<UpdateBy.Count;i++) 
            {
                if (i < UpdateBy.Count - 1) 
                {
                    sb.Append(UpdateBy[i] + "=@" + UpdateBy[i] + " AND  ");
                }
                else
                {
                    sb.Append(UpdateBy[i] + "=@" + UpdateBy[i] + "  ");
                }
            }
            string sql = "";
            sql = "UPDATE " + Obj.ToString() + " SET " + PrepareQueryParameters(Obj, EgxCommandType.Update)[0] + " WHERE "+ sb.ToString() +" ";
            prepareConnection();
            int PropertyCount = Obj.GetType().GetProperties().Count(p => p.GetValue(Obj, null) != null && columns.Exists(c => c.EndsWith(p.Name)) || (p.PropertyType.IsGenericType && columns.Exists(c => c.EndsWith(p.Name))));
            foreach (PropertyInfo property in Obj.GetType().GetProperties().Where(p => p.GetValue(Obj, null) != null && columns.Exists(c => c.EndsWith(p.Name)) || (p.PropertyType.IsGenericType && columns.Exists(c => c.EndsWith(p.Name)))))
            {
                if (PKs.Count == 0) { PKs.Add(""); }
                foreach (string pk in PKs)
                {
                    //if (pk != property.Name)
                    //{
                    if (property.PropertyType.IsGenericType)
                    {
                        if (Nullable.GetUnderlyingType(property.PropertyType).Name == "DateTime" && property.GetValue(Obj, null) == null)
                        {
                            __Command.Parameters.Add(GetDbParameter("@" + property.Name, DBNull.Value, DBConfig.Datatype));
                        }
                        else if (property.PropertyType == typeof(DateTime?) && property.GetValue(Obj, null) != null && DBConfig.Datatype == EgxDataType.Msaccess)
                        {
                            __Command.Parameters.Add(GetDbParameter("@" + property.Name, GetOleDateTime(((DateTime?)property.GetValue(Obj, null)).Value), DBConfig.Datatype));
                        }
                        else
                        {
                            if (property.GetValue(Obj, null) != null) { __Command.Parameters.Add(GetDbParameter("@" + property.Name, property.GetValue(Obj, null), DBConfig.Datatype)); }
                        }
                    }
                    else
                    {
                        if (property.GetValue(Obj, null) != null) { __Command.Parameters.Add(GetDbParameter("@" + property.Name, property.GetValue(Obj, null), DBConfig.Datatype)); }
                    }


                    //}
                }
                //j
            }
            __Command.CommandType = CommandType.Text;
            __Command.Connection = __CONN;
            __Command.CommandText = sql;
            try
            {
                int res = -1;
                if (DBConfig.Datatype == EgxDataType.Msaccess)
                {
                    ProcessOleDbCommand((OleDbCommand)__Command);
                    res = __Command.ExecuteNonQuery();
                    __CONN.Close();
                }
                else { res = __Command.ExecuteNonQuery(); }
            }
            catch (Exception exc) { }
        }
        //Delete From Specific Database Table
        public void Delete(object Obj, string Key)
        {
            string sql = "";
            sql = "DELETE FROM " + Obj.ToString() + " WHERE " + Key + "=@" + Key + " ";
            prepareConnection();
            foreach (PropertyInfo property in Obj.GetType().GetProperties())
            {
                if (property.GetValue(Obj, null) != null && property.Name.ToUpper() == Key.ToUpper())
                {
                    __Command.Parameters.Add(GetDbParameter("@" + property.Name, property.GetValue(Obj, null), DBConfig.Datatype));
                    break;
                }
            }
            __Command.CommandType = CommandType.Text;
            __Command.Connection = __CONN;
            __Command.CommandText = sql;
            int res = __Command.ExecuteNonQuery();
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
        }


        public static void ProcessOleDbCommand(OleDbCommand command)
        {
            //1. Find all occurrences of parameter references in the SQL statement (such as @MyParameter).
            //2. Find the corresponding parameter in the commands parameters list.
            //3. Add the found parameter to the newParameters list and replace the parameter reference in the SQL with a question mark (?).
            //4. Replace the commands parameters list with the newParameters list.

            var newParameters = new List<OleDbParameter>();

            command.CommandText = Regex.Replace(command.CommandText, "(@\\w*)", match =>
            {
                var parameter = command.Parameters.OfType<OleDbParameter>().FirstOrDefault(a => a.ParameterName == match.Groups[1].Value);
                if (parameter != null)
                {
                    var parameterIndex = newParameters.Count;

                    var newParameter = command.CreateParameter();
                    newParameter.OleDbType = parameter.OleDbType;
                    newParameter.ParameterName = "@parameter" + parameterIndex.ToString();
                    newParameter.Value = parameter.Value;

                    newParameters.Add(newParameter);
                }

                return "?";
            });

            command.Parameters.Clear();
            command.Parameters.AddRange(newParameters.ToArray());
        }

        #endregion

        public object ReadReturnedValues(object obj , List<String> extraColumns=null)
        {
            //Last Update
            object o = Activator.CreateInstance(obj.GetType());
            try
            {
                List<string> columns = Cols;
                if (extraColumns!=null) 
                {
                    columns.AddRange(extraColumns);
                }
                foreach (PropertyInfo property in o.GetType().GetProperties())
                {
                    bool x = columns.Exists(c => c.EndsWith(property.Name));
                    if (x && __Reader[property.Name] != DBNull.Value)
                    {
                        if (property.PropertyType.IsGenericType)
                        {
                            PropType = Nullable.GetUnderlyingType(property.PropertyType).Name;
                        }
                        else { PropType = property.PropertyType.Name; }
                        switch (PropType)
                        {

                            case "Int32":
                                property.SetValue(o, int.Parse(__Reader[property.Name].ToString()), null);
                                break;
                            case "Decimal":
                                property.SetValue(o, Decimal.Parse(__Reader[property.Name].ToString()), null);
                                break;
                            case "String":
                                property.SetValue(o, __Reader[property.Name].ToString(), null);
                                break;
                            case "Boolean":
                                property.SetValue(o, bool.Parse(__Reader[property.Name].ToString()), null);
                                break;
                            case "Byte":
                                property.SetValue(o, byte.Parse(__Reader[property.Name].ToString()), null);
                                break;
                            case "DateTime":
                                property.SetValue(o, DateTime.Parse(__Reader[property.Name].ToString()), null);
                                break;
                            case "Single":
                                property.SetValue(o, Single.Parse(__Reader[property.Name].ToString()), null);
                                break;
                            case "Byte[]":
                                property.SetValue(o, (byte[])(__Reader[property.Name]), null);
                                break;
                            case "Double":
                                property.SetValue(o, double.Parse(__Reader[property.Name].ToString()), null);
                                break;
                        }
                    }
                }
            }
            catch (SqlException e) { System.Windows.Forms.MessageBox.Show(e.Message); }
            return o;
        }

        #region Get Object By ID or By Column or Top Items
        //Get Object By ID With The Column Name = ID
        public object GetByID(int ID, object obj)
        {
            getColumns(obj);
            prepareConnection();
            string sql = "SELECT * FROM " + obj.ToString() + " WHERE ID=" + ID + "";
            object o = (object)Activator.CreateInstance(obj.GetType());
            __Command.Connection = __CONN;
            __Command.CommandType = CommandType.Text;
            __Command.CommandText = sql;
            __Reader = __Command.ExecuteReader(CommandBehavior.CloseConnection);
            if (__Reader.Read())
            {
                #region main
                //foreach (PropertyInfo property in o.GetType().GetProperties())
                //{
                //    bool x = columns.Exists(c => c.EndsWith(property.Name));
                //    if (x)
                //    {
                //        if (property.PropertyType.IsGenericType)
                //        {
                //            PropType = Nullable.GetUnderlyingType(property.PropertyType).Name;
                //        }
                //        else { PropType = property.PropertyType.Name; }
                //        switch (PropType)
                //        {
                //            case "Int32":
                //                property.SetValue(o, int.Parse(__Reader[property.Name].ToString()));
                //                break;
                //            case "String":
                //                property.SetValue(o, __Reader[property.Name].ToString());
                //                break;
                //            case "Boolean":
                //                property.SetValue(o, bool.Parse(__Reader[property.Name].ToString()));
                //                break;
                //            case "Byte":
                //                property.SetValue(o, byte.Parse(__Reader[property.Name].ToString()));
                //                break;
                //            case "DateTime":
                //                property.SetValue(o, DateTime.Parse(__Reader[property.Name].ToString()));
                //                break;
                //            case "Single":
                //                property.SetValue(o, Single.Parse(__Reader[property.Name].ToString()));
                //                break;
                //            case "Byte[]":
                //                property.SetValue(o, (byte[])(__Reader[property.Name]));
                //                break;
                //            case "Double":
                //                property.SetValue(o, double.Parse(__Reader[property.Name].ToString()));
                //                break;
                //        }
                //    }
                //}
                #endregion
                return ReadReturnedValues(obj);
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return null;
        }
        //Get Object By ID With The Column Name =IDKeyName
        public object GetByColumn(string ColumnName, object Value, object obj, string strOperator = null)
        {
            List<string> columns = getColumns(obj);
            prepareConnection();
            string sql;
            switch (strOperator)
            {
                case null:
                    sql = "SELECT TOP 1 * FROM " + obj.ToString() + " WHERE " + ColumnName + "='" + Value + "'";
                    break;
                case "like":
                    sql = "SELECT TOP 1 * FROM " + obj.ToString() + " WHERE " + ColumnName + " LIKE '%" + Value + "%'";
                    break;
                case ">":
                    sql = "SELECT TOP 1 * FROM " + obj.ToString() + " WHERE " + ColumnName + ">'" + Value + "'";
                    break;
                case "<":
                    sql = "SELECT TOP 1 * FROM " + obj.ToString() + " WHERE " + ColumnName + "<'" + Value + "'";
                    break;
                case ">=":
                    sql = "SELECT TOP 1 * FROM " + obj.ToString() + " WHERE " + ColumnName + ">='" + Value + "'";
                    break;
                case "<=":
                    sql = "SELECT TOP 1 * FROM " + obj.ToString() + " WHERE " + ColumnName + "<='" + Value + "'";
                    break;
                default:
                    sql = "SELECT TOP 1 * FROM " + obj.ToString() + " WHERE " + ColumnName + "='" + Value + "'";
                    break;
            }
            object o = (object)Activator.CreateInstance(obj.GetType());
            __Command.Connection = __CONN;
            __Command.CommandType = CommandType.Text;
            __Command.CommandText = sql;
            __Reader = __Command.ExecuteReader();

            if (__Reader.Read())
            {
                #region Main
                //foreach (PropertyInfo property in o.GetType().GetProperties())
                //{
                //    bool x = columns.Exists(c=>c.EndsWith(property.Name));
                //    if (x)
                //    {
                //        if (property.PropertyType.IsGenericType)
                //        {
                //            PropType = Nullable.GetUnderlyingType(property.PropertyType).Name;
                //        }
                //        else { PropType = property.PropertyType.Name; }
                //        switch (PropType)
                //        {
                //            case "Int32":
                //                property.SetValue(o, int.Parse(__Reader[property.Name].ToString()));
                //                break;
                //            case "String":
                //                property.SetValue(o, __Reader[property.Name].ToString());
                //                break;
                //            case "Boolean":
                //                property.SetValue(o, bool.Parse(__Reader[property.Name].ToString()));
                //                break;
                //            case "Byte":
                //                property.SetValue(o, byte.Parse(__Reader[property.Name].ToString()));
                //                break;
                //            case "DateTime":
                //                property.SetValue(o, DateTime.Parse(__Reader[property.Name].ToString()));
                //                break;
                //            case "Single":
                //                property.SetValue(o, Single.Parse(__Reader[property.Name].ToString()));
                //                break;
                //            case "Byte[]":
                //                property.SetValue(o, (byte[])(__Reader[property.Name]));
                //                break;
                //            case "Double":
                //                property.SetValue(o, double.Parse(__Reader[property.Name].ToString()));
                //                break;
                //        }
                //    }
                //}
                #endregion
                return ReadReturnedValues(obj);
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return null;
        }
       
            public object GetByColumnCondition(string ColumnName, object Value, object obj,string condition,string strOperator=null)
        {
            List<string> columns = getColumns(obj);
            prepareConnection();
            string sql ;
            switch (strOperator) 
            {
                case null :
                    sql = "SELECT TOP 1 * FROM " + obj.ToString() + " WHERE " + ColumnName + "='" + Value + "' AND "+condition+"";
                    break;
                case "like":
                    sql = "SELECT TOP 1 * FROM " + obj.ToString() + " WHERE " + ColumnName + "LIKE '%" + Value + "%' AND " + condition + "";
                    break;
                case ">":
                    sql = "SELECT TOP 1 * FROM " + obj.ToString() + " WHERE " + ColumnName + ">'" + Value + "' AND " + condition + "";
                    break;
                case "<":
                    sql = "SELECT TOP 1 * FROM " + obj.ToString() + " WHERE " + ColumnName + "<'" + Value + "' AND " + condition + "";
                    break;
                case ">=":
                    sql = "SELECT TOP 1 * FROM " + obj.ToString() + " WHERE " + ColumnName + ">='" + Value + "' AND " + condition + "";
                    break;
                case "<=":
                    sql = "SELECT TOP 1 * FROM " + obj.ToString() + " WHERE " + ColumnName + "<='" + Value + "' AND " + condition + "";
                    break;
                default :
                    sql = "SELECT TOP 1 * FROM " + obj.ToString() + " WHERE " + ColumnName + "='" + Value + "' AND " + condition + "";
                    break;
            }
            object o = (object)Activator.CreateInstance(obj.GetType());
            __Command.Connection = __CONN;
            __Command.CommandType = CommandType.Text;
            __Command.CommandText = sql;
            __Reader = __Command.ExecuteReader();

            if (__Reader.Read())
            {
                #region Main
                //foreach (PropertyInfo property in o.GetType().GetProperties())
                //{
                //    bool x = columns.Exists(c=>c.EndsWith(property.Name));
                //    if (x)
                //    {
                //        if (property.PropertyType.IsGenericType)
                //        {
                //            PropType = Nullable.GetUnderlyingType(property.PropertyType).Name;
                //        }
                //        else { PropType = property.PropertyType.Name; }
                //        switch (PropType)
                //        {
                //            case "Int32":
                //                property.SetValue(o, int.Parse(__Reader[property.Name].ToString()));
                //                break;
                //            case "String":
                //                property.SetValue(o, __Reader[property.Name].ToString());
                //                break;
                //            case "Boolean":
                //                property.SetValue(o, bool.Parse(__Reader[property.Name].ToString()));
                //                break;
                //            case "Byte":
                //                property.SetValue(o, byte.Parse(__Reader[property.Name].ToString()));
                //                break;
                //            case "DateTime":
                //                property.SetValue(o, DateTime.Parse(__Reader[property.Name].ToString()));
                //                break;
                //            case "Single":
                //                property.SetValue(o, Single.Parse(__Reader[property.Name].ToString()));
                //                break;
                //            case "Byte[]":
                //                property.SetValue(o, (byte[])(__Reader[property.Name]));
                //                break;
                //            case "Double":
                //                property.SetValue(o, double.Parse(__Reader[property.Name].ToString()));
                //                break;
                //        }
                //    }
                //}
                #endregion
                return ReadReturnedValues(obj);
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return null;
        }
        public List<object> GetTopItems(int N, object o)
        {
            getColumns(o);
            List<object> res = new List<object>();
            prepareConnection();
            __Command = GetDbCommand("", DBConfig.Datatype);
            __Command.CommandType = CommandType.Text;
            __Command.Connection = __CONN;
            __Command.CommandText = "SELECT TOP " + N + " * from " + o.ToString() + "";
            __Reader = __Command.ExecuteReader();
            while (__Reader.Read())
            {
                res.Add(ReadReturnedValues(o));
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return res;
        }
        #endregion
       
        #region Searching for Objects
        //Search in specific table With Specific Properties 
        public List<object> Search(object obj)
        {
            List<string> columns = getColumns(obj);
            object o = new object();
            string sql = "SELECT * FROM " + obj.ToString() + " WHERE " + PrepareQueryParameters(obj, EgxCommandType.Search)[0] + "";
            ListOfObjects.Clear();
            prepareConnection();
            __Command.CommandType = CommandType.Text;
            __Command.Connection = __CONN;
            foreach (PropertyInfo p in obj.GetType().GetProperties())
            {
                if (p.GetValue(obj, null) != null && !p.GetValue(obj, null).ToString().Equals(null))
                {
                    bool x = columns.Exists(c => c.EndsWith(p.Name));
                    if (x)
                    {
                        __Command.Parameters.Add(GetDbParameter("@" + p.Name, p.GetValue(obj, null), DBConfig.Datatype));
                    }
                }
            }
            if (__Command.Parameters.Count == 0) { sql = "SELECT * FROM " + obj.ToString() + ""; }
            __Command.CommandText = sql;
            __Reader = __Command.ExecuteReader();
            if (__Reader != null)
                while (__Reader.Read())
                {
                    #region Main
                    //o =(object)Activator.CreateInstance(obj.GetType());
                    //foreach (PropertyInfo property in o.GetType().GetProperties())
                    //{
                    //    if (property.PropertyType.IsGenericType)
                    //    {
                    //        PropType = Nullable.GetUnderlyingType(property.PropertyType).Name;
                    //    }
                    //    else { PropType = property.PropertyType.Name; }
                    //    switch (PropType)
                    //    {
                    //        case "Int32":
                    //            property.SetValue(o, int.Parse(__Reader[property.Name].ToString()));
                    //            break;
                    //        case "String":
                    //            property.SetValue(o, __Reader[property.Name].ToString());
                    //            break;
                    //        case "Boolean":
                    //            property.SetValue(o, bool.Parse(__Reader[property.Name].ToString()));
                    //            break;
                    //        case "Byte":
                    //            property.SetValue(o, byte.Parse(__Reader[property.Name].ToString()));
                    //            break;
                    //        case "DateTime":
                    //            property.SetValue(o, DateTime.Parse(__Reader[property.Name].ToString()));
                    //            break;
                    //        case "Single":
                    //            property.SetValue(o, Single.Parse(__Reader[property.Name].ToString()));
                    //            break;
                    //        case "Byte[]":
                    //            property.SetValue(o, (byte[])(__Reader[property.Name]));
                    //            break;
                    //        case "Double":
                    //            property.SetValue(o, double.Parse(__Reader[property.Name].ToString()));
                    //            break;
                    //    }
                    //}

                    #endregion
                    ListOfObjects.Add(ReadReturnedValues(obj));
                }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return ListOfObjects;
        }

        public List<object> Search(object obj, string Condition)
        {
            List<string> columns = getColumns(obj);
            object o = new object();
            string sql = "SELECT * FROM " + obj.ToString() + " WHERE " + PrepareQueryParameters(obj, EgxCommandType.Search)[0] + "";
            ListOfObjects.Clear();
            prepareConnection();
            __Command.CommandType = CommandType.Text;
            __Command.Connection = __CONN;
            foreach (PropertyInfo p in obj.GetType().GetProperties())
            {
                if (p.GetValue(obj, null) != null && !p.GetValue(obj, null).ToString().Equals(null))
                {
                    bool x = columns.Exists(c => c.EndsWith(p.Name));
                    if (x)
                    {
                        __Command.Parameters.Add(GetDbParameter("@" + p.Name, p.GetValue(obj, null), DBConfig.Datatype));
                    }
                }
            }
            if (__Command.Parameters.Count == 0) { sql = "SELECT * FROM " + obj.ToString() + ""; }
            //Conditional Search ................................
            if (!Condition.Trim().Equals(null)) { sql += Condition; }
            __Command.CommandText = sql;
            __Reader = __Command.ExecuteReader();
            if (__Reader != null)
                while (__Reader.Read())
                {
                    #region Main
                    //o =(object)Activator.CreateInstance(obj.GetType());
                    //foreach (PropertyInfo property in o.GetType().GetProperties())
                    //{
                    //    if (property.PropertyType.IsGenericType)
                    //    {
                    //        PropType = Nullable.GetUnderlyingType(property.PropertyType).Name;
                    //    }
                    //    else { PropType = property.PropertyType.Name; }
                    //    switch (PropType)
                    //    {
                    //        case "Int32":
                    //            property.SetValue(o, int.Parse(__Reader[property.Name].ToString()));
                    //            break;
                    //        case "String":
                    //            property.SetValue(o, __Reader[property.Name].ToString());
                    //            break;
                    //        case "Boolean":
                    //            property.SetValue(o, bool.Parse(__Reader[property.Name].ToString()));
                    //            break;
                    //        case "Byte":
                    //            property.SetValue(o, byte.Parse(__Reader[property.Name].ToString()));
                    //            break;
                    //        case "DateTime":
                    //            property.SetValue(o, DateTime.Parse(__Reader[property.Name].ToString()));
                    //            break;
                    //        case "Single":
                    //            property.SetValue(o, Single.Parse(__Reader[property.Name].ToString()));
                    //            break;
                    //        case "Byte[]":
                    //            property.SetValue(o, (byte[])(__Reader[property.Name]));
                    //            break;
                    //        case "Double":
                    //            property.SetValue(o, double.Parse(__Reader[property.Name].ToString()));
                    //            break;
                    //    }
                    //}

                    #endregion
                    ListOfObjects.Add(ReadReturnedValues(obj));
                }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return ListOfObjects;
        }
        #endregion

        public bool IsExists(object obj)
        {
            object o = new object();
            string sql = "SELECT * FROM " + obj.ToString() + " WHERE " + PrepareQueryParameters(obj, EgxCommandType.Search)[0] + "";
            ListOfObjects.Clear();
            prepareConnection();
            __Command.CommandType = CommandType.Text;
            __Command.Connection = __CONN;
            foreach (PropertyInfo p in obj.GetType().GetProperties())
            {
                if (p.GetValue(obj, null) != null && p.GetValue(obj, null).ToString() != "0")
                {
                    __Command.Parameters.Add(GetDbParameter("@" + p.Name, p.GetValue(obj, null), DBConfig.Datatype));
                }
            }
            if (__Command.Parameters.Count == 0) { sql = "SELECT * FROM " + obj.ToString() + ""; }
            __Command.CommandText = sql;
            __Reader = __Command.ExecuteReader();
            if (__Reader != null && __Reader.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
        }



        #region Prepare Query Parameters
        //Returned array has 2 values first one is Parameter and second is ParamertersValue.
        private string[] PrepareQueryParameters(object o, EgxCommandType CommandType)
        {
            List<string> columns = getColumns(o);
            PKs = new List<string>();
            PKs.Clear();
            PKs = getAutoIdentity(o);
            par = new string[2];
            Array.Clear(par, 0, 2);
            int cnt = 0;
            StringBuilder ParametersValues = new StringBuilder();
            StringBuilder Parameters = new StringBuilder();
            int PropertyCount = o.GetType().GetProperties().Where(p => p.GetValue(o, null) != null && columns.Exists(c => c.EndsWith(p.Name))).Count();
            #region Insertion
            if (CommandType == EgxCommandType.Insert)
            {
                foreach (PropertyInfo property in o.GetType().GetProperties().Where(p => p.GetValue(o, null) != null && columns.Exists(c => c.EndsWith(p.Name))))
                {
                    cnt++;
                    if (PKs.Count == 0) { PKs.Add(""); }
                    foreach (string pk in PKs)
                    {

                        if (pk != property.Name)
                        {
                            if (cnt < PropertyCount)
                            {
                                if (property.GetValue(o, null) != null)
                                {

                                    ParametersValues.Append("@" + property.Name + ",");
                                    Parameters.Append(property.Name + ",");
                                }
                            }
                            else
                            {
                                if (property.GetValue(o, null) != null)
                                {
                                    bool x = columns.Exists(c => c.EndsWith(property.Name));
                                    if (x)
                                    {
                                        ParametersValues.Append("@" + property.Name + "");
                                        Parameters.Append(property.Name);
                                    }
                                }
                            }
                        }
                    }
                }
                par[0] = Parameters.ToString();
                par[1] = ParametersValues.ToString();

            }
            #endregion
            #region Updating
            else if (CommandType == EgxCommandType.Update)
            {
                //---
                PropertyCount = o.GetType().GetProperties().Where(p => p.GetValue(o, null) != null && columns.Exists(c => c.EndsWith(p.Name)) || (p.PropertyType.IsGenericType && columns.Exists(c => c.EndsWith(p.Name)))).Count();
                foreach (PropertyInfo property in o.GetType().GetProperties().Where(p => p.GetValue(o, null) != null && columns.Exists(c => c.EndsWith(p.Name)) || (p.PropertyType.IsGenericType && columns.Exists(c => c.EndsWith(p.Name)))))
                {

                    cnt++;
                    if (PKs.Count == 0) { PKs.Add(""); }
                    foreach (string pk in PKs)
                    {
                        if (pk != property.Name)
                        {
                            if (cnt < PropertyCount)
                            {
                                if (property.PropertyType.IsGenericType)
                                {
                                    if (Nullable.GetUnderlyingType(property.PropertyType).Name == "DateTime")
                                    { Parameters.Append("" + property.Name + "=@" + property.Name + ","); }
                                    else
                                    {
                                        if (property.GetValue(o, null) != null) { Parameters.Append("" + property.Name + "=@" + property.Name + ","); }
                                    }
                                }
                                else
                                {
                                    if (property.GetValue(o, null) != null) { Parameters.Append("" + property.Name + "=@" + property.Name + ","); }
                                }

                            }
                            else
                            {
                                if (property.PropertyType.IsGenericType)
                                {
                                    if (Nullable.GetUnderlyingType(property.PropertyType).Name == "DateTime")
                                    { Parameters.Append("" + property.Name + "=@" + property.Name + ""); }
                                    else
                                    {
                                        if (property.GetValue(o, null) != null) { Parameters.Append("" + property.Name + "=@" + property.Name + ""); }
                                    }
                                }
                                else
                                {
                                    if (property.GetValue(o, null) != null) { Parameters.Append("" + property.Name + "=@" + property.Name + ""); }
                                }
                            }
                        }
                    }
                }
                if (Parameters.ToString().Trim().EndsWith(","))
                {
                    par[0] = Parameters.ToString().Trim().Remove(Parameters.ToString().Trim().Length - 1).ToString();
                }
                else { par[0] = Parameters.ToString(); }
                
                par[1] = "";
            }
            #endregion
            #region Searching
            else if (CommandType == EgxCommandType.Search)
            {
                foreach (PropertyInfo property in o.GetType().GetProperties().Where(p => p.GetValue(o, null) != null && columns.Exists(c => c.EndsWith(p.Name))))
                {
                    cnt++;
                  //  if (PKs.Count == 0) { PKs.Add(""); };//This line add to solve PK Empty value proplem by adding dummy value to list
                   // foreach (string pk in PKs)
                   // {
                     //   if (pk != property.Name)
                     //   {
                            if (cnt < PropertyCount)
                            {
                                if (property.GetValue(o, null) != null)
                                {
                                    bool x = columns.Exists(c => c.EndsWith(property.Name));
                                    if (x)
                                    {
                                        //Here new condition for new EgxDateTime Handling Class


                                        Parameters.Append("" + property.Name + "=@" + property.Name + " AND ");
                                    }
                                }
                            }
                            else
                            {
                                if (property.GetValue(o, null) != null)
                                {
                                    bool x = columns.Exists(c => c.EndsWith(property.Name));
                                    if (x)
                                    {
                                        Parameters.Append("" + property.Name + "=@" + property.Name + "");
                                    }
                                }
                            }
                     //   }
                 //   }
                }
                par[0] = Parameters.ToString();
                par[1] = "";
            }
            #endregion
            return par;
        }
        #endregion


        #region Extra Functions

        public List<object> ExecuteSystemQuery<T>(SystemQuery Query)
        {
            object o = new object();
            List<object> Result = new List<object>();
            prepareConnection();
            __Command.Connection = __CONN;
            __Command.CommandType = CommandType.Text;
            __Command.CommandText = Query.Query;
            #region SELECTING
            if (Query.QType == QueryType.Select)
            {
                __Reader = __Command.ExecuteReader();
                if (__Reader != null)
                    while (__Reader.Read())
                    {
                        o = (object)Activator.CreateInstance(typeof(T));
                        foreach (PropertyInfo property in o.GetType().GetProperties())
                        {
                            if (property.PropertyType.IsGenericType)
                            {
                                PropType = Nullable.GetUnderlyingType(property.PropertyType).Name;
                            }
                            else { PropType = property.PropertyType.Name; }
                            switch (PropType)
                            {
                                case "Int32":
                                    property.SetValue(o, int.Parse(__Reader[property.Name].ToString()), null);
                                    break;
                                case "String":
                                    property.SetValue(o, __Reader[property.Name].ToString(), null);
                                    break;
                                case "Boolean":
                                    property.SetValue(o, bool.Parse(__Reader[property.Name].ToString()), null);
                                    break;
                                case "Byte":
                                    property.SetValue(o, byte.Parse(__Reader[property.Name].ToString()), null);
                                    break;
                                case "DateTime":
                                    property.SetValue(o, DateTime.Parse(__Reader[property.Name].ToString()), null);
                                    break;
                                case "Single":
                                    property.SetValue(o, Single.Parse(__Reader[property.Name].ToString()), null);
                                    break;
                                case "Byte[]":
                                    property.SetValue(o, (byte[])(__Reader[property.Name]), null);
                                    break;
                                case "Double":
                                    property.SetValue(o, double.Parse(__Reader[property.Name].ToString()), null);
                                    break;
                            }
                        }

                        Result.Add(o);
                    }
                return Result;
            }
            #endregion
            else
            {
                return null;
            }
        }
        //Execute Custom Sql Statment With Custom Returns...
        public DbDataReader ExecuteSQL(string sql)
        {
            try
            {
                List<object> obj = new List<object>();
                prepareConnection();
                __Command.Connection = __CONN;
                __Command.CommandType = CommandType.Text;
                __Command.CommandText = sql;
                __Reader = __Command.ExecuteReader();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "");
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return __Reader;
        }
        public List<T> ExecuteSQL<T>(string Sql,List<String> extraColumns=null)
        {

            object obj = Activator.CreateInstance(typeof(T));
            List<T> res = new List<T>();
            List<string> pks = getColumns(obj);
            prepareConnection();
            __Command = GetDbCommand("", DBConfig.Datatype);
            __Command.CommandType = CommandType.Text;
            __Command.Connection = __CONN;
            __Command.CommandText = Sql;
            try
            {
                __Reader = __Command.ExecuteReader();
            }
            catch (Exception ex) { string msg = ex.Message; }
            while (__Reader.Read())
            {
                res.Add((T)ReadReturnedValues(obj,extraColumns));
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return res;
        }
        public List<object> ExecuteSQL(string Sql,object o)
        {

            object obj = Activator.CreateInstance(o.GetType());
            List<object> res = new List<object>();
            List<string> pks = getColumns(obj);
            prepareConnection();
            __Command = GetDbCommand("", DBConfig.Datatype);
            __Command.CommandType = CommandType.Text;
            __Command.Connection = __CONN;
            __Command.CommandText = Sql;
            try
            {
                __Reader = __Command.ExecuteReader();
            }
            catch (Exception ex) { string msg = ex.Message; }
            while (__Reader.Read())
            {
                res.Add(ReadReturnedValues(obj));
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
           // __CONN.Close();
            return res;
        }
        //Get Primary Keys in the current table
        public List<string> getAutoIdentity(object O)
        {
            List<string> PKeys = new List<string>();
            string sql = "select c.name from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 AND o.name='" + O.ToString() + "'";
            string ColName = null;
            if (DBConfig.Datatype == EgxDataType.Mssql)
            {
                prepareConnection();
                __Command = GetDbCommand("", DBConfig.Datatype);
                __Command.CommandType = CommandType.Text;
                __Command.Connection = __CONN;
                __Command.CommandText = sql;
                __Reader = __Command.ExecuteReader(CommandBehavior.CloseConnection);
                while (__Reader.Read())
                {
                    ColName = __Reader[0].ToString();
                    PKeys.Add(ColName);
                }
            }
            else if(DBConfig.Datatype == EgxDataType.Msaccess)
            {
                prepareConnection();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM "+O.ToString() + " WHERE 1=2", (OleDbConnection)__CONN);
                DataTable dtc = da.FillSchema(new DataTable(), SchemaType.Source);
                foreach (DataColumn dc in dtc.Columns) 
                {
                    if (dc.AutoIncrement) { PKeys.Add(dc.ColumnName); }
                }
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return PKeys;
        }

        public List<string> getAutoIdentity(string TableName)
        {
            List<string> PKeys = new List<string>();
            string sql = "select c.name from sys.objects o inner join sys.columns c on o.object_id = c.object_id where c.is_identity = 1 AND o.name='" + TableName + "'";
            string ColName = null;
            prepareConnection();
            __Command = GetDbCommand("", DBConfig.Datatype);
            __Command.CommandType = CommandType.Text;
            __Command.Connection = __CONN;
            __Command.CommandText = sql;
            __Reader = __Command.ExecuteReader(CommandBehavior.CloseConnection);
            while (__Reader.Read())
            {
                ColName = __Reader[0].ToString();
                PKeys.Add(ColName);
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return PKeys;
        }

        public List<string> getColumns(object O)
        {
            Cols = new List<string>();
            Cols.Clear();
            string sql ="";
            if (DBConfig.Datatype == EgxDataType.Mssql || DBConfig.Datatype== EgxDataType.GLConnection)
            {
                sql = "select COLUMN_NAME from information_schema.columns where table_name = '" + O.ToString() + "'";
                string ColName = null;
                prepareConnection();
                __Command = GetDbCommand("", DBConfig.Datatype);
                __Command.CommandType = CommandType.Text;
                __Command.Connection = __CONN;
                __Command.CommandText = sql;
                __Reader = __Command.ExecuteReader(CommandBehavior.CloseConnection);
                while (__Reader.Read())
                {
                    ColName = __Reader[0].ToString();
                    Cols.Add(ColName);
                }
               
            }
            else 
            {
                prepareConnection();
                DataTable st = (__CONN as OleDbConnection).GetOleDbSchemaTable( OleDbSchemaGuid.Columns, new string[]{null,null,O.ToString(),null});
                foreach (DataRow dr in st.Rows) 
                {
                    Cols.Add(dr["COLUMN_NAME"].ToString());
                }
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return Cols;
        }

        public List<string> GetPrimaryKeys(string TableName)
        {
            Cols = new List<string>();
            Cols.Clear();
            if (DBConfig.Datatype == EgxDataType.Mssql)
            {
                string sql = "SELECT column_name FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 AND table_name = '" + TableName + "'";
                string ColName = null;
                prepareConnection();
                __Command = GetDbCommand("", DBConfig.Datatype);
                __Command.CommandType = CommandType.Text;
                __Command.Connection = __CONN;
                __Command.CommandText = sql;
                __Reader = __Command.ExecuteReader(CommandBehavior.CloseConnection);
                while (__Reader.Read())
                {
                    ColName = __Reader[0].ToString();
                    Cols.Add(ColName);
                }
            }
            else if (DBConfig.Datatype == EgxDataType.Msaccess) 
            {
                prepareConnection();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM " + TableName + " WHERE 1=2", (OleDbConnection)__CONN);
                DataTable dtc = da.FillSchema(new DataTable(), SchemaType.Source);
                foreach (DataColumn dc in dtc.Columns)
                {
                    if (dc.AutoIncrement) { Cols.Add(dc.ColumnName); }
                }
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return Cols;
        }

        public int GetLatest(string table, string column,string condition)
        {
            int val = 0;
            string sql = "SELECT MAX("+column+") FROM " + table + " WHERE "+ condition+"";
            prepareConnection();
            __Command = GetDbCommand("", DBConfig.Datatype);
            __Command.Connection = __CONN;
            __Command.CommandType = CommandType.Text;
            __Command.CommandText = sql;
            __Reader = __Command.ExecuteReader(CommandBehavior.CloseConnection);
            if (__Reader != null && __Reader.Read())
            {
                val = int.Parse(__Reader[0].ToString());
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return val;
        }


        public static int GetLatest(string table, string column) 
        {
            string sql = "SELECT ISNULL(MAX(" + column + "),-1) FROM " + table + "";
            return (int)DataAccess.GetSqlValue(sql, DBConfig.Datatype);
        }

        //public int GetLatest(string table,string column)
        //{
        //    int val = 0;
        //    string sql = "SELECT ISNULL(MAX("+column+"),0) FROM " + table + "";
        //    prepareConnection();
        //    __Command = GetDbCommand("", DBConfig.Datatype);
        //    __Command.Connection = __CONN;
        //    __Command.CommandType = CommandType.Text;
        //    __Command.CommandText = sql;
        //    __Reader = __Command.ExecuteReader(CommandBehavior.CloseConnection);
        //    if (__Reader != null && __Reader.Read())
        //    {
        //        val = int.Parse(__Reader[0].ToString());
        //    }
        //    return val;
        //}

        public int GetLatestID(string table)
        {
            int val = 0;
            string sql = "SELECT MAX(ID) FROM " + table + "";
            prepareConnection();
            __Command = GetDbCommand("", DBConfig.Datatype);
            __Command.Connection = __CONN;
            __Command.CommandType = CommandType.Text;
            __Command.CommandText = sql;
            __Reader = __Command.ExecuteReader(CommandBehavior.CloseConnection);
            if (__Reader != null && __Reader.Read())
            {
                val = int.Parse(__Reader[0].ToString());
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return val;
        }

        public int GetLatestID(object O)
        {
            int val = 0;
            string sql = "SELECT MAX(ID) FROM " + O.ToString() + "";
            prepareConnection();
            __Command = GetDbCommand("", DBConfig.Datatype);
            __Command.Connection = __CONN;
            __Command.CommandType = CommandType.Text;
            __Command.CommandText = sql;
            __Reader = __Command.ExecuteReader(CommandBehavior.CloseConnection);
            if (__Reader != null && __Reader.Read())
            {
                val = int.Parse(__Reader[0].ToString());
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return val;
        }
        public int GetLatestID(object O, string PKeyName)
        {
            int val = 0;
            string sql = "SELECT MAX(" + PKeyName + ") FROM " + O.ToString() + "";
            prepareConnection();
            __Command = GetDbCommand("", DBConfig.Datatype);
            __Command.Connection = __CONN;
            __Command.CommandType = CommandType.Text;
            __Command.CommandText = sql;
            __Reader = __Command.ExecuteReader(CommandBehavior.CloseConnection);
            if (__Reader != null && __Reader.Read())
            {
                if (__Reader[0] != DBNull.Value)
                {
                    val = int.Parse(__Reader[0].ToString());
                }
                else { val = 0; }
            }
            if (DBConfig.Datatype == EgxDataType.Msaccess)
            {
                __CONN.Close();
            }
            return val;
        }
        public int GetNextID(object O, string PKeyName) 
        {
            return GetLatestID(O, PKeyName) + 1;
        }

        public object ExecuteCustomConnection(string connectionString, string Sql , bool IsQuery ) 
        {
            
            using (SqlConnection conn = new SqlConnection(connectionString)) 
            {
                if (IsQuery)
                {
                    SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                else 
                {
                    SqlCommand cmd = new SqlCommand(Sql, conn);
                    cmd.CommandType = CommandType.Text;
                    int i = cmd.ExecuteNonQuery();
                    return i;
                }
            }

        }
        #endregion


        private Dictionary<string, string> GetTbSchma(object o)
        {
            var dt = new DataTable();
            dt.FillSchmaBySql(" Select * from {0}".Frmt(o.ToString()));
            
            return dt.PrimaryKey.ToDictionary(column => column.ColumnName, column => column.DataType.Name);
        }

        public static DataTable GetTbSchma(string sql)
        {
            
            var dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql,(SqlConnection)GetConnection(DBConfig.ConnectionString, EgxDataType.Mssql));
            da.FillSchema(dt, SchemaType.Source);

            //dt.FillBySql(sql);
             
             return dt;
        }
        public object Move(EgxMoveType moveType,string PKName) 
        {
            try
            {
                if (NavigationObject != null)
                {
                    if (NavigationObject.GetType().GetProperty(PKName).GetValue(NavigationObject, null) != null)
                    {
                        CurrentIndex = (int)NavigationObject.GetType().GetProperty(PKName).GetValue(NavigationObject, null);
                    }
                    else { CurrentIndex = GetFirstIndex(PKName); }
                    if (CurrentIndex >= GetFirstIndex(PKName) && CurrentIndex <= GetLastIndex(PKName) && CurrentIndex != -1)
                    {
                        if (moveType == EgxMoveType.First)
                        {
                            NavigationObject = ExecuteSQL(Strsql(EgxMoveType.First, NavigationObject), NavigationObject)[0];
                            CurrentIndex = (int)NavigationObject.GetType().GetProperty(PKName).GetValue(NavigationObject, null);
                        }
                        else if (moveType == EgxMoveType.Next)
                        {
                            NavigationObject = ExecuteSQL(Strsql(EgxMoveType.Next, NavigationObject), NavigationObject)[0];
                            CurrentIndex = (int)NavigationObject.GetType().GetProperty(PKName).GetValue(NavigationObject, null);
                        }
                        else if (moveType == EgxMoveType.Prev)
                        {
                            NavigationObject = ExecuteSQL(Strsql(EgxMoveType.Prev, NavigationObject), NavigationObject)[0];
                            CurrentIndex = (int)NavigationObject.GetType().GetProperty(PKName).GetValue(NavigationObject, null);

                        }
                        else if (moveType == EgxMoveType.Last)
                        {
                            NavigationObject = ExecuteSQL(Strsql(EgxMoveType.Last, NavigationObject), NavigationObject)[0];
                            CurrentIndex = (int)NavigationObject.GetType().GetProperty(PKName).GetValue(NavigationObject, null);
                        }
                    }
                }
            }
            catch
            {
                NavigationObject = ExecuteSQL(Strsql(EgxMoveType.First, NavigationObject), NavigationObject)[0];
                CurrentIndex = (int)NavigationObject.GetType().GetProperty(PKName).GetValue(NavigationObject, null);
            }
            return NavigationObject;
        }
        public string Strsql(EgxMoveType moveType, object o)
        {
            __obj = o;
            var pks = GetTbSchma(__obj);
            string strsql = "";
            string key = "";
            string keytype = "";
            foreach (KeyValuePair<string, string> keyValuePair in pks)
            {
                key = keyValuePair.Key;
                keytype = keyValuePair.Value;
                break;
            }
            if (pks.Count == 1)
            {
                switch (moveType)
                {

                    case EgxMoveType.First:
                        strsql = "select * from {0} where {1}=(select MIN({1}) from {0})".Frmt(__obj.ToString(), (keytype.Contains("String") ? ("'" + key + "'") : key));
                       
                        break;

                    case EgxMoveType.Next:
                        strsql = "select * from {0} where {1}=(select Min({1}) from {0} Where {1}>{2})".Frmt(__obj.ToString(), (keytype.Contains("String") ? ("'" + key + "'") : key),CurrentIndex.ToString());
                        break;

                    case EgxMoveType.Prev:
                        strsql = "select * from {0} where {1}=(select MAX({1}) from {0} Where {1}<{2})".Frmt(__obj.ToString(), (keytype.Contains("String") ? ("'" + key + "'") : key), CurrentIndex.ToString());
                        break;

                    case EgxMoveType.Last:
                        strsql = "select * from {0} where {1}=(select MAX({1}) from {0})".Frmt(__obj.ToString(), (keytype.Contains("String") ? ("'" + key + "'") : key));
                        break;

                    default:
                        strsql = "";
                        break;
                }
                if (NavigationCondition.Trim().Length > 0) {
                   strsql +=" AND "+ NavigationCondition; 
                }
            }
            return strsql;
        }

        #region Static Methods
        /// <summary>
        /// تحويل النص الموجود فى التكست بوكس الى تاريخ
        /// </summary>
        /// <param name="txtbx"> التكست بوكس المراد عمل له فورمات الى تاريخ </param>
        public static void CnvrtTxtToDt(System.Windows.Forms.TextBox txtbx)
        {
            try
            {
                DateTime dt;
                if (!DateTime.TryParse(txtbx.Text, out dt))
                {
                    switch (txtbx.Text.Length)
                    {
                        case 1: txtbx.Text = DateTime.Parse(String.Format("0{0}/{1}/{2}", txtbx.Text.Substring(0, 1), DateTime.Now.Month, DateTime.Now.Year)).ToShortDateString().ToString(); ; break;
                        case 2: txtbx.Text = DateTime.Parse(String.Format("{0}/{1}/{2}", txtbx.Text.Substring(0, 2), DateTime.Now.Month, DateTime.Now.Year)).ToShortDateString().ToString(); ; break;
                        case 4: txtbx.Text = DateTime.Parse(String.Format("{0}/{1}/{2}", txtbx.Text.Substring(0, 2), txtbx.Text.Substring(2, 2), DateTime.Now.Year)).ToShortDateString().ToString(); ; break;
                        case 6: txtbx.Text = DateTime.Parse(String.Format("{0}/{1}/{2}{3}", txtbx.Text.Substring(0, 2), txtbx.Text.Substring(2, 2), DateTime.Now.Year.ToString().Substring(0, 2), txtbx.Text.Substring(4, 2))).ToShortDateString().ToString(); break;
                        case 8: txtbx.Text = DateTime.Parse(String.Format("{0}/{1}/{2}", txtbx.Text.Substring(0, 2), txtbx.Text.Substring(2, 2), txtbx.Text.Substring(4, 4))).ToShortDateString().ToString(); break;
                        default: txtbx.Text = ""; break;
                    }

                }
                else
                {
                    txtbx.Text = dt.ToShortDateString();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                txtbx.Text = "";
            }

        }

        public static DbDataAdapter GetDataAdaptor(string Sql, EgxDataType DtTyp)
        {
            switch (DtTyp)
            {
                case EgxDataType.Mssql: return new SqlDataAdapter(Sql, DBConfig.ConnectionString /*GProperties.SqlConnectionString*/);
                case EgxDataType.Msaccess: return new OleDbDataAdapter(Sql, (new OleDbConnection(DBConfig.OleConnectionString)));
                case EgxDataType.Temp: return new OleDbDataAdapter(Sql, (new OleDbConnection(DBConfig.TempDBConnectionstring)));
                case EgxDataType.GLConnection: return new SqlDataAdapter(Sql, (new SqlConnection(DBConfig.GLConnectionString)));

                default: return new SqlDataAdapter(Sql, DBConfig.ConnectionString /*GProperties.SqlConnectionString*/); ;
            }

        }
        public static DbCommand GetDbCommand(string sql, EgxDataType DtTyp)
        {
            switch (DtTyp)
            {
                case EgxDataType.Mssql: return new SqlCommand(sql, new SqlConnection(DBConfig.ConnectionString));
                case EgxDataType.Msaccess: return new OleDbCommand(sql, (new OleDbConnection(DBConfig.ConnectionString)));
                case EgxDataType.Temp: return new OleDbCommand(sql, (new OleDbConnection(DBConfig.TempDBConnectionstring)));
                case EgxDataType.GLConnection: return new SqlCommand(sql, (new SqlConnection(DBConfig.GLConnectionString)));
                default: return new SqlCommand();
            }

        }
        public static DbParameter GetDbParameter(string parameterName, object value, EgxDataType DtTyp)
        {
            switch (DtTyp)
            {
                case EgxDataType.Mssql: return new SqlParameter(parameterName, value);
                case EgxDataType.Msaccess: return new OleDbParameter(parameterName, value);
                case EgxDataType.Temp: return new OleDbParameter(parameterName, value);
                case EgxDataType.GLConnection: return new SqlParameter(parameterName, value);

                default:
                    return null;
            }

        }
        public static DbConnection GetConnection(string StrCon, EgxDataType DtTyp)
        {
            switch (DtTyp)
            {
                case EgxDataType.Mssql: return new SqlConnection(StrCon);
                case EgxDataType.Msaccess: return new OleDbConnection(StrCon);
                case EgxDataType.Temp: return new OleDbConnection(StrCon);
                case EgxDataType.GLConnection: return new SqlConnection(StrCon);
                default: return null;
            }

        }
        public static DbCommandBuilder GetDbCommandBuilder(DbDataAdapter Da_, EgxDataType datatype)
        {
            switch (datatype)
            {
                case EgxDataType.Mssql: return new SqlCommandBuilder(Da_ as SqlDataAdapter);
                case EgxDataType.Msaccess: return new OleDbCommandBuilder(Da_ as OleDbDataAdapter);
                case EgxDataType.Temp: return new OleDbCommandBuilder(Da_ as OleDbDataAdapter);
                case EgxDataType.GLConnection: return new SqlCommandBuilder(Da_ as SqlDataAdapter);
                default: return null;
            }
        }
        /// <summary>
        /// انشاء الفولدرات المرات استخدامها فى المشروع
        /// </summary>
        public static void CreateFolders()
        {
            if (!File.Exists("Prop"))
            {
                Directory.CreateDirectory("Prop");
            }
        }
        /// <summary>
        /// ملأ الكمبوبوكس 
        /// </summary>
        /// <param name="Cmb">الكمبوبوكس المراد ملؤه بالداتا</param>
        /// <param name="ValueMember"> ValueMember </param>
        /// <param name="DisplayMember"> DisplayMember </param>
        /// <param name="sqlstr"> جملة السيكول </param>
        /// <param name="Datatype"> نوع الداتا </param>
        public static void FillComboBox(ComboBox Cmb, string ValueMember, string DisplayMember, string sqlstr, EgxDataType Datatype)
        {
            try
            {
                using (DbDataAdapter da = GetDataAdaptor(sqlstr, Datatype))
                {
                    DataTable Dt = new DataTable();

                    Dt.Columns.Add(ValueMember);
                    Dt.Columns.Add(DisplayMember);
                    DataRow dr = Dt.NewRow();
                    dr[0] = DBNull.Value;
                    dr[1] = DBNull.Value;
                    Dt.Rows.Add(dr);
                    da.Fill(Dt);
                    Dt.DefaultView.Sort = Dt.Columns[0].ColumnName;
                    Cmb.DataSource = Dt;
                    Cmb.DisplayMember = DisplayMember;
                    Cmb.ValueMember = ValueMember;

                }
            }
            catch (Exception Ex) { MessageBox.Show(Ex.Message, DBConfig.ProjectName); }
        }
        public static void FillComboBox(DataGridViewComboBoxColumn Cmb, string ValueMember, string DisplayMember, string sqlstr, EgxDataType Datatype)
        {
            try
            {
                using (DbDataAdapter da = GetDataAdaptor(sqlstr, Datatype))
                {
                    DataTable Dt = new DataTable();

                    Dt.Columns.Add(ValueMember);
                    Dt.Columns.Add(DisplayMember);
                    DataRow dr = Dt.NewRow();
                    dr[0] = DBNull.Value;
                    dr[1] = DBNull.Value;
                    Dt.Rows.Add(dr);
                    da.Fill(Dt);
                    Dt.DefaultView.Sort = Dt.Columns[0].ColumnName;
                    Cmb.DataSource = Dt;
                    Cmb.DisplayMember = DisplayMember;
                    Cmb.ValueMember = ValueMember;

                }
            }
            catch (Exception Ex) { MessageBox.Show(Ex.Message, DBConfig.ProjectName); }
        }
        public static void FillComboBox(ComboBox cm)
        {
            try
            {
                DataTable Dt = new DataTable();

                Dt.Columns.Add("DspV", typeof(string));
                Dt.Columns.Add("MemberV", typeof(int));

                DataRow dr = Dt.NewRow();
                dr[0] = DBNull.Value;
                dr[1] = DBNull.Value;
                Dt.Rows.Add(dr);
                foreach (var item in cm.Items)
                {
                    DataRow Dr = Dt.NewRow();
                    string st = item.ToString();
                    string[] Sp = st.Split(':');
                    int MemberV = 0;
                    string DspV = "";
                    foreach (string str in Sp)
                    {
                        int x = 0;
                        if (int.TryParse(str, out x))
                        {
                            Dr["MemberV"] = str;
                            MemberV = int.Parse(str);
                        }
                        else
                        {
                            Dr["DspV"] = str;
                            DspV = str;
                        }
                        if (MemberV != 0 && DspV != "")
                        {
                            Dt.Rows.Add(Dr);
                            Dr = Dt.NewRow();
                        }
                    }
                }

                cm.Items.Clear();
                foreach (DataRow DR in Dt.Rows)
                {
                    DR["DspV"] = DR["MemberV"].Nz() + (DR[0] == DBNull.Value ? "" : " : ") + DR["DspV"].Nz();
                }
                Dt.DefaultView.Sort = Dt.Columns[0].ColumnName;
                cm.DataSource = Dt;
                cm.DisplayMember = "DspV";
                cm.ValueMember = "MemberV";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, DBConfig.ProjectName); }
        }
        public static void FillComboBox(ComboBox cm, string SQL, EgxDataType Datatype)
        {
            try
            {
                DataTable Dt = new DataTable();
                Dt.FillBySql(SQL, Datatype);
                DataRow Dr = Dt.NewRow();
                Dr[0] = DBNull.Value;
                Dr[1] = DBNull.Value;
                Dt.Rows.Add(Dr);
                Dt.DefaultView.Sort = Dt.Columns[0].ColumnName;
                cm.DataSource=Dt;
                foreach (DataRow DR in Dt.Rows)
                {
                    DR[1] = DR[0].Nz() + (DR[0] == DBNull.Value ? "" : " : ") + DR[1].Nz();
                }
                Dt.DefaultView.Sort = Dt.Columns[0].ColumnName;
                cm.ValueMember = Dt.Columns[0].ColumnName;
                cm.DisplayMember = Dt.Columns[1].ColumnName;
              
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, DBConfig.ProjectName); }
        }
        public static void FillComboBox(DataGridViewComboBoxColumn cm, string SQL)
        {
            try
            {
                DataTable Dt = new DataTable();
                Dt.FillBySql(SQL, DBConfig.Datatype);
                DataRow Dr = Dt.NewRow();
                Dr[0] = DBNull.Value;
                Dr[1] = DBNull.Value;
                Dt.Rows.Add(Dr);
                foreach (DataRow DR in Dt.Rows)
                {
                    DR[1] = DR[0].Nz() + (DR[0] == DBNull.Value ? "" : " : ") + DR[1].Nz();
                }
                Dt.DefaultView.Sort = Dt.Columns[0].ColumnName;
                cm.DataSource = Dt;
                cm.ValueMember = Dt.Columns[0].ColumnName;
                cm.DisplayMember = Dt.Columns[1].ColumnName;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, DBConfig.ProjectName); }
        }
        public static void FillComboBox(ComboBox cm, string SQL)
        {
            try
            {
                DataTable Dt = new DataTable();
                Dt.FillBySql(SQL, DBConfig.Datatype);
                DataRow Dr = Dt.NewRow();
                Dr[0] = DBNull.Value;
                Dr[1] = DBNull.Value;
                Dt.Rows.Add(Dr);
                foreach (DataRow DR in Dt.Rows)
                {
                    DR[1] = DR[0].Nz() + (DR[0] == DBNull.Value ? "" : " : ") + DR[1].Nz();
                }
                Dt.DefaultView.Sort = Dt.Columns[0].ColumnName;
                cm.DataSource = Dt;
                cm.ValueMember = Dt.Columns[0].ColumnName;
                cm.DisplayMember = Dt.Columns[1].ColumnName;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, DBConfig.ProjectName); }
        }
        public static void FillComboBox(ComboBox cm, string sqlquery, string Strcon, EgxDataType Dtype)
        {
            try
            {
                DataTable Dt = new DataTable();
                DbDataAdapter Da =
                GetDataAdaptor(sqlquery, Strcon, Dtype);
                Da.Fill(Dt);
                DataRow Dr = Dt.NewRow();
                Dr[0] = DBNull.Value;
                Dr[1] = DBNull.Value;
                Dt.Rows.Add(Dr);
                cm.DataSource = Dt;
                cm.ValueMember = Dt.Columns[0].ColumnName;
                cm.DisplayMember = Dt.Columns[1].ColumnName;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, DBConfig.ProjectName); }
        }

        public static object GetSqlValue(string sql, string Strcon, EgxDataType Dtype)
        {
            using (DbCommand cmd = GetDbCommand("", Dtype))
            {
                try
                {
                    object value = "";
                    cmd.CommandText = sql;
                    cmd.Connection.Open();
                    value = cmd.ExecuteScalar();
                    return value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, DBConfig.ProjectName);
                    return null;
                }
                finally
                {
                    cmd.Connection.Close();
                }

            }
        }
        public static object GetSqlValue(string sql, EgxDataType Dtype)
        {
            using (DbCommand cmd = GetDbCommand("", Dtype))
            {
                try
                {
                    cmd.CommandText = sql;
                    cmd.Connection.Open();
                    object value = "";
                    value = cmd.ExecuteScalar();
                    return value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, DBConfig.ProjectName);
                    return null;
                }
                finally
                {
                    cmd.Connection.Close();
                }

            }
        }
        public object GetSqlValue(string sql)
        {
            using (DbCommand cmd = GetDbCommand("", DBConfig.Datatype))
            {
                try
                {
                    prepareConnection();
                    cmd.Connection = __CONN;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    object value = "";
                    value = cmd.ExecuteScalar();
                    if (value.GetType() != typeof(DBNull))
                    {
                        return value;
                    }
                    else { return null; }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, DBConfig.ProjectName);
                    return null;
                }
                finally
                {
                   // cmd.Connection.Close();
                }

            }
        }

        /// <summary>
        /// فنكشن للاحتفاظ بخواص الداتا جريد فى ملف اكس ام ال
        /// </summary>
        /// <param name="mdg"> الداتا جريد المراد الاحتفاظ بخواصها </param>
        public static void SavePropertiesDataGrid(DataGridView mdg)
        {
            using (DataTable dt = new DataTable(mdg.Name))
            {
                dt.Columns.Add("name");
                dt.Columns.Add("width");
                dt.Columns.Add("index");
                for (int i = 0; i < mdg.Columns.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["name"] = mdg.Columns[i].Name;
                    dr["width"] = mdg.Columns[i].Width;
                    dr["index"] = mdg.Columns[i].DisplayIndex;
                    dt.Rows.Add(dr);
                }
                if (mdg.Columns.Count >= 2)
                {
                    CreateFolders();
                    dt.WriteXml(String.Format("Prop\\{0}.xml", mdg.Name));
                    dt.WriteXmlSchema(String.Format("Prop\\{0}.xsd", mdg.Name));

                }
            }
        }
        /// <summary>
        /// تحميل خواص الداتا جربد من ملف الاكس ام ال المحتفظ بيه
        /// </summary>
        /// <param name="mdg"> الداتا جرد </param>
        public static void LoadPropertiesDataGrid(DataGridView mdg)
        {
            try
            {
                if (File.Exists(String.Format("Prop\\{0}.xsd", mdg.Name)) & File.Exists(String.Format("Prop\\{0}.xml", mdg.Name)))
                {
                    using (DataTable dt = new DataTable())
                    {
                        dt.ReadXmlSchema(String.Format("Prop\\{0}.xsd", mdg.Name));
                        dt.ReadXml(String.Format("Prop\\{0}.xml", mdg.Name));
                        foreach (DataRow dr in dt.Rows)
                        {
                            mdg.Columns[dr[0].ToString()].Width = int.Parse(dr[1].ToString());
                            mdg.Columns[dr[0].ToString()].DisplayIndex = int.Parse(dr[2].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DBConfig.ProjectName);
            }
        }
        /// <summary>
        /// تنفيذ امر مباشر فى الداتا 
        /// </summary>
        /// <param name="sql"> جملة السيكول المراد تنفيذها فى قاعده البيانات</param>
        /// <param name="Dtype"> نوع الداتا بيز </param>
        /// <returns>عدد الصفوف التى تأثرت بالامر المرسل</returns>
        public static int ExecData(string sql, EgxDataType Dtype)
        {
            using (DbCommand Dbcmd = GetDbCommand(sql, Dtype))
            {
                try
                {
                    Dbcmd.CommandText = sql;
                    Dbcmd.Connection.Open();
                    return Dbcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, DBConfig.ProjectName);
                    return 0;
                }
                finally
                {
                    Dbcmd.Connection.Close();
                }

            }
        }
        /// <summary>
        /// تنفيذ امر مباشر فى الداتا 
        /// </summary>
        /// <param name="sql"> جملة السيكول المراد تنفيذها فى قاعده البيانات</param>
        /// <returns>عدد الصفوف التى تأثرت بالامر المرسل</returns>
        public static int ExecData(string sql)
        {
            using (DbCommand Dbcmd = GetDbCommand(sql, DBConfig.Datatype))
            {
                try
                {
                    Dbcmd.CommandText = sql;
                    Dbcmd.Connection.Open();
                    return Dbcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, DBConfig.ProjectName);
                    return 0;
                }
                finally
                {
                    Dbcmd.Connection.Close();
                }

            }
        }
        public static int ExecTransaction(List<string> sqlQueries) 
        {
            SqlTransaction trans = null;
            using (DbCommand Dbcmd = GetDbCommand("", DBConfig.Datatype))
            {
                try
                {
                    Dbcmd.Connection.Open();
                    trans = (SqlTransaction)Dbcmd.Connection.BeginTransaction();
                    foreach (string cmd in sqlQueries)
                    {
                        Dbcmd.CommandText = cmd;
                        Dbcmd.Transaction = trans;
                        Dbcmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, DBConfig.ProjectName);
                    trans.Rollback();
                    return 0;
                }
                finally
                {
                    Dbcmd.Connection.Close();
                }

            }
            return 1;
        }
        public static DataTable GetDataTable(string Sql, string TbName, ref DbDataAdapter Da, EgxDataType Dtype)
        {
            using (DataSet Ds = new DataSet())
            {
                Da = GetDataAdaptor(Sql, Dtype);
                Da.Fill(Ds, TbName);
                return Ds.Tables[0];
            }
        }
        public static DataTable GetDataTable(string Sql, string TbName)
        {
            using (DataSet Ds = new DataSet())
            {
                using (DbDataAdapter Da = GetDataAdaptor(Sql, DBConfig.Datatype))
                {
                    Da.Fill(Ds, TbName);
                    return Ds.Tables[0];
                }
            }
        }
        public static DataTable GetDataTable(string Sql)
        {
            try
            {
                using (DataTable DT = new DataTable())
                {
                    using (DbDataAdapter Da = GetDataAdaptor(Sql, DBConfig.Datatype))
                    {
                        Da.Fill(DT);
                        return DT;
                    }
                }
            }
            catch (Exception exp) { MessageBox.Show(exp.Message); return null; }
        }
        public static DataTable GetDataTable(string Sql, EgxDataType Dtype)
        {
            using (DataTable DT = new DataTable())
            {
                using (DbDataAdapter Da = GetDataAdaptor(Sql, Dtype))
                {
                    Da.Fill(DT);
                    return DT;
                }
            }
        }
        public static void GetDataTable(string Sql, ref DbDataAdapter Da, ref DataTable Dt, EgxDataType DtTyp)
        {
            Dt = new DataTable();
            Da = GetDataAdaptor(Sql, DtTyp);
            Da.Fill(Dt);
        }
        public static bool TabKey(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    SendKeys.Send("{tab}");
                    return true;
                default:
                    return false;
            }
        }

        public static void Set_Txt_Num(TextBox Tx)
        {
            double d = 0;
            if (!double.TryParse(Tx.Text, out d))
            {
                Tx.Text = "";
            }
        }

        /*
        public static void GetInfCtrl(Control ctrl, Form this1)
        {
            try
            {
                if (ctrl == null || this1 == null) return;
                if (ctrl is TextBox)
                {
                    MessageBox.Show(string.Format("Field : {0}\n\n" + "TableName : {1}\n\nTag : {2}", ((TextBox)ctrl).DataBindings[0].BindingMemberInfo.BindingMember, (this1 as FRMWRK.FORMS.AnyFormCreate).TableName, ((TextBox)ctrl).Tag.Nz("")), GProperties.ProjectName);
                }
                else if (ctrl is DataGridView)
                {
                    MessageBox.Show(string.Format("DataSource : {0}\n\nField Name : {1}", ((DataGridView)ctrl).Name, ((DataGridView)ctrl).CurrentCell.OwningColumn.Name), GProperties.ProjectName);
                }
                else if (ctrl is Button)
                {
                    MessageBox.Show(string.Format("Tag : {0}", ((Button)ctrl).Tag), GProperties.ProjectName);
                }
                else if (ctrl is ComboBox)
                {
                    MessageBox.Show(string.Format("Field : {0}\n\n" + "TableName : {1}\n\nTag : {2}", ((ComboBox)ctrl).DataBindings[0].BindingMemberInfo.BindingMember, (this1 as FRMWRK.FORMS.AnyFormCreate).TableName, ((ComboBox)ctrl).Tag.Nz("")), GProperties.ProjectName);
                }
                else if (ctrl is CheckBox)
                {
                    MessageBox.Show(string.Format("Field : {0}\n\n" + "TableName : {1}\n\nTag : {2}", ctrl.Name, ((CheckBox)ctrl).DataBindings[0].BindingMemberInfo.BindingMember, ((CheckBox)ctrl).Tag.Nz("")), GProperties.ProjectName);
                }
            }

            catch (Exception ex) { MessageBox.Show(ex.Message, GProperties.ProjectName); }
        }
        */

        public static DataRow GetDataRow(string SQL, string STRCON, EgxDataType Dttype)
        {
            try
            {
                using (DataTable Dt = new DataTable())
                {
                    DbDataAdapter Da = GetDataAdaptor(SQL, STRCON, Dttype);
                    Da.Fill(Dt);
                    return Dt.Rows[0];
                }
            }
            catch (Exception ex)
            {
              //  MessageBox.Show(ex.Message, DBConfig.ProjectName);
                return null;
            }
        }
        public static DataRow GetDataRow(string SQL, EgxDataType Dttype)
        {
            try
            {
                using (DataTable Dt = new DataTable())
                {
                    using (DbDataAdapter Da = GetDataAdaptor(SQL, Dttype))
                    {
                        Da.Fill(Dt);
                        return Dt.Rows[0];
                    }
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message, DBConfig.ProjectName);
                return null;
            }
        }
        public static DataRow GetDataRow(string SQL, string STRCON)
        {
            try
            {
                using (DataTable Dt = new DataTable())
                {
                    using (DbDataAdapter Da = GetDataAdaptor(SQL, STRCON, DBConfig.Datatype))
                    {
                        Da.Fill(Dt);
                        return Dt.Rows[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DBConfig.ProjectName);
                return null;
            }
        }
        public static DataRow GetDataRow(string SQL)
        {
            try
            {
                using (DataTable Dt = new DataTable())
                {
                    using (DbDataAdapter Da = GetDataAdaptor(SQL, DBConfig.Datatype))
                    {
                        Da.Fill(Dt);
                        if (Dt.Rows.Count > 0)
                            return Dt.Rows[0];
                        else return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, DBConfig.ProjectName);
                return null;
            }
        }

        public static void SaveExcelFileToDatabase(string filePath) 
        {
            String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0\"", filePath);
            using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
            {
                //Create OleDbCommand to fetch data from Excel 
                using (OleDbCommand cmd = new OleDbCommand("Select [ID],[Name],[Designation] from [Sheet1$]", excelConnection))
                {
                    excelConnection.Open();
                    using (OleDbDataReader dReader = cmd.ExecuteReader())
                    {
                        using (SqlBulkCopy sqlBulk = new SqlBulkCopy(DBConfig.ConnectionString))
                        {
                            //Give your Destination table name 
                            sqlBulk.DestinationTableName = "Excel_table";
                            sqlBulk.WriteToServer(dReader);
                        }
                    }
                }
            }
        }
        /*public static void SaveFormProp(Form F)
        {
            DataTable Dt = new DataTable(F.Name);
            Dt.Columns.Add("Location_x");
            Dt.Columns.Add("Location_y");
            Dt.Columns.Add("Size_Height");
            Dt.Columns.Add("Size_Width");
            Dt.Columns.Add("WindowState");
            DataRow Dr = Dt.NewRow();
            Point p = F.Location;
            Dr["Location_x"] = p.X;
            Dr["Location_y"] = p.Y;
            Size S = F.Size;
            Dr["Size_Height"] = S.Height;
            Dr["Size_Width"] = S.Width;
            Dr["WindowState"] = F.WindowState.ToString();
            Dt.Rows.Add(Dr);
            Dt.WriteXml("Prop\\" + F.Name + ".XML");
            Dt.WriteXmlSchema("Prop\\" + F.Name + ".XSD");
        }
        public static void LoadFormProp(Form F)
        {
            if (System.IO.File.Exists("Prop\\" + F.Name + ".xml") && System.IO.File.Exists("Prop\\" + F.Name + ".XSD"))
            {
                DataTable Dt = new DataTable();
                Dt.ReadXmlSchema("Prop\\" + F.Name + ".XSD");
                Dt.ReadXml("Prop\\" + F.Name + ".xml");
                foreach (DataRow Dr in Dt.Rows)
                {
                    switch (Dr["WindowState"].ToString())
                    {
                        case "Maximized": F.WindowState = FormWindowState.Maximized; break;
                        case "Minimized": F.WindowState = FormWindowState.Minimized; break;
                        default: F.WindowState = FormWindowState.Normal;
                            break;
                    }
                    F.Location = new Point(int.Parse(Dr["Location_x"].ToString()), int.Parse(Dr["Location_y"].ToString()));
                    F.Size = new Size(int.Parse(Dr["Size_Width"].ToString()), int.Parse(Dr["Size_Height"].ToString()));
                }

            }

        }*/

        /* التصدير الاكسل من الجرد
        public static void Exp_To_Exl(DataGridView dgrid)
        {

            System.Globalization.CultureInfo Oldci = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook;
            workbook = null;
            workbook = app.Workbooks.Add(Type.Missing);
            app.Visible = false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;

            // get the reference of first sheet. By default its name is Sheet1.
            // store its reference to worksheet
            worksheet = workbook.Sheets["Sheet1"] as Microsoft.Office.Interop.Excel.Worksheet;
            worksheet = workbook.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;

            // changing the name of active sheet
            worksheet.Name = "Exported from gridview";

            // storing header part in Excel
            for (int i = 1; i < dgrid.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dgrid.Columns[i - 1].HeaderText;
            }

            // storing Each row and column value to excel sheet
            for (int i = 0; i < dgrid.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dgrid.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dgrid.Rows[i].Cells[j].Value.ToString();
                }
            }
            app.Visible = true;
            // save the application
            //workbook.SaveAs("D:\\output.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            // Exit from the application
            //app.Quit();
        }
        */



        public static object GetQryData(string sql, EgxDataType dataType, EgxData data)
        {
            var dataAdapter = GetDataAdaptor(sql, dataType);
            switch (data)
            {
                case EgxData.DataTable:
                    var dt = new DataTable();
                    dataAdapter.Fill(dt);
                    return dt;
                case EgxData.DataSet:
                    var ds = new DataSet();
                    dataAdapter.Fill(ds);
                    return ds;
                case EgxData.DataRow:
                    var dr = new DataTable();
                    dataAdapter.Fill(dr); return dr.Rows[0];
                default:
                    return null;
            }

        }
        public static DbDataAdapter GetDataAdaptor(EgxDataType DtTyp)
        {
            switch (DtTyp)
            {
                case EgxDataType.Mssql: return new SqlDataAdapter("", DBConfig.SqlConnectionString);
                case EgxDataType.Msaccess: return new OleDbDataAdapter("", (new OleDbConnection(DBConfig.OleConnectionString)));
                default: return null;
            }

        }
        public static DbDataAdapter GetDataAdaptor(string Sql, string StrCon, EgxDataType DtTyp)
        {
            switch (DtTyp)
            {
                case EgxDataType.Mssql: return new SqlDataAdapter(Sql, StrCon);
                case EgxDataType.Msaccess: return new OleDbDataAdapter(Sql, StrCon);
                default: return null;
            }

        }
        public static T GetQryData<T>(string sql, EgxDataType dataType) where T : new()
        {
            DbDataAdapter dataAdapter = GetDataAdaptor(sql, dataType);
            var t = new T();
            if (t is DataSet)
            {
                var ds = t as DataSet;
                dataAdapter.Fill(ds);
            }
            if (t is DataTable)
            {
                var dt = t as DataTable;
                dataAdapter.Fill(dt);
            }
            if (t is DataRow)
            {
                var dr = t as DataRow;
                dataAdapter.Fill(dr.Table);
            }
            return t;
        }
        #endregion


         
    }
}
