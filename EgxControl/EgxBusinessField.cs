using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Egx.EgxControl
{
    
   public class EgxBusinessField:Dictionary<string,object>
    {
       [DefaultValue(null)]
       public string Name { get; set; }
       [DefaultValue(null)]
       public object Value { get; set; }
       [DefaultValue(null)]
       public Type FieldType { get; set; }

       public EgxBusinessField(string name,object value,Type type) 
       {
           this.Name = name;
           this.Value = value;
           this.FieldType = type;
          
       }

       public EgxBusinessField() { }


    }
}
