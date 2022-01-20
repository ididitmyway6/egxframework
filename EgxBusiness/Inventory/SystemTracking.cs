using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
namespace Egx.EgxBusiness.Inventory
{
    class SystemTracking
    {
        public int? ID { get; set; }
        public DateTime? ENTRY_DATE { get; set; }
        public TimeSpan? ENTRY_TIME { get; set; }
        public int? ENTRY_USER { get; set; }
        public bool? IS_PAYMENT { get; set; }
        public override string ToString()
        {
            return "SystemTracking";
        }

        public SystemTracking() 
        {
            DataAccess da = new DataAccess();
            this.ENTRY_DATE = SystemSetting.date;
            this.ENTRY_TIME = DateTime.Now.TimeOfDay;
           // this.ENTRY_USER = SystemSetting.CurrentUser.ID;
            this.IS_PAYMENT = SystemSetting.IsPaymentTransaction;
            da.Insert(this);
            
        }
    }
}
