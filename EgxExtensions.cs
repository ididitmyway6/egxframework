using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Egx.EgxControl;
using System.Collections.Specialized;
using Egx.EgxDataAccess;
using System.Windows.Forms;

namespace Egx
{
    public static class EgxExtensions
    {
        public static decimal x=0;

        public static string SqlDateTime(this DateTime date)
        {
            return " cast('" + date.Year + "-" + date.Month + "-" + date.Day + " " +
                date.Hour + ":" + date.Minute + ":" + date.Second + "." + date.Millisecond +
                "' as datetime)";
        }
        public static string SqlDateTime(this DateTime date, bool shortFormat)
        {
            if (shortFormat)
            {
                return "" + date.Year + "-" + date.Month + "-" + date.Day + "";
            }
            else
            {
                return " cast('" + date.Year + "-" + date.Month + "-" + date.Day + " " +
                    date.Hour + ":" + date.Minute + ":" + date.Second + "." + date.Millisecond +
                    "' as datetime)";
            }
        }

        public static bool IsNull(this object obj)
        { 
            try
            {
                if (obj == null || obj.Equals(-1) || obj.Equals(-1f) || obj.Equals(DateTime.MinValue) ||
                    obj.Equals(false) || obj.Equals(-1))
                {
                    if (obj.Equals(DateTime.MinValue))
                    {
                        obj = DateTime.Parse("1/1/1753 12:00:00");
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                if (obj == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public static T As<T>(this object objectToConvert) 
        {
            return (T)objectToConvert;
        }

        public static bool IsValidNumber(this EgxTextBox txt)
        {
            return (Decimal.TryParse(txt.Text, out x));
        }

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
        public static string[] split(this string value, char spltval)
        {
            return value.Split(spltval);
        }
        /// <summary>
        /// بدل لاستخدام String.Format
        /// </summary>
        /// <param name="value"> النص المراد تهياته </param>
        /// <param name="objects"> القيم المراد استخدامها </param>
        /// <returns> النص بعد التهيأه </returns>
        public static string Frmt(this string value, params object[] objects)
        {
            return string.Format(value, objects);
        }

        /// <summary>
        /// تحويل اى قيم فارغة او مصفرة الى قيمة فارغة قواعد بيانات
        /// </summary>
        /// <param name="Value"> الابوجيكت </param>
        /// <returns>BDNULL.VALUE</returns>
        public static object AnyToDbnull(this object val)
        {
            if (val.Nz("") == "0" || val.Nz("") == "" || val == null)
                return DBNull.Value;
            else
                return val;
        }

        /// <summary>
        /// تغيير الكائن الى النوع المراد تغييرة
        /// </summary>
        /// <typeparam name="T"> النوع الذى سوف يتغير اليه الكائن </typeparam>
        /// <param name="val"> الكائن </param>
        /// <returns> الكائن بنوعه الجديد </returns>
        public static T CnvrtTo<T>(this object val) where T : IConvertible
        {
            return (T)Convert.ChangeType(val, typeof(T));
        }


        /// <summary>
        ///  تغيير الكائن الى النوع المراد تغييرة او لقيمة جديده فى حالة انه فارغ
        /// </summary>
        /// <typeparam name="T">النوع الذى سوف يتغير اليه الكائن</typeparam>
        /// <param name="val">الكائن</param>
        /// <param name="rtrnval"> القيمة الجديده  </param>
        /// <returns> الكائن بنوعه الجديد او القيمة الجديده بالنوع الجديد </returns>
        public static T CnvrtTo<T>(this object val, object rtrnval) where T : IConvertible
        {
            if (val.Nz("") == "")
            {
                return (T)Convert.ChangeType(rtrnval, typeof(T));
            }
            return (T)Convert.ChangeType(val, typeof(T));
        }


        //////-------------------------------------------------------------------------------------
    
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
        

        public static T CnvrtToT<T>(this object Val) where T : IConvertible
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
                using (DbDataAdapter Adptr = DataAccess.GetDataAdaptor(Sql, DtTyp))
                {
                    Adptr.Fill(Dt);
                    return Dt;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, DBConfig.ProjectName);
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
                using (DbDataAdapter Adptr = DataAccess.GetDataAdaptor(Sql, DBConfig.Datatype))
                {
                    Adptr.Fill(Dt);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, DBConfig.ProjectName);
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
                MessageBox.Show(Ex.Message, DBConfig.ProjectName);
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
                using (DbDataAdapter Adptr = DataAccess.GetDataAdaptor(Sql, DtTyp))
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
                MessageBox.Show(Ex.Message, DBConfig.ProjectName);
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
                using (DbDataAdapter Adptr = DataAccess.GetDataAdaptor(Sql, DBConfig.Datatype))
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
                MessageBox.Show(Ex.Message, DBConfig.ProjectName);
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
            DataAccess.FillComboBox(cmb, sql, dataType);
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
            DataAccess.FillComboBox(cmb, sql);
            return cmb;
        }
        /// <summary>
        /// قم بأمتلاء الكمبوبوكس ب itms
        /// </summary>
        /// <param name="cmb">الكمبوبوكس</param>
        /// <returns></returns>
        public static ComboBox Fill(this ComboBox cmb)
        {
            DataAccess.FillComboBox(cmb);
            return cmb;
        }

        public static void GetDataFill(this ComboBox cmb, string Sql)
        {
            DataAccess.FillComboBox(cmb, Sql);
        }

        public static Int32 ToInt32(this string str)
        {
            if (str != null)
            {
                if (str.Length > 0)
                {
                    return Int32.Parse(str);
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }


        public static Int32 ToInt32(this object obj)
        {
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj);
            }
            else
            {
                return -1;
            }
        }
        public static Int32 IntBool(this bool bol) 
        {
            return Convert.ToInt32(bol);
        }
        public static Int32 ToInt32(this object obj,int defaultValue)
        {
            if (obj != null && obj!=DBNull.Value)
            {
                return Convert.ToInt32(obj);
               
            }
            else
            {
                return defaultValue;
            }
        }

        public static Decimal ToDecimal(this object obj)
        {
            if (obj != null)
            {
                return Convert.ToDecimal(obj);
            }
            else
            {
                return -1;
            }
        }

        public static Decimal ToDecimal(this object obj,decimal def)
        {
            try
            {
                if (obj != null && obj != DBNull.Value)
                {
                    return Convert.ToDecimal(obj);
                }
                else
                {
                    return def;
                }
            }
            catch (Exception e) { MessageBox.Show(e.Message); return 0; }
            
        }

        public static float ToFloat(this string str)
        {
            if (str != null)
            {
                if (str.Length > 0)
                {
                    return float.Parse(str);
                }
                else
                {
                    return -1f;
                }
            }
            else
            {
                return -1f;
            }
        }

        public static float ToFloat(this string str,float defaultValue)
        {
            if (str != null)
            {
                if (str.Length > 0)
                {
                    return float.Parse(str);
                }
                else
                {
                    return -1f;
                }
            }
            else
            {
                return defaultValue;
            }
        }

        public static Decimal ToDecimal(this string str)
        {
            if (str != null)
            {
                if (str.Length > 0)
                {
                    return Decimal.Parse(str);
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        public static Double ToDouble(this string str)
        {
            if (str != null)
            {
                if (str.Length > 0)
                {
                    return Double.Parse(str);
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        public static decimal Approximate(this decimal value) 
        {
            return Math.Round(value);
        }




 
    }
}