using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egx.EgxControl
{
   public interface IEgxEntryForm
    {
       bool IsValid();
       void PreInit();
    }
}
