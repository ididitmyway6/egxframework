using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
namespace Egx.EgxBusiness.EgxSMS
{
    public class SystemSetting
    {
        public int? ID { get; set;}
        public static User CurrentUser { get; set; }
        public string SETTING_NAME { get; set; }
        public string SETTING_DESC { get; set; }
        public string SETTING_VALUE { get; set; }
        public DataAccess _da;
        public static List<DateTime> GetOfficialHolidays() 
        {
            List<DateTime> OfficialHolidays = new List<DateTime>();
            string[] _holidays = SystemSetting.GetSetting("OfficialHolidays").Split(';');
            if (_holidays.Length > 0)
            {
                foreach (string holiday in _holidays)
                {
                    OfficialHolidays.Add(DateTime.Parse(holiday).Date);
                }
            }
            return OfficialHolidays;
        }
        public SystemSetting() 
        {

        }

        public override string ToString()
        {
            return "SystemSetting";
        }


        public static string GetSetting(string settingName) 
        {
            DataAccess _da = new DataAccess();
            SystemSetting ss = new SystemSetting();
             _da = new DataAccess();
            ss= (_da.GetByColumn("SETTING_NAME", settingName, ss) as SystemSetting);
            if (ss != null) { return ss.SETTING_VALUE; } else { return null; }
        }
        public static bool IsHoliday(DateTime date) 
        {
            return SystemSetting.GetOfficialHolidays().Contains(date);
        }

    }
}
