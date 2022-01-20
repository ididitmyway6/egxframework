using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class Tags
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Tags()
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
		/// Gets or sets the TagName value.
		/// </summary>
		public virtual String TagName { get; set; }

		/// <summary>
		/// Gets or sets the TagDesc value.
		/// </summary>
		public virtual String TagDesc { get; set; }

		/// <summary>
		/// Gets or sets the TagLocation value.
		/// </summary>
		public virtual String TagLocation { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Tags" ;
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
		public virtual List<Tags> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Tags>().ToList();
                   }



		public virtual Tags GetByID() {  
                     return (Tags)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Tags> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Tags>().ToList();
                  }
		#endregion
	}
}