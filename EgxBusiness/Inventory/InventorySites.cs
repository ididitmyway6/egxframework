using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class InventorySites
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public InventorySites()
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
		/// Gets or sets the SITE_NAME value.
		/// </summary>
		public virtual String SITE_NAME { get; set; }

        /// <summary>
        /// Gets or sets the DESCRIPTION value.
        /// </summary>
        public virtual String DESCRIPTION { get; set; }

        /// <summary>
        /// Gets or sets the GL_CODE value.
        /// </summary>
        public virtual String GL_CODE { get; set; }

		/// <summary>
		/// Gets or sets the OTHER value.
		/// </summary>
		public virtual String OTHER { get; set; }

		/// <summary>
		/// Gets or sets the CODE value.
		/// </summary>
		public virtual String CODE { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "InventorySites" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   InventorySites test = new InventorySites();
                   test.CODE = this.CODE;
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
		public virtual List<InventorySites> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<InventorySites>().ToList();
                   }



		public virtual InventorySites GetByID() {  
                     return (InventorySites)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<InventorySites> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<InventorySites>().ToList();
                  }
		#endregion
	}
}