using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Casher
{
	

	[Serializable]
	public class StockTransaction
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public StockTransaction()
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
		/// Gets or sets the Product value.
		/// </summary>
		public virtual String Product { get; set; }

		/// <summary>
		/// Gets or sets the Type value.
		/// </summary>
		public virtual String Type { get; set; }

		/// <summary>
		/// Gets or sets the QuantityIn value.
		/// </summary>
		public virtual Int32? QuantityIn { get; set; }

		/// <summary>
		/// Gets or sets the QuantityOut value.
		/// </summary>
		public virtual Int32? QuantityOut { get; set; }

		/// <summary>
		/// Gets or sets the TransactionDate value.
		/// </summary>
		public virtual DateTime? TransactionDate { get; set; }

		/// <summary>
		/// Gets or sets the TransactionTime value.
		/// </summary>
		public virtual String TransactionTime { get; set; }

		/// <summary>
		/// Gets or sets the Shift value.
		/// </summary>
		public virtual Int32? Shift { get; set; }

		/// <summary>
		/// Gets or sets the OrderDetail value.
		/// </summary>
		public virtual Int32? OrderDetail { get; set; }

		/// <summary>
		/// Gets or sets the Comment value.
		/// </summary>
		public virtual String Comment { get; set; }

		/// <summary>
		/// Gets or sets the SystemUser value.
		/// </summary>
		public virtual Int32? SystemUser { get; set; }

		/// <summary>
		/// Gets or sets the TransactionType value.
		/// </summary>
		public virtual Int32? TransactionType { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "StockTransactions";
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

		
		public virtual List<StockTransaction> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<StockTransaction>().ToList();
                   }



		public virtual StockTransaction GetByID() {  
                     return (StockTransaction)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<StockTransaction> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<StockTransaction>().ToList();
                  }
	#endregion
	}
}