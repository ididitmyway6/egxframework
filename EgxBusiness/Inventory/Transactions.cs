using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class Transactions
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public Transactions()
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
        /// Gets or sets the ID value.
        /// </summary>
        public virtual Int32? TRANS_TYPE { get; set; }
        /// <summary>
        /// Gets or sets the ID value.
        /// </summary>
        public virtual Int32? REL_ACCOUNT { get; set; }

		/// <summary>
		/// Gets or sets the TRANS_NAME value.
		/// </summary>
		public virtual String TRANS_NAME { get; set; }

		/// <summary>
		/// Gets or sets the TRANS_DESC value.
		/// </summary>
		public virtual String TRANS_DESC { get; set; }

		/// <summary>
		/// Gets or sets the AMOUNT value.
		/// </summary>
		public virtual Decimal? AMOUNT { get; set; }

		/// <summary>
		/// Gets or sets the TRANS_DATE value.
		/// </summary>
		public virtual DateTime? TRANS_DATE { get; set; }

		/// <summary>
		/// Gets or sets the TRANS_USER value.
		/// </summary>
		public virtual String TRANS_USER_NAME { get; set; }

        public virtual Boolean? AMOUNT_BY_POINTS { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "Transactions" ;
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
		public virtual List<Transactions> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<Transactions>().ToList();
                   }



		public virtual Transactions GetByID() {  
                     return (Transactions)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<Transactions> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<Transactions>().ToList();
                  }
		#endregion
	}
}