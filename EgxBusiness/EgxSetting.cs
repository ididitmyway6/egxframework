using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
namespace Egx.EgxBusiness
{
    public class EgxSetting
    {


        public static void Update(string name, string value) 
        {
            string sql = "UPDATE  Setting  SET [value]='" + value + "' where name='" + name + "'";
            if (DBConfig.IsWebApp)
            {
                DataAccess.ExecData(sql);
            }
            else
            {
                DataAccess.ExecData(sql, EgxDataType.Temp);
            }
        }

        public static void Create(string name,string defaultValue) 
        {
            if (DBConfig.IsWebApp) {
                if (IsExist(name))
                {
                    // Update(name, defaultValue);
                    return;
                }
                else
                {
                    string sql = "INSERT INTO Setting(name,[value]) VALUES('" + name + "','" + defaultValue + "')";
                    DataAccess.ExecData(sql);
                }
            }
            else
            {
                if (IsExist(name))
                {
                    // Update(name, defaultValue);
                    return;
                }
                else
                {
                    string sql = "INSERT INTO Setting(name,[value]) VALUES('" + name + "','" + defaultValue + "')";
                    DataAccess.ExecData(sql, EgxDataType.Temp);
                }
            }
        }

        public static string Get(string name) 
        {
            if (DBConfig.IsWebApp)
            {
                string sql = "select [value] from Setting WHERE name='" + name + "'";
                var dr = DataAccess.GetDataRow(sql);
                if (dr.ItemArray.Length > 0)
                {
                    return DataAccess.GetDataRow(sql).ItemArray[0].ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                string sql = "select [value] from Setting WHERE name='" + name + "'";
                var dr = DataAccess.GetDataRow(sql, EgxDataType.Temp);
                if (dr.ItemArray.Length > 0)
                {
                    return DataAccess.GetDataRow(sql, EgxDataType.Temp).ItemArray[0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        public static bool IsExist(string name) 
        {
            if (DBConfig.IsWebApp) {
                var x = DataAccess.GetDataTable("select * from setting where name='" + name + "'");
                if (x.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var x = DataAccess.GetDataTable("select * from setting where name='" + name + "'", EgxDataType.Temp);
                if (x.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
