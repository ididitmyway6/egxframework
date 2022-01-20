using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;
using Egx.EgxBusiness;

namespace EgxCash.BLL
{

	[Serializable]
	public class TransactionDetails
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public TransactionDetails()
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
		/// Gets or sets the TR_ID value.
		/// </summary>
		public virtual String TR_NAME { get; set; }

		/// <summary>
		/// Gets or sets the DEBIT_ACCOUNT value.
		/// </summary>
		public virtual Int32? DEBIT_ACCOUNT { get; set; }

		/// <summary>
		/// Gets or sets the CREDIT_ACCOUNT value.
		/// </summary>
		public virtual Int32? CREDIT_ACCOUNT { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "TransactionDetails" ;
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
		public virtual List<TransactionDetails> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<TransactionDetails>().ToList();
                   }



		public virtual TransactionDetails GetByID() {  
                     return (TransactionDetails)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<TransactionDetails> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<TransactionDetails>().ToList();
                  }
		#endregion
	}
}