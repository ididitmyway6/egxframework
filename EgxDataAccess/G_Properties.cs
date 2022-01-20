using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Drawing;

namespace Egx.EgxDataAccess
{
    /// <summary>
    /// انواع الداتا بيز المستخدمة
    /// </summary>
    public enum EgxDataType { Mssql = 1, Msaccess, Oracle, Xml , SqlCE ,EFMSSQL, Temp,GLConnection }
    public enum EgxData { DataTable = 1, DataRow, DataSet }
    public enum EgxMoveType { First = 1, Next, Prev, Last }
    public enum EgxCommandType { Insert=1, Update, Delete, Search }
    [Flags]
    public enum TextType { String, Integer, Float, DateTime, Decimal }
    /// <summary>
    ///  الكلاس الخاصة بالمتغيرات العامة على مستوى البرنامج
    /// </summary>
    public abstract class GProperties
    {
        
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

        public static string SqlConnectionString
        {
            get;
            set;
        }
        /// <summary>
        /// مسار الاتصال بقاعدة البيانات الخاصة بالبرنامج اكسس
        /// </summary>
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

    }
}
