using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class SupplierDiscounts
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public SupplierDiscounts()
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
		/// Gets or sets the SupplierID value.
		/// </summary>
		public virtual Int32? SupplierID { get; set; }

		/// <summary>
		/// Gets or sets the DiscountTitle value.
		/// </summary>
		public virtual String DiscountTitle { get; set; }

		/// <summary>
		/// Gets or sets the DiscountLogo value.
		/// </summary>
		public virtual String DiscountLogo { get; set; }

		/// <summary>
		/// Gets or sets the DiscountValue value.
		/// </summary>
		public virtual Double? DiscountValue { get; set; }
        public virtual DateTime? DiscountExpiration { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "SupplierDiscounts" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
             
                da.Insert(this);
                sm.Message = "OK ...";
                sm.Type = MessageType.Pass;
                this.ID = (int)DataAccess.GetSqlValue("select isnull(max(ID),0) from SupplierDiscounts where SupplierID="+this.SupplierID+"", EgxDataType.Mssql);
                sm.Attachment = this;
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
		public virtual List<SupplierDiscounts> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<SupplierDiscounts>().ToList();
                   }



		public virtual SupplierDiscounts GetByID() {  
                     return (SupplierDiscounts)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<SupplierDiscounts> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<SupplierDiscounts>().ToList();
                  }
		#endregion
	}
}