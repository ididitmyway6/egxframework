using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Casher
{
	

	[Serializable]
	public class AccountTransaction
	{
                public DataAccess da;
                public SystemMessage sm;
		#region Construction
		
		public AccountTransaction()
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
		/// Gets or sets the Type value.
		/// </summary>
		public virtual String Type { get; set; }

		/// <summary>
		/// Gets or sets the Amount value.
		/// </summary>
		public virtual Double? Amount { get; set; }

		/// <summary>
		/// Gets or sets the SupType value.
		/// </summary>
		public virtual Int32? SupType { get; set; }

		/// <summary>
		/// Gets or sets the Shift value.
		/// </summary>
		public virtual Int32 Shift { get; set; }

		/// <summary>
		/// Gets or sets the usr value.
		/// </summary>
		public virtual Int32 usr { get; set; }

		/// <summary>
		/// Gets or sets the Date value.
		/// </summary>
		public virtual DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets the Remark value.
		/// </summary>
		public virtual String Remark { get; set; }

		/// <summary>
		/// Gets or sets the OrderID value.
		/// </summary>
		public virtual Int32? OrderID { get; set; }

		/// <summary>
		/// Gets or sets the Closed value.
		/// </summary>
		public virtual Int32? Closed { get; set; }
		#endregion

		#region Overrides
		
		public override String ToString()
		{
			return "AccountTransactions";
		}

		
		#endregion

		#region CRUD Methods
		
		public virtual SystemMessage Insert() 
                { 
                   AccountTransaction test = new AccountTransaction();
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

		
		public virtual List<AccountTransaction> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<AccountTransaction>().ToList();
                   }



		public virtual AccountTransaction GetByID() {  
                     return (AccountTransaction)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<AccountTransaction> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<AccountTransaction>().ToList();
                  }
	#endregion
	}
}