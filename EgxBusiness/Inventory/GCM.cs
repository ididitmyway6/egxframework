using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class GCM
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public GCM()
		{
		    da = new DataAccess();
            sm=  new SystemMessage();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the ID value.
		/// </summary>
		public virtual Int32? ID { get; set; }

		/// <summary>
		/// Gets or sets the GCM_ID value.
		/// </summary>
		public virtual String GCM_ID { get; set; }

		/// <summary>
		/// Gets or sets the REG_DATE value.
		/// </summary>
		public virtual DateTime? REG_DATE { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "GCM" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   GCM test = new GCM();
                   test.GCM_ID = this.GCM_ID;
                   if (da.IsExists(test))
            {
                sm.Message = "Already Exist ... ";
                sm.Type = MessageType.Stop;
            }
            else 
            {
                da.Insert(this);
                sm.Message = "OK ...";
                sm.Type = MessageType.Pass;
            }
                   return sm;
                }
				
		public virtual SystemMessage Update() { 
            try
                {
                    da.Update(this, "ID");
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
		public virtual List<GCM> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<GCM>().ToList();
                   }



		public virtual GCM GetByID() {  
                     return (GCM)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<GCM> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<GCM>().ToList();
                  }
		#endregion
	}
}