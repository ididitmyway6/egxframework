using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using System.Windows.Forms;

namespace Egx.EgxBusiness.Clinic
{

	[Serializable]
	public class tst
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public tst()
		{
                     da = new DataAccess();
                     sm=  new SystemMessage();
		}

		
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the ID value.
		/// </summary>
		public virtual Int32? id { get; set; }

		/// <summary>
		/// Gets or sets the CHKUP_DATE value.
		/// </summary>
		public virtual string txt { get; set; }
 


		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "tst";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                { 
 
                    da.Insert(this);
                    sm.Message = "OK ...";
                    sm.Type = MessageType.Pass;
      
                    return sm;

        }
      
		
		public virtual SystemMessage Update() { 
                     try
                      {
                        da.Update(this, "id");
                        sm.Message = "OK ...";
                        sm.Type = MessageType.Pass;
                      }
                     catch
                       {
                            sm.Message = "Error"; sm.Type = MessageType.Fail;
                       }
                          return sm;
                  }

		public virtual SystemMessage Delete() { 
                              try {
                                    da.Delete(this, "ID");
                                    sm.Message = "OK ...";
                                    sm.Type = MessageType.Pass;
                                  }
                              catch { sm.Message = "Error ...";
                                      sm.Type = MessageType.Fail;
                                     }
                           return sm;
                    }

		public static Visit GetLastSession(int pid)
        {
            DataAccess daccess = new DataAccess();
            List<Visit> lst = daccess.ExecuteSQL<Visit>("select top  1 * from visits where P_ID="+pid+" order by CHKUP_DATE desc");
            if (lst.Count > 0)
            {
                return lst[0];
            }
            else { return null; }
        }

		public virtual List<Visit> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Visit>().ToList();
                   }



		public virtual Visit GetByID() {  
                     return (Visit)new DataAccess().GetByID(id.Value, this);
                  }


	        public virtual List<Visit> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Visit>().ToList();
                  }
	#endregion
	}
}