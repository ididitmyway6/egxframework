using Egx.EgxBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egx
{
    class EgxEventArgs
    {
    }

    public class PreActionEventArgs 
    {
        public EgxControl.FormAction ActionType { get; set; }
        public object CurrentObject { get; set; }
        public string Message { get; set; }
    }

    public class AfterActionEventArgs 
    {
        public EgxControl.FormAction ActionType { get; set; }
        public object CurrentObject { get; set; }
        public string Message { get; set; }
        public SystemMessage SystemMessage { get; set; }
    }

    public class EgxNavigatorEventArgs 
    {
        public object CurrentObject { get; set; }
        public int CurrentIndex { get; set; }
        public string Message { get; set; }
    }
}
