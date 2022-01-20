using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Egx.EgxDataAccess;

namespace Egx.EgxBusiness.Inventory
{

	[Serializable]
	public class UnionsTransactions
	{
	            public DataAccess da;
                public SystemMessage sm;
		#region Construction
		public UnionsTransactions()
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
		/// Gets or sets the TR_DATE value.
		/// </summary>
		public virtual DateTime? TR_DATE { get; set; }

		/// <summary>
		/// Gets or sets the FROM_DATE value.
		/// </summary>
		public virtual DateTime? FROM_DATE { get; set; }

		/// <summary>
		/// Gets or sets the TO_DATE value.
		/// </summary>
		public virtual DateTime? TO_DATE { get; set; }

		/// <summary>
		/// Gets or sets the UID value.
		/// </summary>
		public virtual Int32? UID { get; set; }

		/// <summary>
		/// Gets or sets the COMMISION value.
		/// </summary>
		public virtual Double? COMMISION { get; set; }

		/// <summary>
		/// Gets or sets the AMOUNT value.
		/// </summary>
		public virtual Decimal? AMOUNT { get; set; }

		/// <summary>
		/// Gets or sets the PAID value.
		/// </summary>
		public virtual Decimal? PAID { get; set; }

		/// <summary>
		/// Gets or sets the REC_NAME value.
		/// </summary>
		public virtual String REC_NAME { get; set; }

		/// <summary>
		/// Gets or sets the COMMENT value.
		/// </summary>
		public virtual String COMMENT { get; set; }

		/// <summary>
		/// Gets or sets the IS_BANK value.
		/// </summary>
		public virtual Boolean? IS_BANK { get; set; }

		/// <summary>
		/// Gets or sets the USR_NAME value.
		/// </summary>
		public virtual String USR_NAME { get; set; }
		#endregion

		#region Overrides
		public override String ToString()
		{
			return "UnionsTransactions" ;
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
		public virtual List<UnionsTransactions> Search() 
                   {            
                      return new DataAccess().Search(this).Cast<UnionsTransactions>().ToList();
                   }



		public virtual UnionsTransactions GetByID() {  
                     return (UnionsTransactions)new DataAccess().GetByID(ID.Value, this);
                  }


	        public virtual List<UnionsTransactions> GetAll() 
                  { 
                             return new DataAccess().GetTopItems(100,this).Cast<UnionsTransactions>().ToList();
                  }
		#endregion
	}
}