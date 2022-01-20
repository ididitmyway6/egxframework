using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egx.EgxBusiness.EgxSMS
{
    public class AttendanceVM
    {
        public int ID { get; set; }
        public string ST_NAME { get; set; }
        public bool IS_ABSENT { get; set; }
        public DateTime DATE { get; set; }
        public Students student { get; set; }
        public EventsStudent EventStudent { get; set; }
        public string Notes { get; set; }

    }
}
