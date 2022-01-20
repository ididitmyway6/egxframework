using Egx.EgxDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egx.EgxBusiness.SMS
{
    public class Periodic
    {
        public static void ApplyEventPeridoicalSetting() 
        {
            DataAccess.ExecData("UPDATE Events SET EventState=1 WHERE EventState=-1 and EventStartDate <= cast(GETDATE() as date)");
            DataAccess.ExecData("UPDATE Events SET EventState=0 WHERE EventState=1 and EventEndDate < GETDATE()	");
        }
    }
}
