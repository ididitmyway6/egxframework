using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Egx.EgxDataAccess
{
    public class DBConfig
    {
        static DBConfig() 
        {
            SqlConnectionString = "";
            OleConnectionString = "";
            TempDBConnectionstring = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\SysDB.mdb";
        }
        public static string ConnectionString 
        { get 
        {
            if (Datatype == EgxDataType.Msaccess) 
            { 
                return OleConnectionString; 
            } 
            else if (Datatype == EgxDataType.Mssql && SqlConnectionString.Length>0) 
            { 
                return SqlConnectionString;
            }
            else if (Datatype == EgxDataType.Mssql)
            {
                return "Server='" + DBConfig.ServerName + "';Database='" + DBConfig.DbName + "';User Id='" + DBConfig.UserName + "'; Password='" + DBConfig.Password + "';";
            }
            else if (Datatype == EgxDataType.EFMSSQL) 
            {
                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
                sqlBuilder.DataSource = ServerName;
                sqlBuilder.InitialCatalog = DbName;
                sqlBuilder.UserID = UserName;
                sqlBuilder.Password = Password;
                //sqlBuilder.IntegratedSecurity = true;
                EntityConnectionStringBuilder entityConnBuilder = new EntityConnectionStringBuilder();
                entityConnBuilder.Provider = "System.Data.SqlClient";
                entityConnBuilder.ProviderConnectionString = sqlBuilder.ToString();
                entityConnBuilder.Metadata = "res://*/" + ModelName + ".csdl|res://*/" + ModelName + ".ssdl|res://*/" + ModelName + ".msl";
                return entityConnBuilder.ToString();
                    //"metadata=res://*/"+ModelName+".csdl|res://*/"+ModelName+".ssdl|res://*/"+ModelName+".msl;provider=System.Data.SqlClient;provider connection string=&quot;data source="+ServerName+";initial catalog="+DbName+";user id="+UserName+";password="+Password+";MultipleActiveResultSets=True;App=EntityFramework&quot;" ;
            }
            else
            {
                return null;
            }
        } 
        }

        public static string ServerName { get; set; }

        public static string ModelName { get; set; }

        public static string DbName { get; set; }

        public static string DbType { get; set; }

        public static string UserName { get; set; }

        public static string Password { get; set; }

        public static string GLConnectionString { get; set; }

        public static bool SqlServerTracking { get; set; }

        public static bool IsWebApp { get; set; }
        public static EgxDataType Datatype
        {
            get;
            set;
        }

        public static string StrDataSql
        {
            get;
            set;
        }

        /// <summary>
        /// اسم البرنامج المنتج  
        /// </summary>
        public static string ProjectName
        {
            get;
            set;
        }
        /// <summary>
        /// مسار الاتصال بقاعدة البيانات الخاصة بالبرنامج سيكول سيرفر
        /// </summary>
        [DefaultValue("")]
        public static string SqlConnectionString
        {
            get;
            set;
        }
        /// <summary>
        /// مسار الاتصال بقاعدة البيانات الخاصة بالبرنامج اكسس
        /// </summary>
        [DefaultValue("")]
        public static string OleConnectionString
        {
            get;
            set;
        }

        public static string Dbused
        {
            get;
            set;
        }
        public static string Lang
        {
            get;
            set;
        }
        public static string TempDBConnectionstring { get; set; }
        public static void SetGLConnection(string server, string db, string user, string password) 
        {
            GLConnectionString= "Server='" + server + "';Database='" + db + "';User Id='" + user + "'; Password='" + password + "';";

        }


    }
}
