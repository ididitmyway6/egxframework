using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egx.EgxBusiness.EgxSMS
{
    public class SystemConstants
    {
        public enum EventState {STARTED=1,ENDED=0,NOT_STARTED=-1,CANCELLED=-2 }
        public enum StudentState { PASS=1,FAIL=0,REFUND=2,WAITING_EVAL=-1,CANCELLED=-2}
    }
}
