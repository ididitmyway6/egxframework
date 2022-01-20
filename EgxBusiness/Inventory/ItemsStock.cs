using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class ItemsStock
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public ItemsStock()
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
		/// Gets or sets the ITEM_NAME value.
		/// </summary>
		public virtual String ITEM_NAME { get; set; }

		/// <summary>
		/// Gets or sets the ITEM_CODE value.
		/// </summary>
		public virtual Int32? ITEM_ID { get; set; }

		/// <summary>
		/// Gets or sets the INV_SITE value.
		/// </summary>
		public virtual Int32? INV_SITE { get; set; }

		/// <summary>
		/// Gets or sets the QOH value.
		/// </summary>
		public virtual float? QOH { get; set; }

		/// <summary>
		/// Gets or sets the Q_AVAILABLE value.
		/// </summary>
		public virtual float? Q_AVAILABLE { get; set; }

        /// <summary>
        /// Gets or sets the Q_RESERVED value.
        /// </summary>
        public virtual float? Q_RESERVED { get; set; }

        /// <summary>
        /// Gets or sets the Q_CORRUBTED value.
        /// </summary>
        public virtual float? Q_CORRUBTED { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "ItemsStock" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   ItemsStock test = new ItemsStock();
                   test.ITEM_ID = this.ITEM_ID;
                   test.INV_SITE = this.INV_SITE;
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
		public virtual List<ItemsStock> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<ItemsStock>().ToList();
                   }


     
        public static  ItemsStock GetByID(int itemID, int storeLocation = -1)
        {
            var item = new ItemsStock() { INV_SITE = storeLocation, ITEM_ID = itemID }.Search().FirstOrDefault();
            if (item != null)
            {
                return item;
            }
            else { return null; }
                  }
       


	        public virtual List<ItemsStock> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(1000000,this).Cast<ItemsStock>().ToList();
                  }
		#endregion
	}
}