using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Egx.EgxDataAccess
{
    public enum SystemQueryKeys { ID, Query, QDesc }
    public enum Types { INT, NVARCHAR_50, NVARCHAR_MAX, DATE, DATE_TIME }
    public enum QueryType { Insert, Update, Delete, Select }
    public class SystemQuery : DataAccess
    {
        public int ID { get; set; }
        public string Query { get; set; }
        public string QDesc { get; set; }
        public string QName { get; set; }
        public QueryType QType { get; set; }
        DataAccess dal;
        ProceduresList ProcList;
        QueryParameters QPars;
        public override string ToString()
        {
            return "SystemQueries";
        }
        public SystemQuery()
        {
            dal = new DataAccess();
            QPars = new QueryParameters();
            // ProcList.Clear();
            //QPars.Clear();
        }

        public void AddProcedure(string procText)
        {
            ProcList.Add(procText);
        }

        public void AddParameter(string name, string type, string DefaultValue)
        {
            QPars.NewQueryParameters(name, type, DefaultValue);
        }

        public string GenerateSystemQuery(EgxCommandType CommandType)
        {
            return @" USE " + DBConfig.DbName + " GO SET ANSI_NULLS ON GO SET QUOTED_IDENTIFIER ON GO " +
                   @" Create PROCEDURE [dbo].[" + (CommandType == EgxCommandType.Search ? this.QDesc + "Search" :
                   CommandType == EgxCommandType.Insert ? this.QDesc + "Insert" :
                   CommandType == EgxCommandType.Update ? this.QDesc + "Update" :
                   CommandType == EgxCommandType.Delete ? this.QDesc + "Delete" :
                   "Empty") + "]";

        }

        public void CreateNewQuery()
        {
            dal.Insert(this);
        }

        public void UpdateQuery(SystemQueryKeys UpdateKey)
        {
            switch (UpdateKey)
            {
                case SystemQueryKeys.ID:
                    dal.Update(this, "ID");
                    break;
                case SystemQueryKeys.QDesc:
                    dal.Update(this, "QDesc");
                    break;
                default:
                    dal.Update(this, "Query");
                    break;
            }
        }

        public void DeleteQuery(SystemQueryKeys DeleteBy)
        {
            switch (DeleteBy)
            {
                case SystemQueryKeys.ID:
                    dal.Delete(this, "ID");
                    break;
                case SystemQueryKeys.QDesc:
                    dal.Delete(this, "QDesc");
                    break;
                default:
                    dal.Delete(this, "Query");
                    break;
            }
        }

        public SystemQuery GetQuery(SystemQueryKeys SelectBy)
        {
            switch (SelectBy)
            {
                case SystemQueryKeys.ID:
                    return (SystemQuery)dal.GetByColumn("ID", this.ID, this);
                case SystemQueryKeys.QDesc:
                    return (SystemQuery)dal.GetByColumn("QDesc", this.QDesc, this);
                default:
                    return (SystemQuery)dal.GetByColumn("Query", this.Query, this);
            }
        }

        public void ExecuteProceduresList(ProceduresList ProcList)
        {

        }

        public class ProceduresList : List<string>
        {
            //Constructor

            public ProceduresList()
            {

            }
        }

        public class QueryParameters : List<string>, IEnumerable
        {
            Hashtable ht;
            List<string> lst;
            int i = 0;
            public QueryParameters()
            {
                ht = new Hashtable();
                i = 0;
            }
            public void NewQueryParameters(string name, string type, string DefaultValue)
            {
                lst = new List<string>();
                lst.Clear();
                lst.Add(name);
                lst.Add(type);
                lst.Add(DefaultValue);
                ht.Add(i, lst);
                i++;
            }


            public int Count() { return i; }

            public IEnumerator GetEnumerator()
            {
                ICollection key = ht.Keys;
                foreach (object o in key)
                {
                    if (o == null)
                    {
                        break;
                    }
                    yield return ht[o];
                }
            }
        }
    }
}
