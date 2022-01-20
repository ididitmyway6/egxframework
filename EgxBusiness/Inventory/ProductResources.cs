using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class ProductResources
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public ProductResources()
		{
		    da = new DataAccess();
            sm=  new SystemMessage();
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the id value.
		/// </summary>
		public virtual Int32? ID { get; set; }

		/// <summary>
		/// Gets or sets the productID value.
		/// </summary>
		public virtual Int32? productID { get; set; }

		/// <summary>
		/// Gets or sets the resourceName value.
		/// </summary>
		public virtual String resourceName { get; set; }

		/// <summary>
		/// Gets or sets the resourceDesc value.
		/// </summary>
		public virtual String resourceDesc { get; set; }

        /// <summary>
        /// Gets or sets the resourceLink value.
        /// </summary>
        public virtual String resourceLink { get; set; }

        /// <summary>
        /// Gets or sets the resourceLink value.
        /// </summary>
        public virtual String flipBookName { get; set; }

		#endregion

		#region Overrides
		public override String ToString()
		{
			return "ProductResources" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   ProductResources test = new ProductResources();
                   test.resourceName = this.resourceName;
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
		public virtual List<ProductResources> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<ProductResources>().ToList();
                   }



		public virtual ProductResources GetByID() {  
                     return (ProductResources)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<ProductResources> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<ProductResources>().ToList();
                  }
		#endregion
	}
}