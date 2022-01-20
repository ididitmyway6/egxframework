using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;

namespace Egx.EgxDataAccess
{
    public class GMthds
    {
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
                case EgxDataType.Msaccess: return new OleDbDataAdapter(Sql, (new OleDbConnection(GProperties.OleConnectionString)));
                default: return new SqlDataAdapter(Sql, DBConfig.ConnectionString /*GProperties.SqlConnectionString*/); ;
            }

        }
        public static DbCommand GetDbCommand(string sql, EgxDataType DtTyp)
        {
            switch (DtTyp)
            {
                case EgxDataType.Mssql: return new SqlCommand(sql, new SqlConnection(GProperties.SqlConnectionString));
                case EgxDataType.Msaccess: return new OleDbCommand(sql, (new OleDbConnection(GProperties.OleConnectionString)));
                default: return new SqlCommand();
            }

        }
        public static DbParameter GetDbParameter(string parameterName,object value,EgxDataType DtTyp)
        {
            switch (DtTyp)
            {
                case EgxDataType.Mssql: return new SqlParameter(parameterName,value);
                case EgxDataType.Msaccess: return new OleDbParameter(parameterName,value);
                default:
                    return null;
            }

        }

        public static DbDataAdapter GetDataAdaptor(EgxDataType DtTyp)
        {
            switch (DtTyp)
            {
                case EgxDataType.Mssql: return new SqlDataAdapter("", GProperties.SqlConnectionString);
                case EgxDataType.Msaccess: return new OleDbDataAdapter("", (new OleDbConnection(GProperties.OleConnectionString)));
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

        public static DbConnection GetConnection(string StrCon, EgxDataType DtTyp)
        {
            switch (DtTyp)
            {
                case EgxDataType.Mssql: return new SqlConnection(StrCon);
                case EgxDataType.Msaccess: return new OleDbConnection(StrCon);
                default: return null;
            }

        }

        public static DbCommandBuilder GetDbCommandBuilder(DbDataAdapter Da_, EgxDataType datatype)
        {
            switch (datatype)
            {
                case EgxDataType.Mssql: return new SqlCommandBuilder(Da_ as SqlDataAdapter);
                case EgxDataType.Msaccess: return new OleDbCommandBuilder(Da_ as OleDbDataAdapter);
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
            catch (Exception Ex) { MessageBox.Show(Ex.Message, GProperties.ProjectName); }
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
            catch (Exception Ex) { MessageBox.Show(Ex.Message, GProperties.ProjectName); }
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
            catch (Exception ex) { MessageBox.Show(ex.Message, GProperties.ProjectName); }
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
                cm.DataSource = Dt;
                foreach (DataRow DR in Dt.Rows)
                {
                    DR[1] = DR[0].Nz() + (DR[0] == DBNull.Value ? "" : " : ") + DR[1].Nz();
                }
                Dt.DefaultView.Sort = Dt.Columns[0].ColumnName;
                cm.ValueMember = Dt.Columns[0].ColumnName;
                cm.DisplayMember = Dt.Columns[1].ColumnName;
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, GProperties.ProjectName); }
        }
        public static void FillComboBox(DataGridViewComboBoxColumn cm, string SQL)
        {
            try
            {
                DataTable Dt = new DataTable();
                Dt.FillBySql(SQL, GProperties.Datatype);
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
            catch (Exception ex) { MessageBox.Show(ex.Message, GProperties.ProjectName); }
        }
        public static void FillComboBox(ComboBox cm, string SQL)
        {
            try
            {
                DataTable Dt = new DataTable();
                Dt.FillBySql(SQL, GProperties.Datatype);
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
            catch (Exception ex) { MessageBox.Show(ex.Message, GProperties.ProjectName); }
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
            catch (Exception ex) { MessageBox.Show(ex.Message, GProperties.ProjectName); }
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
                    MessageBox.Show(ex.Message, GProperties.ProjectName);
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
                    MessageBox.Show(ex.Message, GProperties.ProjectName);
                    return null;
                }
                finally
                {
                    cmd.Connection.Close();
                }

            }
        }
        public static object GetSqlValue(string sql)
        {
            using (DbCommand cmd = GetDbCommand("", GProperties.Datatype))
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
                    MessageBox.Show(ex.Message, GProperties.ProjectName);
                    return null;
                }
                finally
                {
                    cmd.Connection.Close();
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
                MessageBox.Show(ex.Message, GProperties.ProjectName);
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
                    MessageBox.Show(ex.Message, GProperties.ProjectName);
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
            using (DbCommand Dbcmd = GetDbCommand(sql, GProperties.Datatype))
            {
                try
                {
                    Dbcmd.CommandText = sql;
                    Dbcmd.Connection.Open();
                    return Dbcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, GProperties.ProjectName);
                    return 0;
                }
                finally
                {
                    Dbcmd.Connection.Close();
                }

            }
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
                using (DbDataAdapter Da = GetDataAdaptor(Sql, GProperties.Datatype))
                {
                    Da.Fill(Ds, TbName);
                    return Ds.Tables[0];
                }
            }
        }
        public static DataTable GetDataTable(string Sql)
        {
            using (DataTable DT = new DataTable())
            {
                using (DbDataAdapter Da = GetDataAdaptor(Sql, GProperties.Datatype))
                {
                    Da.Fill(DT);
                    return DT;
                }
            }
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
                MessageBox.Show(ex.Message, GProperties.ProjectName);
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
                MessageBox.Show(ex.Message, GProperties.ProjectName);
                return null;
            }
        }
        public static DataRow GetDataRow(string SQL, string STRCON)
        {
            try
            {
                using (DataTable Dt = new DataTable())
                {
                    using (DbDataAdapter Da = GetDataAdaptor(SQL, STRCON, GProperties.Datatype))
                    {
                        Da.Fill(Dt);
                        return Dt.Rows[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GProperties.ProjectName);
                return null;
            }
        }
        public static DataRow GetDataRow(string SQL)
        {
            try
            {
                using (DataTable Dt = new DataTable())
                {
                    using (DbDataAdapter Da = GetDataAdaptor(SQL, GProperties.Datatype))
                    {
                        Da.Fill(Dt);
                        return Dt.Rows[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GProperties.ProjectName);
                return null;
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



    }

}