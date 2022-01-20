using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Casher
{
	

	[Serializable]
	public class Stock
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public Stock()
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
		/// Gets or sets the Location value.
		/// </summary>
		public virtual Int32? Location { get; set; }

		/// <summary>
		/// Gets or sets the Product value.
		/// </summary>
		public virtual String Product { get; set; }

		/// <summary>
		/// Gets or sets the QuantityOnHand value.
		/// </summary>
		public virtual Int32? QuantityOnHand { get; set; }

		/// <summary>
		/// Gets or sets the Reserved value.
		/// </summary>
		public virtual Int32? Reserved { get; set; }

		/// <summary>
		/// Gets or sets the Available value.
		/// </summary>
		public virtual Int32? Available { get; set; }

		/// <summary>
		/// Gets or sets the Discount value.
		/// </summary>
		public virtual float? Discount { get; set; }

		/// <summary>
		/// Gets or sets the Category value.
		/// </summary>
		public virtual Int32? Category { get; set; }

		/// <summary>
		/// Gets or sets the ReorderPoint value.
		/// </summary>
		public virtual Int32? ReorderPoint { get; set; }

		/// <summary>
		/// Gets or sets the ProductImage value.
		/// </summary>
		public virtual Byte[] ProductImage { get; set; }

		/// <summary>
		/// Gets or sets the IsInventory value.
		/// </summary>
		public virtual Boolean? IsInventory { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "Stock";
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

		
		public virtual List<Stock> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Stock>().ToList();
                   }



		public virtual Stock GetByID() {  
                     return (Stock)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Stock> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Stock>().ToList();
                  }
	#endregion
	}
}