using Egx.EgxControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Egx.EgxBusiness
{
    public  class EgxValidations
    {
        public List<Control> Required { get; set; }
        static decimal x = 0;
        public EgxValidations()
        {
            Required = new List<Control>();
        }
        public void AddRequired(Control control) 
        {
            Required.Add(control);
        }


    }
}
