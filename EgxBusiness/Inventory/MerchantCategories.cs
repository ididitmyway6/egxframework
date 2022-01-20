using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class MerchantCategories
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public MerchantCategories()
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
		/// Gets or sets the CategoryName value.
		/// </summary>
		public virtual String CategoryName { get; set; }

		/// <summary>
		/// Gets or sets the CategoryIcon value.
		/// </summary>
		public virtual String CategoryIcon { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "MerchantCategories" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   MerchantCategories test = new MerchantCategories();
                   //test.COMARISON_KEY = this.COMARISON_KEY
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
		public virtual List<MerchantCategories> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<MerchantCategories>().ToList();
                   }



		public virtual MerchantCategories GetByID() {  
                     return (MerchantCategories)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<MerchantCategories> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<MerchantCategories>().ToList();
                  }
		#endregion
	}
}