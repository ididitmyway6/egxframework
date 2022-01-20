using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class NM_Transactions
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public NM_Transactions()
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
		/// Gets or sets the TRANS_TYPE value.
		/// </summary>
		public virtual String TRANS_TYPE { get; set; }

		/// <summary>
		/// Gets or sets the TRANS_DATE value.
		/// </summary>
		public virtual DateTime? TRANS_DATE { get; set; }

		/// <summary>
		/// Gets or sets the CUSTOMER_ID value.
		/// </summary>
		public virtual Int32? CUSTOMER_ID { get; set; }

		/// <summary>
		/// Gets or sets the CUSTOMER_NAME value.
		/// </summary>
		public virtual String CUSTOMER_NAME { get; set; }

		/// <summary>
		/// Gets or sets the AMOUNT value.
		/// </summary>
		public virtual Decimal? AMOUNT { get; set; }

        public virtual DateTime? EntryDate { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "NM_Transactions" ;
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
		public virtual List<NM_Transactions> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<NM_Transactions>().ToList();
                   }



		public virtual NM_Transactions GetByID() {  
                     return (NM_Transactions)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<NM_Transactions> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<NM_Transactions>().ToList();
                  }
		#endregion
	}
}