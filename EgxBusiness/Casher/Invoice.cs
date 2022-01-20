using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Casher
{
	

	[Serializable]
	public class Invoice
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public Invoice()
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
		/// Gets or sets the OrderNumber value.
		/// </summary>
		public virtual Int32? OrderNumber { get; set; }

		/// <summary>
		/// Gets or sets the Location value.
		/// </summary>
		public virtual Int32? Location { get; set; }

		/// <summary>
		/// Gets or sets the Total value.
		/// </summary>
		public virtual Double? Total { get; set; }

		/// <summary>
		/// Gets or sets the InvoiceDate value.
		/// </summary>
		public virtual DateTime? InvoiceDate { get; set; }

		/// <summary>
		/// Gets or sets the InvoiceTime value.
		/// </summary>
		public virtual String InvoiceTime { get; set; }

		/// <summary>
		/// Gets or sets the Customer value.
		/// </summary>
		public virtual Int32? Customer { get; set; }

		/// <summary>
		/// Gets or sets the Supplier value.
		/// </summary>
		public virtual Int32? Supplier { get; set; }

		/// <summary>
		/// Gets or sets the Type value.
		/// </summary>
		public virtual String Type { get; set; }

		/// <summary>
		/// Gets or sets the InvoiceStatus value.
		/// </summary>
		public virtual String InvoiceStatus { get; set; }

		/// <summary>
		/// Gets or sets the Shift value.
		/// </summary>
		public virtual Int32? Shift { get; set; }

		/// <summary>
		/// Gets or sets the TransactionType value.
		/// </summary>
		public virtual Int32? TransactionType { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "Invoices";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                { 
                   Invoice test = new Invoice();
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

		
		public virtual List<Invoice> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Invoice>().ToList();
                   }



		public virtual Invoice GetByID() {  
                     return (Invoice)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Invoice> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Invoice>().ToList();
                  }
	#endregion
	}
}