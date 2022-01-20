using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness
{
    public interface EgxObject
    {
        SystemMessage Insert();
        SystemMessage Update();
        SystemMessage Delete();
        List<T> GetAll<T>();
       
    }
}
