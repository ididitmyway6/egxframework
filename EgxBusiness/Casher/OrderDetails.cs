using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Casher
{
	

	[Serializable]
	public class OrderDetails
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public OrderDetails()
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
        /// Gets or sets the OrderID value.
        /// </summary>
        public virtual Int32? OrderID { get; set; }
        /// <summary>
        /// Gets or sets the OrderID value.
        /// </summary>
        public virtual Int32? OrderNumber { get; set; }

		/// <summary>
		/// Gets or sets the ProductID value.
		/// </summary>
		public virtual Int32? ProductID { get; set; }

		/// <summary>
		/// Gets or sets the Location value.
		/// </summary>
		public virtual Int32? Location { get; set; }

		/// <summary>
		/// Gets or sets the ProductName value.
		/// </summary>
		public virtual String ProductName { get; set; }

		/// <summary>
		/// Gets or sets the Quantity value.
		/// </summary>
		public virtual Int32? Quantity { get; set; }

		/// <summary>
		/// Gets or sets the Price value.
		/// </summary>
		public virtual float? Price { get; set; }

		/// <summary>
		/// Gets or sets the Total value.
		/// </summary>
		public virtual float? Total { get; set; }

		/// <summary>
		/// Gets or sets the Discount value.
		/// </summary>
		public virtual float? Discount { get; set; }

		/// <summary>
		/// Gets or sets the Net value.
		/// </summary>
		public virtual float? Net { get; set; }

		/// <summary>
		/// Gets or sets the Comment value.
		/// </summary>
		public virtual String Comment { get; set; }

		/// <summary>
		/// Gets or sets the ReturnedQnt value.
		/// </summary>
		public virtual Int32? ReturnedQnt { get; set; }

		/// <summary>
		/// Gets or sets the OrderType value.
		/// </summary>
		public virtual String OrderType { get; set; }

		/// <summary>
		/// Gets or sets the TransactionType value.
		/// </summary>
		public virtual Int32? TransactionType { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "OrderDetails";
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

		
		public virtual List<OrderDetails> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<OrderDetails>().ToList();
                   }



		public virtual OrderDetails GetByID() {  
                     return (OrderDetails)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<OrderDetails> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<OrderDetails>().ToList();
                  }
	#endregion
	}
}