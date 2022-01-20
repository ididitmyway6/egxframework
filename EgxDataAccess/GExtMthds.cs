using System;
using System.Runtime.CompilerServices;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Egx.EgxDataAccess
{
    public static class GExtMthds
    {
        /// <summary>
        /// تقوم بتحويل الكائنات او القيم الفارغة الى قيم جديده
        /// </summary>
        /// <param name="Value"> قمية المتغير</param>
        /// <param name="RetValue"> القيمة المرتجعة البديلة</param>
        /// <returns>القيمة المستخرجة من التحويل</returns>
        public static string Nz(this object Value, string RetValue)
        {
            if (Value == DBNull.Value || Value == null || Value.ToString() == "")
            {
                return RetValue;
            }
            else
            {
                return Value.ToString();
            }
        }

        public static object DBNullToNull(this object dbnull) 
        {
            if (dbnull == DBNull.Value) { return null; } else { return dbnull; }
        }


        public static string SqlDateTime(this DateTime? date)
        {
            return " cast('" + date.Value.Year + "-" + date.Value.Month + "-" + date.Value.Day + " " +
                date.Value.Hour + ":" + date.Value.Minute + ":" + date.Value.Second + "." + date.Value.Millisecond +
                "' as datetime)";
        }
        public static string SqlDateTime(this DateTime? date,bool shortFormat)
        {
            if (shortFormat) 
            {
                return "" + date.Value.Year + "-" + date.Value.Month + "-" + date.Value.Day + "";
            }
            else
            {
                return " cast('" + date.Value.Year + "-" + date.Value.Month + "-" + date.Value.Day + " " +
                    date.Value.Hour + ":" + date.Value.Minute + ":" + date.Value.Second + "." + date.Value.Millisecond +
                    "' as datetime)";
            }
        }

        /// <summary>
        /// تحويل الكائن الى نص
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string Nz(this object Value)
        {
            return Value.ToString();
        }

        /// <summary>
        /// تقطيع النص 
        /// </summary>
        /// <param name="value">النص</param>
        /// <param name="spltval">حرف التقطيع</param>
        /// <returns></returns>
        public static string[] SplitN(this string value, char spltval)
        {
            return value.Split(spltval);
        }

        public static string Format(this string value, params object[] objects)
        {
            return string.Format(value, objects);
        }

        /// <summary>
        /// تحويل اى قيم فارغة او مصفرة الى قيمة فارغة قواعد بيانات
        /// </summary>
        /// <param name="Value"> الابوجيكت </param>
        /// <returns>BDNULL.VALUE</returns>
        public static object AnyToDbnull(this object Value)
        {
            if (Value.Nz("") == "0" || Value.Nz("") == "" || Value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return Value;
            }
        }

        public static T CnvrtTo<T>(this object Val) where T : IConvertible
        {
            return (T)Convert.ChangeType(Val, typeof(T));
        }

        /// <summary>
        /// تعبئة الجدول بجملة السيكول 
        /// </summary>
        /// <param name="Dt"> الجدول </param>
        /// <param name="Sql">جملة السيكول</param>
        /// <param name="DtTyp">نوع الداتا بيز </param>
        /// <returns>بيرجع الجدول بالبيانات </returns>
        public static DataTable FillBySql(this DataTable Dt, string Sql, EgxDataType DtTyp)
        {
            try
            {
                using (DbDataAdapter Adptr = GMthds.GetDataAdaptor(Sql, DtTyp))
                {
                    Adptr.Fill(Dt);
                    return Dt;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, GProperties.ProjectName);
                return null;
            }
        }
        /// <summary>
        /// تعبئة الجدول بجملة السيكول 
        /// </summary>
        /// <param name="Dt"> الجدول </param>
        /// <param name="Sql">جملة السيكول</param>
        /// <returns>بيرجع الجدول بالبيانات </returns>
        public static void FillBySql(this DataTable Dt, string Sql)
        {
            try
            {
                using (DbDataAdapter Adptr = GMthds.GetDataAdaptor(Sql, GProperties.Datatype))
                {
                    Adptr.Fill(Dt);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, GProperties.ProjectName);
            }
        }

        public static void FillSchmaBySql(this DataTable Dt, string Sql)
        {
            try
            {
                using (DbDataAdapter Adptr = DataAccess.GetDataAdaptor(Sql, DBConfig.Datatype))
                {
                    Adptr.FillSchema(Dt, SchemaType.Source);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, GProperties.ProjectName);
            }
        }
        /// <summary>
        /// تعبئة داتا رو بجملة السيكول
        /// </summary>
        /// <param name="Dr"> داتا رو </param>
        /// <param name="Sql"> جمله السيكول </param>
        /// <param name="DtTyp"> نوع الداتا بيز </param>
        /// <returns> استرجاع الداتا رو بالبيانات  </returns>
        public static DataRow FillBySql(this DataRow Dr, string Sql, EgxDataType DtTyp)
        {
            try
            {
                using (DbDataAdapter Adptr = GMthds.GetDataAdaptor(Sql, DtTyp))
                {
                    using (var Dt = new DataTable())
                    {
                        Adptr.Fill(Dt);
                        Dr = Dt.Rows[0];
                        return Dt.Rows[0];
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, GProperties.ProjectName);
                return null;
            }
        }
        /// <summary>
        /// تعبئة داتا رو بجملة السيكول
        /// </summary>
        /// <param name="Dr"> داتا رو </param>
        /// <param name="Sql"> جمله السيكول </param>
        /// <returns> استرجاع الداتا رو بالبيانات  </returns>
        public static DataRow FillBySql(this DataRow Dr, string Sql)
        {
            try
            {
                using (DbDataAdapter Adptr = GMthds.GetDataAdaptor(Sql, GProperties.Datatype))
                {
                    using (DataTable Dt = new DataTable())
                    {
                        Adptr.Fill(Dt);
                        return Dt.Rows[0];
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, GProperties.ProjectName);
                return null;
            }
        }
        /// <summary>
        /// قم بامتلاء الكمبوبوكس بجملة السيكول ونوع الداتا 
        /// </summary>
        /// <param name="cmb"> الكمبوبوكس </param>
        /// <param name="sql"> جملة السيكول </param>
        /// <param name="dataType"> نوع الداتا بيز </param>
        /// <returns></returns>
        public static ComboBox Fill(this ComboBox cmb, string sql, EgxDataType dataType)
        {
            GMthds.FillComboBox(cmb, sql, dataType);
            return cmb;
        }
        /// <summary>
        /// قم بامتلاء الكمبوبوكس بجملة السيكول ونوع الداتا 
        /// </summary>
        /// <param name="cmb"> الكمبوبوكس </param>
        /// <param name="sql"> جملة السيكول </param>
        /// <returns></returns>
        public static ComboBox Fill(this ComboBox cmb, string sql)
        {
            GMthds.FillComboBox(cmb, sql);
            return cmb;
        }
        /// <summary>
        /// قم بأمتلاء الكمبوبوكس ب itms
        /// </summary>
        /// <param name="cmb">الكمبوبوكس</param>
        /// <returns></returns>
        public static ComboBox Fill(this ComboBox cmb)
        {
            GMthds.FillComboBox(cmb);
            return cmb;
        }

        public static void GetDataFill(this ComboBox cmb, string Sql)
        {
            GMthds.FillComboBox(cmb, Sql);
        }

    }


}
namespace System.Runtime.CompilerServices
{

}