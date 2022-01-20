using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
namespace Egx.EgxBusiness.Inventory
{
   public  class SystemSetting:DBConfig
    {
       public static User CurrentUser { get; set; }
       public static bool? IsPaymentTransaction { get; set; }
       public static DateTime? date { get; set; }
       public static bool IsGLBounded { get { if (EgxSetting.Get("GLDBNAME").Length > 0) { return true; } else { return false; } } }
    }
}
