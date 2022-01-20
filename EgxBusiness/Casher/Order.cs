using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Casher
{
	

	[Serializable]
	public class Order
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public Order()
		{
                     da = new DataAccess();
                     sm=  new SystemMessage();
		}

		
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the OrderNumber value.
		/// </summary>

        public virtual Int32? ID { get; set; }


		public virtual Int32? OrderNumber { get; set; }

		/// <summary>
		/// Gets or sets the OrderDate value.
		/// </summary>
		public virtual DateTime? OrderDate { get; set; }

		/// <summary>
		/// Gets or sets the OrderTime value.
		/// </summary>
		public virtual String OrderTime { get; set; }

		/// <summary>
		/// Gets or sets the OrderUser value.
		/// </summary>
		public virtual Int32? OrderUser { get; set; }

		/// <summary>
		/// Gets or sets the Location value.
		/// </summary>
		public virtual Int32? Location { get; set; }

		/// <summary>
		/// Gets or sets the Total value.
		/// </summary>
		public virtual float? Total { get; set; }

		/// <summary>
		/// Gets or sets the Customer value.
		/// </summary>
		public virtual Int32? Customer { get; set; }

		/// <summary>
		/// Gets or sets the Supplier value.
		/// </summary>
		public virtual Int32? Supplier { get; set; }

		/// <summary>
		/// Gets or sets the OrderType value.
		/// </summary>
		public virtual String OrderType { get; set; }

		/// <summary>
		/// Gets or sets the InvoiceStatus value.
		/// </summary>
		public virtual String InvoiceStatus { get; set; }

		/// <summary>
		/// Gets or sets the OrderStatus value.
		/// </summary>
		public virtual String OrderStatus { get; set; }

		/// <summary>
		/// Gets or sets the Shift value.
		/// </summary>
		public virtual Int32? Shift { get; set; }

		/// <summary>
		/// Gets or sets the OrderParent value.
		/// </summary>
		public virtual Int32? OrderParent { get; set; }

		/// <summary>
		/// Gets or sets the OrderRef value.
		/// </summary>
		public virtual Int32? OrderRef { get; set; }

		/// <summary>
		/// Gets or sets the TransactionType value.
		/// </summary>
		public virtual Int32? TransactionType { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "Orders";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                {
                this.OrderNumber = da.GetNextID(this, "OrderNumber");
                this.OrderDate = DateTime.Now;
                this.OrderTime = DateTime.Now.TimeOfDay.ToString();
            
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
                                  Order o = Search()[0];
                                    da.Delete(o, "ID");
                                  
                                    sm.Message = "OK ...";
                                    sm.Type = MessageType.Pass;
                                  }
                              catch { sm.Message = "Error ...";
                                      sm.Type = MessageType.Fail;
                                     }
                           return sm;
                    }

        public int GetNextOrderNumber() 
        {
            return  da.GetNextID(this, "OrderNumber");
        }
        public int GetNextOrderID()
        {
            return da.GetNextID(this, "ID");
        }
		public virtual List<Order> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Order>().ToList();
                   }



		public virtual Order GetByID() {  
                     return (Order)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Order> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Order>().ToList();
                  }
	#endregion
	}
}