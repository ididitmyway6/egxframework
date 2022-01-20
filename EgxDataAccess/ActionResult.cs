using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egx.EgxDataAccess
{
     partial class ActionResult
    {
        public string SqlQuery { get; set; }
        public bool SuccessAction { get; set; }
        public string ActionResultMessage { get; set; }
        public ActionResult() { }
    }
}
