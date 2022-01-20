using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.EgxSMS
{

	[Serializable]
	public class AccountTransactions
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public AccountTransactions()
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
		/// Gets or sets the ACC_ID value.
		/// </summary>
		public virtual Int32? ACC_ID { get; set; }

		/// <summary>
		/// Gets or sets the TR_TYPE value.
		/// </summary>
		public virtual Int32? TR_TYPE { get; set; }

		/// <summary>
		/// Gets or sets the TR_DATE value.
		/// </summary>
		public virtual DateTime? TR_DATE { get; set; }

		/// <summary>
		/// Gets or sets the AMOUNT value.
		/// </summary>
		public virtual Decimal? AMOUNT { get; set; }

		/// <summary>
		/// Gets or sets the SUB_ACC_ID value.
		/// </summary>
		public virtual Int32? SUB_ACC_ID { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "AccountTransactions" ;
		}
		#endregion

		#region CRUD Methods
			
		public virtual SystemMessage Insert() 
                { 
                   AccountTransactions test = new AccountTransactions();
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
		public virtual List<AccountTransactions> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<AccountTransactions>().ToList();
                   }



		public virtual AccountTransactions GetByID() {  
                     return (AccountTransactions)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<AccountTransactions> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<AccountTransactions>().ToList();
                  }
		#endregion
	}
}