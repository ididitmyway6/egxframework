using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using Egx.EgxBusiness;

namespace Egx.EgxBusiness.GL
{

	[Serializable]
	public class Inventories
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Inventories()
		{
		    da = new DataAccess();
            sm=  new SystemMessage();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the INV_CODE value.
		/// </summary>
		public virtual Int32? ID { get; set; }

		/// <summary>
		/// Gets or sets the INV_NAME value.
		/// </summary>
		public virtual String INV_NAME { get; set; }

		/// <summary>
		/// Gets or sets the INV_BEGIN_CREDIT value.
		/// </summary>
		public virtual Decimal? INV_BEGIN_CREDIT { get; set; }

		/// <summary>
		/// Gets or sets the NOTES value.
		/// </summary>
		public virtual String NOTES { get; set; }

        /// <summary>
        /// Gets or sets the DEP_DATE value.
        /// </summary>
        public virtual DateTime? DEP_DATE { get; set; }

        /// <summary>
        /// Gets or sets the REL_ACC value.
        /// </summary>
        public virtual Int32? REL_ACC { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Inventories" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   Inventories test = new Inventories();
                   test.ID = this.ID;
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
		public virtual List<Inventories> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Inventories>().ToList();
                   }



		public virtual Inventories GetByID() {  
                     return (Inventories)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Inventories> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Inventories>().ToList();
                  }
		#endregion
	}
}