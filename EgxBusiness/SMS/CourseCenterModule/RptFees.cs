using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egx.EgxBusiness.EgxSMS.CourseCenterModule
{
    public class RptFees
    {
        public int Month { get; set; }
        public string InvoiceDate { get; set; }
        public decimal Required { get; set; }
        public decimal Paid { get; set; }
        public int Year { get; set; }
    }
}
