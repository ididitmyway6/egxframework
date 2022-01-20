using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egx.EgxBusiness
{
    public static class Date
    {
        public static int CalculateDifferenceInMonth(DateTime start, DateTime end)
        {
            var diffMonths = (end.Month + end.Year * 12) - (start.Month + start.Year * 12);
            return diffMonths+1;
        }
    }
}
